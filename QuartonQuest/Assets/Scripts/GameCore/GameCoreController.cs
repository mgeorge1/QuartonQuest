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
                CurrentTurn == GameTurnState.PLAYERDONE
            );
        }
    }
    public bool IsPlayerDone { get { return CurrentTurn == GameTurnState.PLAYERDONE; } }
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
    private const string NOSELECTION = "";
    public enum GameTurnState { 
        NONE, 
        PLAYER, 
        OPPONENT, 
        PLAYERWON, 
        OPPONENTWON, 
        GAMEWON, 
        GAMETIED, 
        PLAYERDONE, 
        PLAYERCHOOSEPIECE, 
        PLAYERCHOOSETILE 
    };
    public GameTurnState CurrentTurn { get; set; } = GameTurnState.NONE;

    public void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GameCoreModel.GameWon += GameOverState;
    }

    void ResetGame()
    {
        model.NewGame();
        DisableTiles();
        DisablePieces();
        CurrentTurn = GameTurnState.NONE;
        OnDeckPiece = NOSELECTION;
        LastTileClicked = NOSELECTION;
    }

    public IEnumerator PlayGame ()
    {
        ResetGame();
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
        yield return Opponent.SendMove();
    }

    IEnumerator PlayerFirstTurn()
    {
        Debug.Log("Player picking first piece");
        EnablePieces();
        DisableTiles();
        CurrentTurn = GameTurnState.PLAYERCHOOSEPIECE;
        while (!IsPlayerDone) yield return null;
        yield return Opponent.SendFirstMove();
    }

    IEnumerator OpponentTurn()
    {
        DisableTiles();
        DisablePieces();
        yield return Opponent.WaitForTurn("A2", OnDeckPiece);

        model.Move(Opponent.NextMove.Tile, OnDeckPiece);
        board.MovePiece(Opponent.NextMove.Tile);
        
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
            yield return Opponent.WaitForPickFirstTurn((IsPlayerTurn ? GameTurnState.OPPONENT : GameTurnState.PLAYER));
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
        CurrentTurn = GameTurnState.PLAYERCHOOSEPIECE;
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

