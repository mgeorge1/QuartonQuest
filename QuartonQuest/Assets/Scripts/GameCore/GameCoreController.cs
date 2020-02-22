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
    public Dictionary<String, int> PieceNumberMap { 
        get
        {
            return model.PieceNumberMap;
        }
    }

    private GameCoreModel model = new GameCoreModel();
    private string pieceToBePlaced;
    
    private enum GameTurnState { NONE, PLAYER, OPPONENT, PLAYERWON, OPPONENTWON, GAMEWON, GAMETIED, PLAYERDONE};
    GameTurnState currentTurn = GameTurnState.NONE;

    private class TestOpponent : IOpponent
    {
        public Move NextMove { get; set; } = new Move();

        public IEnumerator WaitForTurn(GameCoreController instance, string lastTile, string onDeckPiece)
        {
            NextMove.Piece = "";
            NextMove.Tile = "A1";
            NextMove.OnDeckPiece = "A2";
            yield return new WaitForSeconds(5);
        }

        public IEnumerator PickFirstPiece(GameCoreController instance)
        {
            NextMove.OnDeckPiece = "A4";
            yield return new WaitForSeconds(2);
        }

        public IEnumerator GameOver(bool didWin)
        {
            return null;
        }
    }

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
        PickFirstTurn();
        yield return StartCoroutine(PickFirstPiece());
        while(currentTurn != GameTurnState.GAMEWON && currentTurn!=GameTurnState.GAMETIED)
        {
            if(currentTurn==GameTurnState.PLAYER)
            {
                yield return StartCoroutine(PlayerTurn());
                Debug.Log("Player has made their move");
                SwapTurn();
            }
            else if(currentTurn==GameTurnState.OPPONENT)
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
        while (currentTurn == GameTurnState.PLAYER) yield return null;
    }

    IEnumerator PlayerFirstTurn()
    {
        Debug.Log("Player picking first piece");
        EnablePieces();
        DisableTiles();
        while (currentTurn == GameTurnState.PLAYER) yield return null;
    }

    IEnumerator OpponentTurn()
    {
        DisableTiles();
        DisablePieces();
        yield return Opponent.WaitForTurn(Instance, "A2", pieceToBePlaced);

        model.Move(Opponent.NextMove.Tile, Opponent.NextMove.OnDeckPiece);
        board.MovePiece(Opponent.NextMove.Tile);
        board.MoveOnDeck(Opponent.NextMove.OnDeckPiece);
    }

    IEnumerator OpponentFirstTurn()
    {
        Debug.Log("Opponent picking first piece");
        DisableTiles();
        DisablePieces();
        yield return Opponent.PickFirstPiece(Instance);
        board.MoveOnDeck(Opponent.NextMove.OnDeckPiece);
        pieceToBePlaced = Opponent.NextMove.OnDeckPiece;
    }

    IEnumerator PickFirstPiece()
    {
        switch (currentTurn)
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
        Debug.Log((currentTurn == GameTurnState.PLAYER ? "Player" : "Opponent") + " Choose a Piece");
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
        if(!model.isGameTied)
        {
            Debug.Log((currentTurn == GameTurnState.PLAYER || currentTurn == GameTurnState.PLAYERDONE ? "Player" : "Opponent") + " Has Won");
            currentTurn = GameTurnState.GAMEWON;
        }
        else
        {
            Debug.Log("GAME HAS TIED");
            currentTurn = GameTurnState.GAMETIED;
        }
    }

    public void SwapTurn()
    {
        currentTurn = currentTurn == GameTurnState.OPPONENT ? GameTurnState.PLAYER : GameTurnState.OPPONENT;
        Debug.Log((currentTurn == GameTurnState.PLAYER || currentTurn == GameTurnState.PLAYERDONE ? "Player" : "Opponent") + "'s Turn");
    }

    public void PickFirstTurn()
    {
        //Can be changed to manual turn picking later
        System.Random rand = new System.Random();
        currentTurn = rand.Next(0, 2) == 1 ? GameTurnState.PLAYER : GameTurnState.OPPONENT;
        Debug.Log((currentTurn == GameTurnState.PLAYER ? "Player" : "Opponent") + "'s Turn");
    }

    public void SelectPiece(string pieceName)
    {
        pieceToBePlaced = pieceName;
        currentTurn = GameTurnState.PLAYERDONE;
    }

    public void SelectTile (string tileName)
    {
        model.Move(tileName, pieceToBePlaced);
        pieceToBePlaced = "";
        DisableTiles();
        EnablePieces();
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

