using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using UnityEngine;

class GameCoreController : MonoBehaviour
{
    public static GameCoreController instance;
    GameCoreModel model = new GameCoreModel();
    public Board board;

    string PieceToBePlaced;
    
    private enum GameTurnState { NONE, PLAYER1, PLAYER2, GAMEWON, GAMETIED};
    GameTurnState currentTurn = GameTurnState.NONE;

    public void Awake()
    {
        instance = this;
    }

    IEnumerator Start()
    {
        GameCoreModel.GameWon += GameOverState;
        yield return PlayGame();
    }

    IEnumerator PlayGame ()
    {
        model.NewGame();
        PickFirstTurn();
        yield return PickFirstPiece();
        SwapTurn();
        while(currentTurn != GameTurnState.GAMEWON && currentTurn!=GameTurnState.GAMETIED)
        {
            if(currentTurn==GameTurnState.PLAYER1)
            {
                yield return Player1Turn();
                Debug.Log("Player 1 has made their move");
                SwapTurn();
            }
            else if(currentTurn==GameTurnState.PLAYER2)
            {
                yield return Player2Turn();
                Debug.Log("Player 2 has made their move");
                SwapTurn();
            }
        }
    }

    IEnumerator Player1Turn()
    {
        Debug.Log(Convert.ToString(currentTurn) + " Place Piece");
        EnableTiles();
        Debug.Log(Convert.ToString(currentTurn) + " Select a piece for other player");
        EnablePieces();
        return null;
    }

    IEnumerator Player2Turn()
    {
        Debug.Log(Convert.ToString(currentTurn) + " Place Piece");
        EnableTiles();
        Debug.Log(Convert.ToString(currentTurn) + " Select a piece for other player");
        EnablePieces();
        return null;
    }

    IEnumerator PickFirstPiece()
    {
        EnablePieces();
        Debug.Log(Convert.ToString(currentTurn)+ " Choose a Piece");
        return null;
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
        board.DisableTileClicking();
    }


    public void GameOverState()
    {
        if(!model.isGameTied)
        {
            Debug.Log(Convert.ToString(currentTurn) + " Has Won");
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
        currentTurn = currentTurn == GameTurnState.PLAYER2 ? GameTurnState.PLAYER1 : GameTurnState.PLAYER2;
        Debug.Log("Player " + Convert.ToString(currentTurn) + "'s Turn");
    }

    public void PickFirstTurn()
    {
        //Can be changed to manual turn picking later
        System.Random rand = new System.Random();
        currentTurn = rand.Next(0, 2) == 1 ? GameTurnState.PLAYER1 : GameTurnState.PLAYER2;
        Debug.Log("Player " + Convert.ToString(currentTurn) + "'s Turn");
    }

    public void SelectPiece(string pieceName)
    {
        PieceToBePlaced = pieceName;
        SwapTurn();
    }

    public void SelectTile (string tileName)
    {
        model.Move(tileName, PieceToBePlaced);
        PieceToBePlaced = "";
    }

    /*
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
    */
}

