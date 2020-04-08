using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;
using System.Threading.Tasks;
public class AIController : MonoBehaviour, IOpponent
{
    static QuartoSearchTree quartoAI = new QuartoSearchTree();
    public Move NextMove { get; set; } = new Move();
    public bool IsMaster { get { return false; } }

    private const string EMPTYSLOT = "[]";

    public static AI.moveData AIMoveData;

    
    public IEnumerator AIThreadProcedure()
    {
        Debug.Log("AI thread is running...");
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

        string onDeckPiece = GameCoreController.Instance.OnDeckPiece;
        int pieceToFind = GameCoreController.Instance.PieceNumberMap[onDeckPiece];
        int difficulty = 3;
        Task<AI.moveData> AITask = Task.Run(() => quartoAI.generateTree(newBoard, pieceToFind, pieces, difficulty));

        while (AITask.Status != TaskStatus.RanToCompletion)
            yield return null;

        AIMoveData = AITask.Result;
    }

    public IEnumerator WaitForPickFirstPiece()
    {
        // Change to pick random piece
        int rand = Random.Range(0, GameCoreController.Instance.GetPlayablePiecesList().Count);
        NextMove.OnDeckPiece = GameCoreController.Instance.NumberPieceMap[rand];
        return null;
    }

    public IEnumerator WaitForTurn()
    {
        yield return AIThreadProcedure();

        NextMove.OnDeckPiece = AIMoveData.pieceToPlay;
        NextMove.Tile = AIMoveData.lastMoveOnBoard;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("AIController initiated");
        
    }

    private static void intitializeAiPieceList(HashSet<string> gameCorePieceList, ref AI.Piece[] aiPieceList, Dictionary<string, int> PieceNumberMap)
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

    public void SendFirstTurn(GameCoreController.GameTurnState turn)
    {
        // Never called by the AI
    }

    public void SendFirstMove()
    {
        // AI doesn't care what is in this function
        // It will get the Board state later
    }

    public void SendMove()
    {
        // AI doesn't care what is in this function
        // It will get the board state later
    }

    public void SendForfeit()
    {
        //Ignore forfeit in the AI
    }

    public void RequestRematch()
    {
        GameCoreController.Instance.ResetGameScene();
    }

    public IEnumerator Disconnect()
    {
        // Possibly end thread execution here
        return null;
    }

    public void ReplayGame()
    {
        // AI ignores this for now
    }
}
