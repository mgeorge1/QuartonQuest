using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using UnityEngine;

public class GameCoreController : MonoBehaviour
{
    public static GameCoreController Instance { get; set; }
    public Board board;
    public IOpponent Opponent;
    public string OnDeckPiece { get; set; }
    public string LastTileClicked { get; set; }
    public bool IsGameOver
    {
        get
        {
            return (
                CurrentTurn == GameTurnState.PLAYERWON ||
                CurrentTurn == GameTurnState.OPPONENTWON ||
                CurrentTurn == GameTurnState.PLAYERFORFEIT ||
                CurrentTurn == GameTurnState.OPPONENTFORFEIT ||
                CurrentTurn == GameTurnState.GAMETIED
            );
        }
    }
    public bool IsPlayerTurn
    {
        get
        {
            return (
                CurrentTurn == GameTurnState.PLAYER || 
                CurrentTurn == GameTurnState.PLAYERCHOOSEPIECE ||
                CurrentTurn == GameTurnState.PLAYERCHOOSETILE ||
                CurrentTurn == GameTurnState.PLAYERDONE ||
                CurrentTurn == GameTurnState.OPPONENTFORFEIT
            );
        }
    }
    public bool IsPlayerDone { get 
        {
            return (
                CurrentTurn == GameTurnState.PLAYERDONE ||
                CurrentTurn == GameTurnState.PLAYERWON ||
                CurrentTurn == GameTurnState.GAMETIED
            );
        } 
    }
    public bool RematchRequestPending = false;
    public Dictionary<String, int> PieceNumberMap { 
        get
        {
            return model.PieceNumberMap;
        }
    }
    public Dictionary<int, String> NumberPieceMap
    {
        get
        {
            return model.NumberPieceMap;
        }
    }

    private GameCoreModel model = new GameCoreModel();
    public delegate void GameOverListener();
    public event GameOverListener GameOver;
    private const string NOSELECTION = "";
    public enum GameTurnState { 
        NONE, 
        PLAYER, 
        OPPONENT, 
        PLAYERWON, 
        OPPONENTWON,
        PLAYERFORFEIT,
        OPPONENTFORFEIT,
        GAMEWON, 
        GAMETIED, 
        PLAYERDONE, 
        PLAYERCHOOSEPIECE, 
        PLAYERCHOOSETILE 
    };

    private GameTurnState currentTurn = GameTurnState.NONE;
    public GameTurnState CurrentTurn
    {
        get
        {
            return currentTurn;
        }
        set
        {
            currentTurn = value;
            if (IsGameOver)
                GameOver?.Invoke();
        }
    }

    public void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GameCoreModel.GameWon += GameOverState;
    }

    void ResetGameData()
    {
        model.NewGame();
        DisableTiles();
        DisablePieces();
        OnDeckPiece = NOSELECTION;
        LastTileClicked = NOSELECTION;
    }

    public IEnumerator PlayGame(bool playerGoesFirst)
    {
        CurrentTurn = (playerGoesFirst) ? GameTurnState.PLAYER : GameTurnState.OPPONENT;
        return PlayGame();
    }

    public IEnumerator PlayGame()
    {
        ResetGameData();

        if (CurrentTurn == GameTurnState.NONE)
            yield return StartCoroutine(PickFirstTurn());

        yield return StartCoroutine(PickFirstPiece());

        while(!IsGameOver)
        {
            if(CurrentTurn==GameTurnState.PLAYER)
            {
                yield return StartCoroutine(PlayerTurn());
                Debug.Log("Player has made their move");
                SwapTurn();
            }
            else if(CurrentTurn==GameTurnState.OPPONENT)
            {
                yield return StartCoroutine(OpponentTurn());
                Debug.Log("Opponent has made their move");
                SwapTurn();
            }
        }
    }

    IEnumerator PlayerTurn()
    {
        Debug.Log("Enabling tiles for player and waiting for input");
        EnableTiles();
        DisablePieces();
        CurrentTurn = GameTurnState.PLAYERCHOOSETILE;
        while (!IsPlayerDone) yield return null;
        Opponent.SendMove();
    }

    IEnumerator PlayerFirstTurn()
    {
        Debug.Log("Player picking first piece");
        EnablePieces();
        DisableTiles();
        CurrentTurn = GameTurnState.PLAYERCHOOSEPIECE;
        while (!IsPlayerDone) yield return null;
        Opponent.SendFirstMove();
    }

    IEnumerator OpponentTurn()
    {
        DisableTiles();
        DisablePieces();
        yield return Opponent.WaitForTurn();

        model.Move(Opponent.NextMove.Tile, OnDeckPiece);
        board.MovePiece(Opponent.NextMove.Tile);
        yield return new WaitForSeconds(.5f);

        if (!IsGameOver)
        {
            board.MoveOnDeck(Opponent.NextMove.OnDeckPiece);
            OnDeckPiece = Opponent.NextMove.OnDeckPiece;
        }
    }

    IEnumerator OpponentFirstTurn()
    {
        Debug.Log("Opponent picking first piece");
        DisableTiles();
        DisablePieces();
        yield return Opponent.WaitForPickFirstPiece();
        board.MoveOnDeck(Opponent.NextMove.OnDeckPiece);
        OnDeckPiece = Opponent.NextMove.OnDeckPiece;
    }

    IEnumerator PickFirstPiece()
    {
        switch (CurrentTurn)
        {
            case GameTurnState.PLAYER:
                yield return PlayerFirstTurn();
                SwapTurn();
                break;
            case GameTurnState.OPPONENT:
                yield return OpponentFirstTurn();
                SwapTurn();
                break;
        }
    }

    public void EnableTiles()
    {
        Tile.OnClickTile += SelectTile;
        board.EnableTileClicking();
    }

    public void DisableTiles()
    {
        Tile.OnClickTile -= SelectTile;
        board.DisableTileClicking();
    }

    public void EnablePieces()
    {
        Piece.OnClickPiece += SelectPiece;
        board.EnablePieceClicking();
    }

    public void DisablePieces()
    {
        Piece.OnClickPiece -= SelectPiece;
        board.DisablePieceClicking();
    }

    public void GameOverState()
    {
        DisablePieces();
        DisableTiles();

        if(!model.isGameTied)
        {
            Debug.Log((IsPlayerTurn ? "Player" : "Opponent") + " Has Won");

            if (IsPlayerTurn)
                CurrentTurn = GameTurnState.PLAYERWON;
            else
                CurrentTurn = GameTurnState.OPPONENTWON;
        }
        else
        {
            Debug.Log("GAME HAS TIED");
            CurrentTurn = GameTurnState.GAMETIED;
        }

        GameOver?.Invoke();
    }

    public void SwapTurn()
    {
        if (IsGameOver)
            return;

        CurrentTurn = (IsPlayerTurn ? GameTurnState.OPPONENT : GameTurnState.PLAYER);
        Debug.Log((IsPlayerTurn ? "Player" : "Opponent") + "'s Turn");
    }

    private IEnumerator PickFirstTurn()
    {
        if (Opponent.IsMaster)
        {
            // Wait for Opponent to communicate the first turn
            Debug.Log("Opponent is picking first turn");
            while (CurrentTurn == GameTurnState.NONE) yield return null;
        } 
        else
        {
            Debug.Log("Picking first turn");
            System.Random rand = new System.Random();
            CurrentTurn = rand.Next(0, 2) == 1 ? GameTurnState.PLAYER : GameTurnState.OPPONENT;
            Debug.Log((IsPlayerTurn ? "Player" : "Opponent") + "'s Turn");
            Opponent.SendFirstTurn((IsPlayerTurn ? GameTurnState.OPPONENT : GameTurnState.PLAYER));
        }
    }

    public void SelectPiece(string pieceName)
    {
        Debug.Log("Piece selected - " + pieceName);
        OnDeckPiece = pieceName;
        CurrentTurn = GameTurnState.PLAYERDONE;
    }

    public void SelectTile (string tileName)
    {
        Debug.Log("Attempting to make move - " + OnDeckPiece + " to " + tileName);
        DisableTiles();
        EnablePieces();
        model.Move(tileName, OnDeckPiece);
        LastTileClicked = tileName;
        OnDeckPiece = NOSELECTION;

        if (!IsGameOver)
            CurrentTurn = GameTurnState.PLAYERCHOOSEPIECE;
    }

    public void Forfeit()
    {
        Opponent.SendForfeit();
        CurrentTurn = GameTurnState.PLAYERFORFEIT;
    }

    public void RequestRematchFromOpponent()
    {
        if (RematchRequestPending)
        {
            Debug.Log("Rematch already pending. Master client will initiate rematch.");
            if (!Opponent.IsMaster)
                Opponent.ReplayGame();
        }
        else
        {
            RematchRequestPending = true;
            Opponent.RequestRematch();
        }
    }

    public void RequestRematchFromPlayer()
    {
        if (RematchRequestPending)
        {
            Debug.Log("Rematch already pending. Master client will initiate rematch.");
            if (!Opponent.IsMaster)
                Opponent.ReplayGame();
        } 
        else
        {
            RematchRequestPending = true;
            GUIController.Instance.RequestRematchFromPlayer();
        }
    }

    public IEnumerator Disconnect()
    {
        yield return Opponent.Disconnect();
    }

    public void ResetGameScene()
    {
        ResetGameData();
        GUIController.Instance.ReloadCurrentScene();
    }

    public void ReplayGame()
    {
        Opponent.ReplayGame();
    }

    public string[,] GetBoard()
    {
        return model.GameBoard;
    }

    //Only for console application version
    public void PrintBoard()
    {
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                Debug.Log(Convert.ToString(model.GameBoard[x, y]));
            }
            Debug.Log("");
        }

    }

    public HashSet<string> GetPlayablePiecesList()
    {
        return model.PlayablePieces;
    }

}

