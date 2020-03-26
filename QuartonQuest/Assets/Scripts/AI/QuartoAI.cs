using System;

namespace AI
{
    public class QuartoSearchTree
    {
        public const int MAXGAMEBOARD = 16;
        public const int NULLPIECE = 55;
        public int totalGamestates;
        public Node finalMoveDesicion;
        public class Node
        {
            public string[] gameBoard = new string[MAXGAMEBOARD];
            public Node[] children = new Node[MAXGAMEBOARD];
            public Node parent;
            public Piece[] pieces = new Piece[MAXGAMEBOARD];
            public int pieceToPlay;
            public int moveOnBoard;
        }

        public Node root;
        public QuartoSearchTree()
        {
            root = null;
        }

        public moveData generateTree(string[] newGameBoard, int piece, Piece[] currentPieces)
        {
            totalGamestates = 0;
            int piecesOnBoard;
            int maxDepth;

            Node newNode = new Node();
            newNode.gameBoard = newGameBoard;
            newNode.pieceToPlay = piece;
            root = newNode;

            Node currentNode = root;
            Node parentNode;
            parentNode = currentNode;
            currentNode.pieces = currentPieces;

            // Sets tree depth according to how many pieces are on the board
            piecesOnBoard = AIFunctions.countPiecesOnBoard(newGameBoard);
            maxDepth = AIFunctions.setTreeDepth(piecesOnBoard);

            generateChildrenGamestate(currentNode, parentNode, piece, maxDepth, 0);

            winningMove move = NegaMax.searchForBestPlay(currentNode, maxDepth, 0, -MAXGAMEBOARD, MAXGAMEBOARD, false);

            //Checks for win by opponent, given the piece chosen
            //If win it makes it equal to the next child and so on
            if (piecesOnBoard != 0 && move.winningNode.pieceToPlay != NULLPIECE)
                move.winningNode = AIFunctions.checkForOpponentWin(move.winningNode);

            Console.Write("Total moves generated: ");
            Console.WriteLine(totalGamestates);
            AIFunctions.printBoard(move.winningNode);
            Console.WriteLine();
            Console.Write("Heuristic of Final Node: ");
            Console.Write(move.heuristicValue);
            Console.WriteLine();

            // This is bad but it works.
            string pieceOnDeck = move.winningNode.pieceToPlay == NULLPIECE ?
                NULLPIECE.ToString() : move.winningNode.pieces[move.winningNode.pieceToPlay].piece;

            return new moveData
            {
                lastMoveOnBoard = move.winningNode.pieces[move.winningNode.moveOnBoard].piece,
                pieceToPlay = pieceOnDeck
            };
        }

        // Generates all the moves that could possibly be made for a given gamestate and piece.
        public void generateChildrenGamestate(Node currentNode, Node parentNode, int piece, int maxDepth, int depth)
        {
            int boardPosCount = 0;
            int childCount = 0;

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

                    AIFunctions.copyPieceMap(nextNode, currentNode, piece);
                    nextNode.pieceToPlay = NULLPIECE;

                    childCount++;
                    if (depth < maxDepth)
                    {
                        Node childNode = nextNode;
                        Node nextParentNode = childNode;
                        int newDepth = depth + 1;
                        generateChildrenPiece(childNode, nextParentNode, maxDepth, newDepth);
                    }
                }
                boardPosCount++;
            }
        }

        // Generates all the piece selections that could possibly be made for a given gamestate.
        public void generateChildrenPiece(Node currentNode, Node parentNode, int maxDepth, int depth)
        {
            int pieceMapCount = 0;
            int childCount = 0;

            while (pieceMapCount < MAXGAMEBOARD)
            {
                if (currentNode.pieces[pieceMapCount].getPlayablePiece())
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

                    AIFunctions.copyPieceMap(nextNode, currentNode, pieceMapCount);

                    childCount++;
                    if (depth < maxDepth)
                    {
                        Node childNode = nextNode;
                        Node nextParentNode = childNode;
                        int piece = nextNode.pieceToPlay;
                        int newDepth = depth + 1;
                        generateChildrenGamestate(childNode, nextParentNode, piece, maxDepth, newDepth);
                    }
                }
                pieceMapCount++;
            }
        }

        static void Main(string[] args)
        {

            QuartoSearchTree tree = new QuartoSearchTree();
            string[] board = { "C1", null, null, null,
                               "A4", null, null, "A3",
                               "D3", "B2", null, null,
                               "B1", "B3", "A2", "C2"
                             };
            Piece[] pieces = new Piece[MAXGAMEBOARD];
            pieces[0].setValues("A1",  true);
            pieces[1].setValues("A2",  false);
            pieces[2].setValues("A3",  false);
            pieces[3].setValues("A4", false);
            pieces[4].setValues("B1", false);
            pieces[5].setValues("B2",  false);
            pieces[6].setValues("B3",  false);
            pieces[7].setValues("B4",  true);
            pieces[8].setValues("C1",  false);
            pieces[9].setValues("C2", false);
            pieces[10].setValues("C3", true);
            pieces[11].setValues("C4", true);
            pieces[12].setValues("D1", true);
            pieces[13].setValues("D2", true);
            pieces[14].setValues("D3", false);
            pieces[15].setValues("D4", true);

            tree.generateTree(board, 0, pieces);
        }
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