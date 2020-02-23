using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public class AIController : MonoBehaviour, IOpponent
{
    QuartoSearchTree quartoAI = new QuartoSearchTree();
    public Move NextMove { get; set; } = new Move();
    public bool IsMaster { get { return false; } }

    private const string EMPTYSLOT = "[]";

    public IEnumerator GameOver(bool didWin)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator WaitForPickFirstPiece()
    {
        // Change to pick random piece
        int rand = Random.Range(0, GameCoreController.Instance.GetPlayablePiecesList().Count);
        NextMove.OnDeckPiece = GameCoreController.Instance.NumberPieceMap[rand];

        return null;
    }

    public IEnumerator WaitForTurn(string lastTile, string onDeckPiece)
    {
        string[,] board = GameCoreController.Instance.GetBoard();
        string[] newBoard = new string[board.Length];
        AI.Piece[] pieces = new AI.Piece[GameCoreController.Instance.PieceNumberMap.Count];
        

        int newBoardCounter = 0;

        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                newBoard[newBoardCounter] = board[i, j] == EMPTYSLOT ? null : board[i, j];
                newBoardCounter++;
            }
        }
        intitializeAiPieceList(GameCoreController.Instance.GetPlayablePiecesList(), ref pieces, GameCoreController.Instance.PieceNumberMap);

        AI.moveData moveData = quartoAI.generateTree(2, newBoard, GameCoreController.Instance.PieceNumberMap[onDeckPiece], pieces);
        NextMove.OnDeckPiece = moveData.pieceToPlay;
        NextMove.Tile = moveData.lastMoveOnBoard;

        return null; 
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("AIController initiated");
    }

    private void intitializeAiPieceList(HashSet<string> gameCorePieceList, ref AI.Piece[] aiPieceList, Dictionary<string, int> PieceNumberMap)
    {
        aiPieceList[0].setValues("A1", false);
        aiPieceList[1].setValues("A2", false);
        aiPieceList[2].setValues("A3", false);
        aiPieceList[3].setValues("A4", false);
        aiPieceList[4].setValues("B1", false);
        aiPieceList[5].setValues("B2", false);
        aiPieceList[6].setValues("B3", false);
        aiPieceList[7].setValues("B4", false);
        aiPieceList[8].setValues("C1", false);
        aiPieceList[9].setValues("C2", false);
        aiPieceList[10].setValues("C3", false);
        aiPieceList[11].setValues("C4", false);
        aiPieceList[12].setValues("D1", false);
        aiPieceList[13].setValues("D2", false);
        aiPieceList[14].setValues("D3", false);
        aiPieceList[15].setValues("D4", false);

        foreach (string key in gameCorePieceList)
        {
            aiPieceList[PieceNumberMap[key]].setPlayable(true);
        }
    }

    public IEnumerator WaitForPickFirstTurn(GameCoreController.GameTurnState turn)
    {
        // Never called by the AI
        return null;
    }

    public IEnumerable SendFirstMove()
    {
        // AI doesn't care what is in this function
        // It will get the Board state later
        return null;
    }

    public IEnumerable SendMove()
    {
        // AI doesn't care what is in this function
        // It will get the board state later
        return null;
    }
}
