using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public class AIController : MonoBehaviour, IOpponent
{
    QuartoSearchTree quartoAI = new QuartoSearchTree();
    public Move NextMove { get; set; } = new Move();
    private const string EMPTYSLOT = "[]";

    public IEnumerator GameOver(bool didWin)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator PickFirstPiece(GameCoreController instance)
    {
        NextMove.OnDeckPiece = "A1";

        return null;
    }

    public IEnumerator WaitForTurn(GameCoreController instance, string lastTile, string onDeckPiece)
    {
        string[,] board = instance.GetBoard();
        string[] newBoard = new string[board.Length];
        AI.Piece[] pieces = new AI.Piece[instance.PieceNumberMap.Count];
        

        int newBoardCounter = 0;

        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                newBoard[newBoardCounter] = board[i, j] == EMPTYSLOT ? null : board[i, j];
                newBoardCounter++;
            }
        }
        intitializeAiPieceList(instance.GetPlayablePiecesList(), ref pieces, instance.PieceNumberMap);

        AI.moveData moveData = quartoAI.generateTree(2, newBoard, instance.PieceNumberMap[onDeckPiece], pieces);
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
}
