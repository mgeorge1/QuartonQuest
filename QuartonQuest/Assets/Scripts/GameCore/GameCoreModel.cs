using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Collections;
using UnityEngine;

public class GameCoreModel
{
    public bool GameOver { get; set; } = false;

    public delegate void GameOverEvent();
    public static event GameOverEvent GameWon;

    public bool isGameTied = false;

    Dictionary<string, int[]> pieceList;
    Dictionary<string, Point> boardSlotList;
    public Dictionary<string, int> PieceNumberMap { get; private set; }
    public Dictionary<int, String> NumberPieceMap { get; private set; }
    const int MAXATTRIBUTEVARIANCE = 1;
    
    public string[,] GameBoard { get; set; } = new string[4, 4];

    public HashSet<string> PlayablePieces = new HashSet<string>();

    public GameCoreModel()
    {
        initializePieceList();
        InitializeBoardSlotList();
        InitializePieceNumberMap();
        InitializeNumberPieceMap();
    }

    void initializePieceList()
    {
        pieceList = new Dictionary<String, int[]>
        {
            {"[]", new int[] {2,2,2,2} },

            {"A1", new int[] {0, 0, 0, 0}},
            {"A2", new int[] {0, 0, 0, 1}},
            {"A3", new int[] {0, 0, 1, 0}},
            {"A4", new int[] {0, 0, 1, 1}},

            {"B1", new int[] {0, 1, 0, 0}},
            {"B2", new int[] {0, 1, 0, 1}},
            {"B3", new int[] {0, 1, 1, 0}},
            {"B4", new int[] {0, 1, 1, 1}},

            {"C1", new int[] {1, 0, 0, 0}},
            {"C2", new int[] {1, 0, 0, 1}},
            {"C3", new int[] {1, 0, 1, 0}},
            {"C4", new int[] {1, 0, 1, 1}},

            {"D1", new int[] {1, 1, 0, 0}},
            {"D2", new int[] {1, 1, 0, 1}},
            {"D3", new int[] {1, 1, 1, 0}},
            {"D4", new int[] {1, 1, 1, 1}}
        };

    }
    private void InitializeBoardSlotList()
    {
        boardSlotList = new Dictionary<string, Point>
        {
            {"A1", new Point(0, 0)},
            {"A2", new Point(0, 1)},
            {"A3", new Point(0, 2)},
            {"A4", new Point(0, 3)},

            {"B1", new Point(1, 0)},
            {"B2", new Point(1, 1)},
            {"B3", new Point(1, 2)},
            {"B4", new Point(1, 3)},

            {"C1", new Point(2, 0)},
            {"C2", new Point(2, 1)},
            {"C3", new Point(2, 2)},
            {"C4", new Point(2, 3)},

            {"D1", new Point(3, 0)},
            {"D2", new Point(3, 1)},
            {"D3", new Point(3, 2)},
            {"D4", new Point(3, 3)}
        };
    }

    private void InitializePieceNumberMap()
    {
        PieceNumberMap = new Dictionary<String, int>
        {
            {"A1", 0},
            {"A2", 1},
            {"A3", 2},
            {"A4", 3},
                   
            {"B1", 4},
            {"B2", 5},
            {"B3", 6},
            {"B4", 7},
                   
            {"C1", 8},
            {"C2", 9},
            {"C3", 10},
            {"C4", 11},
                   
            {"D1", 12},
            {"D2", 13},
            {"D3", 14},
            {"D4", 15}
        };
    }

    private void InitializeNumberPieceMap()
    {
        NumberPieceMap = new Dictionary<int, String>
        {
            {0, "A1"},
            {1, "A2"},
            {2, "A3"},
            {3, "A4"},

            {4, "B1"},
            {5, "B2"},
            {6, "B3"},
            {7, "B4"},

            {8, "C1"},
            {9, "C2"},
            {10, "C3"},
            {11, "C4"},

            {12, "D1"},
            {13, "D2"},
            {14, "D3"},
            {15, "D4"}
        };
    }

    public bool Move(string boardSlotId, string pieceId)
    {
        Point boardSlot = boardSlotList[boardSlotId];
        return Move(boardSlot.X, boardSlot.Y, pieceId);
    }

    public bool Move(int row, int col, string movedPiece)
    {
        if (PlayablePieces.Contains(movedPiece) && GameBoard[row, col] == "[]")
        {
            Debug.Log("Setting move in model - " + movedPiece);
            GameBoard[row, col] = movedPiece;

            PlayablePieces.Remove(movedPiece);

            GameOver = HasWon(row, col);
            CheckTieGame();

            if (GameOver)
            {
                Debug.Log(Environment.NewLine + "GAME OVER" + Environment.NewLine);
                GameWon?.Invoke();
            }
            return true;
        }
        else return false;
           
    }

    public void CheckTieGame()
    {
        if(PlayablePieces.Count==0 && !GameOver)
        {   
            isGameTied = true;
            GameOver = true;
        }
    }

    public bool HasWon(int currRow, int currCol)
    {
        bool hasWon = false;
      
        HashSet<int> attr1 = new HashSet<int>();
        HashSet<int> attr2 = new HashSet<int>();
        HashSet<int> attr3 = new HashSet<int>();
        HashSet<int> attr4 = new HashSet<int>();

        //string pieceKey = gameBoard[currRow, currCol];

        //check by row
        for (int x = 0; x < 4; x++)
        {
            addAttr(currRow, x);
        }

        if (compareBitSet(attr1) || compareBitSet(attr2) || compareBitSet(attr3) || compareBitSet(attr4))
        {
            hasWon = true;
        }
        else
        {
            clearAttr();
            //check by column
            for (int y = 0; y < 4; y++)
            {
                addAttr(y, currCol);
            }
            if (compareBitSet(attr1) || compareBitSet(attr2) || compareBitSet(attr3) || compareBitSet(attr4))
            {
                hasWon = true;
            }
            //check for diagonal \
            else if (currCol == currRow)
            {
                clearAttr();
                for (int c = 0; c < 4; c++)
                {
                    addAttr(c, c);
                }
                if (compareBitSet(attr1) || compareBitSet(attr2) || compareBitSet(attr3) || compareBitSet(attr4))
                {
                    hasWon = true;
                }
            }
            //Check for diagonal /
            else if ((currCol + currRow) == 3)
            {
                clearAttr();
                for (int c = 0; c < 4; c++)
                {
                    addAttr(c, (3 - c));
                }
                if (compareBitSet(attr1) || compareBitSet(attr2) || compareBitSet(attr3) || compareBitSet(attr4))
                {
                    hasWon = true;
                }
            }
        }

        bool addAttr(int y, int x)
        {
            attr1.Add(pieceList[GameBoard[y, x]][0]);
            attr2.Add(pieceList[GameBoard[y, x]][1]);
            attr3.Add(pieceList[GameBoard[y, x]][2]);
            attr4.Add(pieceList[GameBoard[y, x]][3]);
            return true;
        }  

        bool clearAttr()
        {
            attr1.Clear();
            attr2.Clear();
            attr3.Clear();
            attr4.Clear();
            return true;
        }
            
        return hasWon;
    }

    bool compareBitSet(HashSet<int> attrSet)
    {
        return (attrSet.Count == MAXATTRIBUTEVARIANCE);
    }

    public bool CheckForPlayablePiece(string piece)
    {
        return (PlayablePieces.Contains(piece));
    }

    public bool RemovePlayablePiece(string piece)
    {
        return (PlayablePieces.Remove(piece));
    }


    public void NewGame()
    {
        GameBoard = new string[4, 4];
        for(int x=0; x<4; x++)
        {
            for(int y=0; y<4; y++)
            {
                GameBoard[x, y] = "[]";
            }
        }

        PlayablePieces = new HashSet<string> { "A1", "A2", "A3", "A4", "B1", "B2", "B3", "B4", "C1", "C2", "C3", "C4", "D1", "D2", "D3", "D4" };
    }
}

