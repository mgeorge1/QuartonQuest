using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Collections;

namespace GameCore_V2
{
    public class QuartoGame
    {
        public bool GameOver { get; set; } = false;
        Dictionary<string, int[]> pieceList;
        Dictionary<string, Point> boardSlotList;
        const int MAXATTRIBUTEVARIANCE = 1;
        public string[,] GameBoard { get; set; } = new string[4, 4];

        public HashSet<string> PlayablePieceList = new HashSet<string>();


        public QuartoGame()
        {
            InitializePieceList();
            InitializeBoardSlotList();
        }

        private void InitializePieceList()
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

       /* public struct gamePiece
        {
            public string Name;
            public bool Tall;
            public bool Round;
            public bool Pitted;
            public bool Light;

            public gamePiece(string name, bool tall, bool round, bool pitted, bool light)
            {
                Name = name;
                Tall = tall;
                Round = round;
                Pitted = pitted;
                Light = light;
            }
        }*/



        public bool Move(string boardSlotId, string pieceId)
        {
            Point boardSlot = boardSlotList[boardSlotId];
            return Move(boardSlot.X, boardSlot.Y, pieceId);
        }

        public bool Move(int row, int col, string movedPiece)
        {
            if (PlayablePieceList.Contains(movedPiece) && GameBoard[row, col] == "[]")
            {
                GameBoard[row, col] = movedPiece;

                PlayablePieceList.Remove(movedPiece);

                GameOver = HasWon(row, col);
                if (GameOver)
                {
                    Console.WriteLine(Environment.NewLine + "GAME OVER" + Environment.NewLine);
                }
                return true;
            }
            else return false;
           
        }

        public bool HasWon(int currRow, int currCol)
        {
            bool hasWon = false;
            /*
            string tall = "";
            string round = "";
            string pitted = "";
            string light = "";
            */
            HashSet<int> attr1 = new HashSet<int>();
            HashSet<int> attr2 = new HashSet<int>();
            HashSet<int> attr3 = new HashSet<int>();
            HashSet<int> attr4 = new HashSet<int>();

            //string pieceKey = gameBoard[currRow, currCol];

            //check by row
            for (int x = 0; x < 4; x++)
            {
                attr1.Add(pieceList[GameBoard[currRow, x]][0]);
                attr2.Add(pieceList[GameBoard[currRow, x]][1]);
                attr3.Add(pieceList[GameBoard[currRow, x]][2]);
                attr4.Add(pieceList[GameBoard[currRow, x]][3]);
            }

            if (compareBitSet(attr1) || compareBitSet(attr2) || compareBitSet(attr3) || compareBitSet(attr4))
            {
                hasWon = true;
            }
            else
            {
                attr1.Clear();
                attr2.Clear();
                attr3.Clear();
                attr4.Clear();
                //check by column
                for (int y = 0; y < 4; y++)
                {
                    attr1.Add(pieceList[GameBoard[y,currCol]][0]);
                    attr2.Add(pieceList[GameBoard[y,currCol]][1]);
                    attr3.Add(pieceList[GameBoard[y,currCol]][2]);
                    attr4.Add(pieceList[GameBoard[y,currCol]][3]);
                }
                if (compareBitSet(attr1) || compareBitSet(attr2) || compareBitSet(attr3) || compareBitSet(attr4))
                {
                    hasWon = true;
                }
                //check for diagonal \
                else if (currCol == currRow)
                {
                    attr1.Clear();
                    attr2.Clear();
                    attr3.Clear();
                    attr4.Clear();
                    for (int c = 0; c < 4; c++)
                    {
                        attr1.Add(pieceList[GameBoard[c, c]][0]);
                        attr2.Add(pieceList[GameBoard[c, c]][1]);
                        attr3.Add(pieceList[GameBoard[c, c]][2]);
                        attr4.Add(pieceList[GameBoard[c, c]][3]);
                    }
                    if (compareBitSet(attr1) || compareBitSet(attr2) || compareBitSet(attr3) || compareBitSet(attr4))
                    {
                        hasWon = true;
                    }
                }
                //Check for diagonal /
                else if ((currCol + currRow) == 3)
                {
                    attr1.Clear();
                    attr2.Clear();
                    attr3.Clear();
                    attr4.Clear();
                    for (int c = 0; c < 4; c++)
                    {
                        attr1.Add(pieceList[GameBoard[c, (3 - c)]][0]);
                        attr2.Add(pieceList[GameBoard[c, (3 - c)]][1]);
                        attr3.Add(pieceList[GameBoard[c, (3 - c)]][2]);
                        attr4.Add(pieceList[GameBoard[c, (3 - c)]][3]);
                    }
                    if (compareBitSet(attr1) || compareBitSet(attr2) || compareBitSet(attr3) || compareBitSet(attr4))
                    {
                        hasWon = true;
                    }
                }
            }

            return hasWon;
        }

        bool compareBitSet(HashSet<int> attrSet)
        {
            return (attrSet.Count == MAXATTRIBUTEVARIANCE);
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

            PlayablePieceList = new HashSet<string> { "A1", "A2", "A3", "A4", "B1", "B2", "B3", "B4", "C1", "C2", "C3", "C4", "D1", "D2", "D3", "D4" };
        }
    }
}
