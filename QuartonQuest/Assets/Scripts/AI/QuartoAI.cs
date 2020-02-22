using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections;
using HeuristicCalculator;

namespace AI
{
    public class QuartoSearchTree
    {
        public const int MAXGAMEBOARD = 16;
        public const int NULLPIECE = 55;
        public int totalGamestates;
        public Node finalMoveDesicion;
        int winnerHeuristicValue = -1;
        public class Node
        {
            public string[] gameBoard = new string[MAXGAMEBOARD];
            public Node[] children = new Node[MAXGAMEBOARD];
            public Node parent;
            public Node sibling;
            public Piece[] pieces = new Piece[MAXGAMEBOARD];
            public int pieceToPlay;
            public int moveOnBoard;
        }

       

        public struct winningMove
        {
            public Node winningNode;
            public int heuristicValue;
        }

        public Node root;
        public QuartoSearchTree()
        {
            root = null;
        }

        public moveData generateTree(int maxDepth, string[] newGameBoard, int piece, Piece[] currentPieces)
        {
            totalGamestates = 0;
            Node newNode = new Node();
            newNode.gameBoard = newGameBoard;
            newNode.pieceToPlay = piece;
            root = newNode;

            Node currentNode = root;
            Node parentNode;
            Node sibling = null;
            parentNode = currentNode;
            currentNode.sibling = null;
            currentNode.pieces = currentPieces;

            generateChildrenGamestate(currentNode, parentNode, piece, sibling, maxDepth, 0);

            winningMove move = searchForBestPlay(currentNode, 0, maxDepth, 0);
            Console.Write("Total moves generated: ");
            Console.WriteLine(totalGamestates);
            printBoard(move.winningNode);

            // This is bad but it works.
            string pieceOnDeck = move.winningNode.pieceToPlay == NULLPIECE ? 
                NULLPIECE.ToString() : move.winningNode.pieces[move.winningNode.pieceToPlay].piece;
            return new moveData { 
                lastMoveOnBoard = move.winningNode.pieces[move.winningNode.moveOnBoard].piece, 
                pieceToPlay = pieceOnDeck
            };
        }

        public void generateChildrenGamestate(Node currentNode, Node parentNode, int piece, Node previousSibling, int maxDepth, int depth)
        {
            int boardPosCount = 0;
            int childCount = 0;
            int siblingCount = 0;
   

            while (boardPosCount < MAXGAMEBOARD)
            {
                if (currentNode.gameBoard[boardPosCount] == null)
                {
                    totalGamestates++;
                    Node nextNode = new Node();
                    for (int counter = 0; counter < MAXGAMEBOARD; counter++)
                    {
                        string value = currentNode.gameBoard[counter];
                        nextNode.gameBoard[counter] = value;
                    }
                    nextNode.gameBoard[boardPosCount] = currentNode.pieces[piece].getPiece();

                    nextNode.moveOnBoard = boardPosCount;

                    nextNode.parent = parentNode;
                    parentNode.children[childCount] = nextNode;

                    copyPieceMap(nextNode, currentNode, piece);
                    nextNode.pieceToPlay = NULLPIECE;

                    if (siblingCount > 0)
                    {
                        previousSibling.sibling = nextNode;
                    }
                    siblingCount++;

                    previousSibling = nextNode;

                    childCount++;

                    //printBoard(nextNode);
                    if (depth < maxDepth)
                    {
                        Node childNode = nextNode;
                        Node nextParentNode = childNode;
                        Node newSibling = null;
                        int newDepth = depth + 1;
                        generateChildrenPiece(childNode, nextParentNode, newSibling, maxDepth, newDepth);
                    }
                }
                boardPosCount++;
            }
        }

        public void generateChildrenPiece(Node currentNode, Node parentNode, Node previousSibling, int maxDepth, int depth)
        {
            int pieceMapCount = 0;
            int childCount = 0;
            int siblingCount = 0;

            while (pieceMapCount < MAXGAMEBOARD)
            {
                if(currentNode.pieces[pieceMapCount].getPlayablePiece())
                {
                    totalGamestates++;
                    Node nextNode = new Node();

                    for (int counter = 0; counter < MAXGAMEBOARD; counter++)
                    {
                        string value = currentNode.gameBoard[counter];
                        nextNode.gameBoard[counter] = value;
                    }

                    nextNode.pieceToPlay = pieceMapCount;
                    int newMove = currentNode.moveOnBoard;

                    nextNode.moveOnBoard = newMove;
                    nextNode.parent = parentNode;

                    parentNode.children[childCount] = nextNode;

                    copyPieceMap(nextNode, currentNode, pieceMapCount);

                    if (siblingCount > 0)
                    {
                        previousSibling.sibling = nextNode;
                    }
                    siblingCount++;

                    previousSibling = nextNode;

                    
                    childCount++;
                    //printBoard(nextNode);
                    if (depth < maxDepth)
                    {
                        Node childNode = nextNode;
                        Node nextParentNode = childNode;
                        Node newSibling = null;
                        int piece = nextNode.pieceToPlay;
                        int newDepth = depth + 1;
                        generateChildrenGamestate(childNode, nextParentNode, piece, newSibling, maxDepth, newDepth); //need to fix playable piece problem on nodes
                    }
                }
                pieceMapCount++;
            }
        }
        public void copyPieceMap(Node recievingNode, Node nodeToCopy, int pieceToFind)
        {
            string pieceString;
            bool piecePlayable;
            for(int i = 0; i < MAXGAMEBOARD; i++)
            {
                pieceString = nodeToCopy.pieces[i].getPiece();
                piecePlayable = nodeToCopy.pieces[i].getPlayablePiece();

                recievingNode.pieces[i].setValues(pieceString, piecePlayable);
            }
            recievingNode.pieces[pieceToFind].setPlayable(false);
        }
        
        public void printBoard(Node current)
        {
            for (int j = 0; j < MAXGAMEBOARD; j++)
            {
                if (current.gameBoard[j] != null)
                {
                    Console.Write(current.gameBoard[j]);
                    Console.Write(" ");
                }
                else
                    Console.Write("X ");

                if((j + 1) % 4 == 0)
                {
                    Console.WriteLine();
                }
            }
            Console.WriteLine();
            Console.Write("Piece to Play: ");
            Console.WriteLine(current.pieceToPlay);
            Console.WriteLine();
        }

        public winningMove searchForBestPlay(Node currentNode, int depthCounter, int depth, int negaMax)
        {
            int j = 0;
            string pieceString;
            int boardPosition = 0;
            int negaMaxCompare;
            winningMove winChoice = new winningMove();
            winningMove winChoice2 = new winningMove();
            winChoice.heuristicValue = -1;
            winChoice.winningNode = new Node();

            // If only root in tree
            if (currentNode.parent == null && currentNode.children[0] == null)
            {
                Console.WriteLine("Search Failed: Tree only contains root");
            }

            // Detects for win
            if (currentNode.parent == null)
            {
                for (int i = 0; i < MAXGAMEBOARD && currentNode.children[i] != null; i++)
                {
                    if (currentNode.pieceToPlay == NULLPIECE)
                        pieceString = null;
                    else
                        pieceString = currentNode.pieces[currentNode.pieceToPlay].piece;

                    while (currentNode.gameBoard[boardPosition] != null)
                    {
                        boardPosition++;
                    }
                    bool win = Heuristic.isWin(currentNode.gameBoard, pieceString, boardPosition);

                    if (win)
                    {
                        winChoice.winningNode = currentNode.children[i];
                        winChoice.heuristicValue = 0;
                        return winChoice;
                    }
                    boardPosition++;
                }
            }

            // If it is the bottom of the tree
            if (currentNode.children[0] == null)
            {
                for (int i = 0; i < MAXGAMEBOARD && currentNode.parent.children[i] != null; i++)
                {
                    if (currentNode.parent.children[i].pieceToPlay == NULLPIECE)
                        pieceString = null;
                    else
                        pieceString = currentNode.parent.children[i].pieces[currentNode.parent.children[i].pieceToPlay].piece;

                    negaMaxCompare = Heuristic.calculateHeuristic(currentNode.parent.children[i].gameBoard, pieceString);

                    if(i == 0)
                    {
                        negaMax = Heuristic.calculateHeuristic(currentNode.parent.children[i].gameBoard, pieceString);
                        winChoice.winningNode = currentNode.parent.children[i];
                        winChoice.heuristicValue = negaMax;
                    }
                    else if(-negaMaxCompare > -negaMax)
                    {
                        negaMax = -negaMaxCompare;
                        winChoice.winningNode = currentNode.parent.children[i];
                        winChoice.heuristicValue = negaMax;
                    }
                }
            }

            // Iterates through all children nodes
            else
            {
                while (j < MAXGAMEBOARD && currentNode.children[j] != null)
                {
                    if (j == 0)
                    {
                        winChoice = searchForBestPlay(currentNode.children[j], depthCounter++, depth, negaMax);

                        if (currentNode.parent == null || currentNode.parent.parent == null){}
                        else if (currentNode.parent.parent.parent == null)
                        {
                            winChoice.winningNode = currentNode;
                        }
                    }
                    else
                    {
                        winChoice2 = searchForBestPlay(currentNode.children[j], depthCounter++, depth, negaMax);
                        if (-winChoice2.heuristicValue > -winChoice.heuristicValue)
                            {
                                winChoice.heuristicValue = -winChoice2.heuristicValue;

                                if (currentNode.parent == null){}
                                else if (currentNode.parent.parent == null) 
                                {
                                    winChoice.heuristicValue = winChoice2.heuristicValue;
                                }
                                else if (currentNode.parent.parent.parent == null) 
                                {
                                    winChoice.winningNode = currentNode;
                                    winChoice.heuristicValue = winChoice2.heuristicValue;
                                }
                            }
                    }
                    j++;
                }
            }
            //printBoard(winChoice.winningNode);
            Console.WriteLine(winChoice.heuristicValue);
            return winChoice;
        }
        //static void Main(string[] args)
        //{
           
        //    QuartoSearchTree tree = new QuartoSearchTree();
        //    string[] board = { "A1", "B1", null, null, 
        //                       "B3", null, "C2", "B4", 
        //                       "C1", null, "C3", null,
        //                       "A4", null, "D3", "D2" };
        //    Piece[] pieces = new Piece[MAXGAMEBOARD];
        //    pieces[0].setValues("A1", false);
        //    pieces[1].setValues("A2", true);
        //    pieces[2].setValues("A3", true);
        //    pieces[3].setValues("A4", false);
        //    pieces[4].setValues("B1", false);
        //    pieces[5].setValues("B2", true);
        //    pieces[6].setValues("B3", false);
        //    pieces[7].setValues("B4", false);
        //    pieces[8].setValues("C1", false);
        //    pieces[9].setValues("C2", false);
        //    pieces[10].setValues("C3", false);
        //    pieces[11].setValues("C4", true);
        //    pieces[12].setValues("D1", true);
        //    pieces[13].setValues("D2", false);
        //    pieces[14].setValues("D3", false);
        //    pieces[15].setValues("D4", true);

        //    int value = Heuristic.calculateHeuristic(board, "C4");
        //    bool val = Heuristic.isWin(board, "C4", 2);
        //    tree.generateTree(2, board, 11, pieces);
        //    Console.WriteLine(value);
        //    Console.WriteLine(val);
        //}
    }
    public struct Piece
    {
        public string piece;
        public bool playable;

        public void setValues(string piece, bool playable)
        {
            this.piece = piece;
            this.playable = playable;
        }

        public void setPlayable(bool playable)
        {
            this.playable = playable;
        }
        public bool getPlayablePiece()
        {
            return this.playable;
        }

        public string getPiece()
        {
            return this.piece;
        }
    }

    public struct moveData
    {
        public string lastMoveOnBoard;
        public string pieceToPlay;
    }
}