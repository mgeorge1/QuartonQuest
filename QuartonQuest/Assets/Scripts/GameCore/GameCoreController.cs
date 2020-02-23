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
    
    public enum GameTurnState { NONE, PLAYER, OPPONENT, PLAYERWON, OPPONENTWON, GAMEWON, GAMETIED, PLAYERDONE };
    public GameTurnState CurrentTurn { get; set; } = GameTurnState.NONE;

    //private class TestOpponent : IOpponent
    //{
    //    public Move NextMove { get; set; } = new Move();

    //    public IEnumerator WaitForTurn(GameCoreController instance, string lastTile, string onDeckPiece)
    //    {
    //        NextMove.Piece = "";
    //        NextMove.Tile = "A1";
    //        NextMove.OnDeckPiece = "A2";
    //        yield return new WaitForSeconds(5);
    //    }

    //    public IEnumerator PickFirstPiece(GameCoreController instance)
    //    {
    //        NextMove.OnDeckPiece = "A4";
    //        yield return new WaitForSeconds(2);
    //    }

    //    public IEnumerator GameOver(bool didWin)
    //    {
    //        return null;
    //    }
    //}

    public void Awake()
    {
        Instance = this;
        //Opponent = new TestOpponent();
    }

    void Start()
    {
        GameCoreModel.GameWon += GameOverState;
    }

    public IEnumerator PlayGame ()
    {
        model.NewGame();
        DisableTiles();
        DisablePieces();
        //EnablePieces();
        //EnableTiles();
        yield return StartCoroutine(PickFirstTurn());
        yield return StartCoroutine(PickFirstPiece());
        while(CurrentTurn != GameTurnState.GAMEWON && CurrentTurn!=GameTurnState.GAMETIED)
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
        while (CurrentTurn == GameTurnState.PLAYER) yield return null;
        yield return Opponent.SendMove();
    }

    IEnumerator PlayerFirstTurn()
    {
        Debug.Log("Player picking first piece");
        EnablePieces();
        DisableTiles();
        while (CurrentTurn == GameTurnState.PLAYER) yield return null;
        yield return Opponent.SendFirstMove();
    }

    IEnumerator OpponentTurn()
    {
        DisableTiles();
        DisablePieces();
        yield return Opponent.WaitForTurn("A2", OnDeckPiece);

        model.Move(Opponent.NextMove.Tile, OnDeckPiece);
        board.MovePiece(Opponent.NextMove.Tile);
        
        if (CurrentTurn != GameTurnState.GAMEWON && CurrentTurn != GameTurnState.GAMETIED)
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
        //Debug.Log((CurrentTurn == GameTurnState.PLAYER ? "Player" : "Opponent") + " Choose a Piece");
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
            Debug.Log((CurrentTurn == GameTurnState.PLAYER || CurrentTurn == GameTurnState.PLAYERDONE ? "Player" : "Opponent") + " Has Won");
            CurrentTurn = GameTurnState.GAMEWON;
        }
        else
        {
            Debug.Log("GAME HAS TIED");
            CurrentTurn = GameTurnState.GAMETIED;
        }
    }

    public void SwapTurn()
    {
        if (CurrentTurn == GameTurnState.GAMEWON || CurrentTurn == GameTurnState.GAMETIED)
            return;

        CurrentTurn = CurrentTurn == GameTurnState.OPPONENT ? GameTurnState.PLAYER : GameTurnState.OPPONENT;
        Debug.Log((CurrentTurn == GameTurnState.PLAYER || CurrentTurn == GameTurnState.PLAYERDONE ? "Player" : "Opponent") + "'s Turn");
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
            Debug.Log((CurrentTurn == GameTurnState.PLAYER ? "Player" : "Opponent") + "'s Turn");
            yield return Opponent.WaitForPickFirstTurn((CurrentTurn == GameTurnState.OPPONENT ? GameTurnState.PLAYER : GameTurnState.OPPONENT));
        }
    }

    public void SelectPiece(string pieceName)
    {
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
        OnDeckPiece = "";
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

