using System;
using HeuristicCalculator;
using System.Collections.Generic;
namespace AI
{
    public static class AIFunctions
    {

        // Copies a node's piece map to a string array
        public static void copyPieceMap(QuartoSearchTree.Node recievingNode, QuartoSearchTree.Node nodeToCopy, int pieceToFind)
        {
            string pieceString;
            bool piecePlayable;
            for (int i = 0; i < QuartoSearchTree.MAXGAMEBOARD; i++)
            {
                pieceString = nodeToCopy.pieces[i].getPiece();
                piecePlayable = nodeToCopy.pieces[i].getPlayablePiece();

                recievingNode.pieces[i].setValues(pieceString, piecePlayable);
            }
            recievingNode.pieces[pieceToFind].setPlayable(false);
        }

        public static int countPiecesOnBoard(string[] gameBoard)
        {
            int pieceCount = 0;
            for (int i = 0; i < QuartoSearchTree.MAXGAMEBOARD; i++)
            {
                if (gameBoard[i] != null)
                    pieceCount += 1;
            }

            return pieceCount;
        }

        // Depths set to perform under 5 seconds in current state
        public static int setTreeDepth(int piecesOnBoard, int difficulty)
        {
            int newDepth = 1;
            if (difficulty == 3)
            {
                if (piecesOnBoard >= 2 && piecesOnBoard <= 4)
                    newDepth = 3;
                else if (piecesOnBoard == 5)
                    newDepth = 4;
                else if (piecesOnBoard >= 6)
                    newDepth = 5;
                else if (piecesOnBoard > 7)
                    newDepth = 14;
            }

            else if (difficulty == 2)
            {
                if (piecesOnBoard >= 2 && piecesOnBoard <= 4)
                    newDepth = 3;
                else if (piecesOnBoard == 5)
                    newDepth = 3;
                else if (piecesOnBoard >= 6)
                    newDepth = 4;
                else if (piecesOnBoard > 7)
                    newDepth = 6;
            }

            else if (difficulty == 1)
            {
                if (piecesOnBoard >= 2 && piecesOnBoard <= 4)
                    newDepth = 2;
                else if (piecesOnBoard == 5)
                    newDepth = 2;
                else if (piecesOnBoard >= 6)
                    newDepth = 3;
                else if (piecesOnBoard > 7)
                    newDepth = 4;
            }

            return newDepth;
        }

        // Checks if the game state that is being sent back to the oppenent will cause the opponent to win. If so find another solution,
        // if no solution or no win send that game state. 
        public static QuartoSearchTree.Node checkForOpponentWin(QuartoSearchTree.Node currentNode, string winBlockPiece)
        {
            QuartoSearchTree.Node newWinNode = null;
            bool hasWon = true;
            string[] gameBoard;
            string piece;
            int move;

            
            for (int i = 0; hasWon && currentNode.parent.children[i] != null; i++)
            {
                if (winBlockPiece != null)
                    gameBoard = currentNode.gameBoard;
                else
                    gameBoard = currentNode.parent.children[i].gameBoard;

                piece = currentNode.parent.children[i].pieces[currentNode.parent.children[i].pieceToPlay].getPiece();
                move = currentNode.parent.children[i].moveOnBoard;

                hasWon = isWinOnBoard(gameBoard, piece);

                if (!hasWon && piece != winBlockPiece)
                {
                    if (winBlockPiece == null)
                        newWinNode = currentNode.parent.children[i];
                    else
                    {
                        newWinNode = currentNode.parent.children[i];
                        newWinNode.gameBoard = gameBoard;
                    }

                }
            }

            if (hasWon && newWinNode == null)
            {
                newWinNode = currentNode;
            }

            return newWinNode;
        }

        public static bool isWinOnBoard(string[] gameBoard, string pieceString)
        {
            bool hasWon = true;
            for (int i = 0; i < QuartoSearchTree.MAXGAMEBOARD; i++)
            {
                if (gameBoard[i] == null)
                {
                    hasWon = Heuristic.isWin(gameBoard, pieceString, i);
                    if (hasWon)
                        return hasWon;
                }
            }

            return hasWon;
        }

        public static List<int> makePlayablePiecesOnly(Piece[] pieces)
        {
            List<int> piecesPlayable = new List<int>();
            for (int i = 0; i < QuartoSearchTree.MAXGAMEBOARD; i++)
            {
                if (pieces[i].playable == true)
                {
                    piecesPlayable.Add(i);
                }
            }

            return piecesPlayable;
        }

        public static List<int> makePlayablePositionList(string[] gameBoard)
        {
            List<int> positionsPlayable = new List<int>();
            for (int i = 0; i < QuartoSearchTree.MAXGAMEBOARD; i++)
            {
                if (gameBoard[i] == null)
                {
                    positionsPlayable.Add(i);
                }
            }
            return positionsPlayable;
        }

        public static int selectWinPostition(int winRowSlot, int rowNumber)
        {
            int positionToBlock = 0;

            if (winRowSlot == -1)
            {
                positionToBlock = -1;
            }
            else if (rowNumber == 0)
            {
                if (winRowSlot == 1)
                    positionToBlock = 0;
                else if (winRowSlot == 2)
                    positionToBlock = 1;
                else if (winRowSlot == 3)
                    positionToBlock = 2;
                else if (winRowSlot == 4)
                    positionToBlock = 3;
            }
            else if (rowNumber == 1)
            {
                if (winRowSlot == 1)
                    positionToBlock = 4;
                else if (winRowSlot == 2)
                    positionToBlock = 5;
                else if (winRowSlot == 3)
                    positionToBlock = 6;
                else if (winRowSlot == 4)
                    positionToBlock = 7;
            }
            else if (rowNumber == 2)
            {
                if (winRowSlot == 1)
                    positionToBlock = 8;
                else if (winRowSlot == 2)
                    positionToBlock = 9;
                else if (winRowSlot == 3)
                    positionToBlock = 10;
                else if (winRowSlot == 4)
                    positionToBlock = 11;
            }
            else if (rowNumber == 3)
            {
                if (winRowSlot == 1)
                    positionToBlock = 12;
                else if (winRowSlot == 2)
                    positionToBlock = 13;
                else if (winRowSlot == 3)
                    positionToBlock = 14;
                else if (winRowSlot == 4)
                    positionToBlock = 15;

            }
            else if (rowNumber == 4)
            {
                if (winRowSlot == 1)
                    positionToBlock = 0;
                else if (winRowSlot == 2)
                    positionToBlock = 4;
                else if (winRowSlot == 3)
                    positionToBlock = 8;
                else if (winRowSlot == 4)
                    positionToBlock = 12;
            }
            else if (rowNumber == 5)
            {
                if (winRowSlot == 1)
                    positionToBlock = 1;
                else if (winRowSlot == 2)
                    positionToBlock = 5;
                else if (winRowSlot == 3)
                    positionToBlock = 9;
                else if (winRowSlot == 4)
                    positionToBlock = 13;
            }
            else if (rowNumber == 6)
            {
                if (winRowSlot == 1)
                    positionToBlock = 2;
                else if (winRowSlot == 2)
                    positionToBlock = 6;
                else if (winRowSlot == 3)
                    positionToBlock = 10;
                else if (winRowSlot == 4)
                    positionToBlock = 14;
            }
            else if (rowNumber == 7)
            {
                if (winRowSlot == 1)
                    positionToBlock = 3;
                else if (winRowSlot == 2)
                    positionToBlock = 7;
                else if (winRowSlot == 3)
                    positionToBlock = 11;
                else if (winRowSlot == 4)
                    positionToBlock = 15;
            }
            else if (rowNumber == 8)
            {
                if (winRowSlot == 1)
                    positionToBlock = 0;
                else if (winRowSlot == 2)
                    positionToBlock = 5;
                else if (winRowSlot == 3)
                    positionToBlock = 10;
                else if (winRowSlot == 4)
                    positionToBlock = 15;
            }
            else if (rowNumber == 9)
            {
                if (winRowSlot == 1)
                    positionToBlock = 3;
                else if (winRowSlot == 2)
                    positionToBlock = 6;
                else if (winRowSlot == 3)
                    positionToBlock = 9;
                else if (winRowSlot == 4)
                    positionToBlock = 12;
            }

            return positionToBlock;
        }

        public static int findWinningPositionToBlock(string[] gameBoard, string pieceToPlay)
        {
            byte pieceBinary;
            byte[] pieces = new byte[QuartoSearchTree.MAXGAMEBOARD];
            int positionToBlock = -1;
            int posInRowToBlock;
            pieceBinary = Heuristic.convertToBinaryRepresentation(pieceToPlay);
            for (int i = 0; i < QuartoSearchTree.MAXGAMEBOARD; i++)
            {
                pieces[i] = Heuristic.convertToBinaryRepresentation(gameBoard[i]);
            }

            //checks columns and rows for wins
            int rowCounter = 0;
            for (int colCounter = 0; colCounter < 4; colCounter++)
            {
                //checks rows for wins on all matches   
                posInRowToBlock = findWinningPositionInRow(pieces[rowCounter], pieces[rowCounter + 1], pieces[rowCounter + 2], pieces[rowCounter + 3], colCounter, pieceBinary, 1);
                positionToBlock = selectWinPostition(posInRowToBlock, colCounter);
                if (positionToBlock > -1)
                    return positionToBlock;
                posInRowToBlock = findWinningPositionInRow(pieces[rowCounter], pieces[rowCounter + 1], pieces[rowCounter + 2], pieces[rowCounter + 3], colCounter, pieceBinary, 0);
                positionToBlock = selectWinPostition(posInRowToBlock, colCounter);
                if (positionToBlock > -1)
                    return positionToBlock;

                //checks columns for wins on all matches
                posInRowToBlock = findWinningPositionInRow(pieces[colCounter], pieces[colCounter + 4], pieces[colCounter + 8], pieces[colCounter + 12], colCounter + 4, pieceBinary, 1);
                positionToBlock = selectWinPostition(posInRowToBlock, colCounter + 4);
                if (positionToBlock > -1)
                    return positionToBlock;
                posInRowToBlock = findWinningPositionInRow(pieces[colCounter], pieces[colCounter + 4], pieces[colCounter + 8], pieces[colCounter + 12], colCounter + 4, pieceBinary, 0);
                positionToBlock = selectWinPostition(posInRowToBlock, colCounter + 4);
                if (positionToBlock > -1)
                    return positionToBlock;

                rowCounter += 4;
            }


            //check first diagonal for wins
            posInRowToBlock = findWinningPositionInRow(pieces[0], pieces[5], pieces[10], pieces[15], 8, pieceBinary, 1);
            positionToBlock = selectWinPostition(posInRowToBlock, 8);
            if (positionToBlock > -1)
                return positionToBlock;
            posInRowToBlock = findWinningPositionInRow(pieces[0], pieces[5], pieces[10], pieces[15], 8, pieceBinary, 0);
            positionToBlock = selectWinPostition(posInRowToBlock, 8);
            if (positionToBlock > -1)
                return positionToBlock;

            //check second diagonal for wins
            posInRowToBlock = findWinningPositionInRow(pieces[3], pieces[6], pieces[9], pieces[12], 9, pieceBinary, 1);
            positionToBlock = selectWinPostition(posInRowToBlock, 9);
            if (positionToBlock > -1)
                return positionToBlock;
            posInRowToBlock = findWinningPositionInRow(pieces[3], pieces[6], pieces[9], pieces[12], 9, pieceBinary, 0);
            positionToBlock = selectWinPostition(posInRowToBlock, 9);
            if (positionToBlock > -1)
                return positionToBlock;

            return positionToBlock;
        }

        public static int findWinningPositionInRow(byte piece1, byte piece2, byte piece3, byte piece4, int rowNumber, byte pieceBinary, int oddBit)
        {
            var slot1 = 0;
            var slot2 = 0;
            var slot3 = 0;
            var slot4 = 0;
            var potentialSlot = 0;
            int oppositeBit;
            int nullPieceCount = 0;
            int nullPiece = 0;
            int[] nullValues = { nullPiece, nullPieceCount };
            int position = -1;

            nullValues = Heuristic.countNullPieces(piece1, piece2, piece3, piece4);

            if (nullValues[1] > 1 || nullValues[1] == 0) { }

            else
            {
                if (oddBit == 1)
                    oppositeBit = 0;
                else
                    oppositeBit = 1;

                for (int k = 3; k >= 0; k--)
                {
                    //checks rows for wins on all matches
                    slot1 = piece1 >> k;
                    slot2 = piece2 >> k;
                    slot3 = piece3 >> k;
                    slot4 = piece4 >> k;
                    potentialSlot = pieceBinary >> k;

                    if (nullValues[0] == 1)
                        slot1 = oppositeBit;
                    else if (nullValues[0] == 2)
                        slot2 = oppositeBit;
                    else if (nullValues[0] == 3)
                        slot3 = oppositeBit;
                    else if (nullValues[0] == 4)
                        slot4 = oppositeBit;

                    if (slot1 % 2 == oddBit && slot2 % 2 == oddBit && slot3 % 2 == oddBit)
                    {
                        position = 4;
                        return position;
                    }
                    else if (slot1 % 2 == oddBit && slot2 % 2 == oddBit && slot4 % 2 == oddBit)
                    {
                        position = 3;
                        return position;
                    }
                    else if (slot1 % 2 == oddBit && slot3 % 2 == oddBit && slot4 % 2 == oddBit)
                    {
                        position = 2;
                        return position;
                    }
                    else if (slot2 % 2 == oddBit && slot3 % 2 == oddBit && slot4 % 2 == oddBit)
                    {
                        position = 1;
                        return position;
                    }
                }
            }
            return position;
        }
    }
}