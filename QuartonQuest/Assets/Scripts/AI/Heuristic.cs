using System;
using System.Collections.Generic;
using System.Text;

// Fix to be able to calculate heurisic for game generation and piece generation
namespace HeuristicCalculator
{
    public class Heuristic
    {
        public const int NULLPIECE = 55;
        public const int MAXGAMEBOARD = 16;

        public class winningValues
        {
            public int[] nullPiece = new int[2];
            public bool[] rowWon = new bool[10];
            public int finalResult = 0;
            public bool isPlaying = true;
        }
        public static byte convertToBinaryRepresentation(string piece)
        {
            byte pieceBinary = 0;
            if (piece == null)
            {
                pieceBinary = NULLPIECE;
            }
            else if (piece == "A1")
            {
                pieceBinary = 0b_0000;
            }
            else if (piece == "A2")
            {
                pieceBinary = 0b_0001;
            }
            else if (piece == "A3")
            {
                pieceBinary = 0b_0010;
            }
            else if (piece == "A4")
            {
                pieceBinary = 0b_0011;
            }
            else if (piece == "B1")
            {
                pieceBinary = 0b_0100;
            }
            else if (piece == "B2")
            {
                pieceBinary = 0b_0101;
            }
            else if (piece == "B3")
            {
                pieceBinary = 0b_0110;
            }
            else if (piece == "B4")
            {
                pieceBinary = 0b_0111;
            }
            else if (piece == "C1")
            {
                pieceBinary = 0b_1000;
            }
            else if (piece == "C2")
            {
                pieceBinary = 0b_1001;
            }
            else if (piece == "C3")
            {
                pieceBinary = 0b_1010;
            }
            else if (piece == "C4")
            {
                pieceBinary = 0b_1011;
            }
            else if (piece == "D1")
            {
                pieceBinary = 0b_1100;
            }
            else if (piece == "D2")
            {
                pieceBinary = 0b_1101;
            }
            else if (piece == "D3")
            {
                pieceBinary = 0b_1110;
            }
            else if (piece == "D4")
            {
                pieceBinary = 0b_1111;
            }

            return pieceBinary;
        }

        public static int[] countNullPieces(int firstPiece, int secPiece, int thirdPiece, int fourthPiece)
        {
            int nullPieceCount = 0;
            int nullPiece = 0;
            if (firstPiece == NULLPIECE)
            {
                nullPieceCount++;
                nullPiece = 1;
            }
            if (secPiece == NULLPIECE)
            {
                nullPieceCount++;
                nullPiece = 2;
            }
            if (thirdPiece == NULLPIECE)
            {
                nullPieceCount++;
                nullPiece = 3;
            }
            if (fourthPiece == NULLPIECE)
            {
                nullPieceCount++;
                nullPiece = 4;
            }

            int[] nullValues = { nullPiece, nullPieceCount };
            return nullValues;
        }

        public static bool isWin(string[] gameBoard, string pieceToPlay, int boardPosition)
        {
            byte pieceBinary;
            byte[] pieces = new byte[16];
            winningValues winningValues = new winningValues();

            for (int i = 0; i < 10; i++)
                winningValues.rowWon[i] = false;

            pieceBinary = convertToBinaryRepresentation(pieceToPlay);
            for (int i = 0; i < MAXGAMEBOARD; i++)
            {
                pieces[i] = convertToBinaryRepresentation(gameBoard[i]);
            }

            if (boardPosition == 0 || boardPosition == 1 || boardPosition == 2 || boardPosition == 3)
            {
                winningValues = checkWinRows(pieces[0], pieces[1], pieces[2], pieces[3], 0, winningValues, pieceBinary, 1);
                winningValues = checkWinRows(pieces[0], pieces[1], pieces[2], pieces[3], 0, winningValues, pieceBinary, 0);
            }
            if (boardPosition == 4 || boardPosition == 5 || boardPosition == 6 || boardPosition == 7)
            {
                winningValues = checkWinRows(pieces[4], pieces[5], pieces[6], pieces[7], 1, winningValues, pieceBinary, 1);
                winningValues = checkWinRows(pieces[4], pieces[5], pieces[6], pieces[7], 1, winningValues, pieceBinary, 0);
            }
            if (boardPosition == 8 || boardPosition == 9 || boardPosition == 10 || boardPosition == 11)
            {
                winningValues = checkWinRows(pieces[8], pieces[9], pieces[10], pieces[11], 2, winningValues, pieceBinary, 1);
                winningValues = checkWinRows(pieces[8], pieces[9], pieces[10], pieces[11], 2, winningValues, pieceBinary, 0);
            }
            if (boardPosition == 12 || boardPosition == 13 || boardPosition == 14 || boardPosition == 15)
            {
                winningValues = checkWinRows(pieces[12], pieces[13], pieces[14], pieces[15], 3, winningValues, pieceBinary, 1);
                winningValues = checkWinRows(pieces[12], pieces[13], pieces[14], pieces[15], 3, winningValues, pieceBinary, 0);
            }
            if (boardPosition == 0 || boardPosition == 4 || boardPosition == 8 || boardPosition == 12)
            {
                winningValues = checkWinRows(pieces[0], pieces[4], pieces[8], pieces[12], 4, winningValues, pieceBinary, 1);
                winningValues = checkWinRows(pieces[0], pieces[4], pieces[8], pieces[12], 4, winningValues, pieceBinary, 0);
            }
            if (boardPosition == 1 || boardPosition == 5 || boardPosition == 9 || boardPosition == 13)
            {
                winningValues = checkWinRows(pieces[1], pieces[5], pieces[9], pieces[12], 5, winningValues, pieceBinary, 1);
                winningValues = checkWinRows(pieces[1], pieces[5], pieces[9], pieces[12], 5, winningValues, pieceBinary, 0);
            }
            if (boardPosition == 2 || boardPosition == 6 || boardPosition == 10 || boardPosition == 14)
            {
                winningValues = checkWinRows(pieces[2], pieces[6], pieces[10], pieces[14], 6, winningValues, pieceBinary, 1);
                winningValues = checkWinRows(pieces[2], pieces[6], pieces[10], pieces[14], 6, winningValues, pieceBinary, 0);
            }
            if (boardPosition == 3 || boardPosition == 7 || boardPosition == 11 || boardPosition == 15)
            {
                winningValues = checkWinRows(pieces[3], pieces[7], pieces[11], pieces[15], 7, winningValues, pieceBinary, 1);
                winningValues = checkWinRows(pieces[3], pieces[7], pieces[11], pieces[15], 7, winningValues, pieceBinary, 0);
            }
            if (boardPosition == 0 || boardPosition == 5 || boardPosition == 10 || boardPosition == 15)
            {
                winningValues = checkWinRows(pieces[0], pieces[5], pieces[10], pieces[15], 8, winningValues, pieceBinary, 1);
                winningValues = checkWinRows(pieces[0], pieces[5], pieces[10], pieces[15], 8, winningValues, pieceBinary, 0);               
            }
            if (boardPosition == 3 || boardPosition == 6 || boardPosition == 9 || boardPosition == 12)
            {
                winningValues = checkWinRows(pieces[3], pieces[6], pieces[9], pieces[12], 9, winningValues, pieceBinary, 1);
                winningValues = checkWinRows(pieces[3], pieces[6], pieces[9], pieces[12], 9, winningValues, pieceBinary, 0);
            }

            if (winningValues.finalResult > 0)
                return true;
            else
                return false;
        }
        // Calculates how many wins each characteristic of the piece to be played can win on any given board.
        public static int calculateHeuristic(string[] gameBoard, string pieceToPlay)
        {
            byte pieceBinary;
            byte[] pieces = new byte[16];
            winningValues winningValues = new winningValues();

            for (int i = 0; i < 10; i++)
                winningValues.rowWon[i] = false;
            // Array.Fill is not working in Unity as of 2/18/2020
            // Array.Fill(rowWon, false);

            pieceBinary = convertToBinaryRepresentation(pieceToPlay);
            for (int i = 0; i < MAXGAMEBOARD; i++)
            {
                pieces[i] = convertToBinaryRepresentation(gameBoard[i]);
            }

            if (pieceToPlay == null)
            {
                winningValues.isPlaying = false;
            }

            //checks columns and rows for wins
            int rowCounter = 0;
            for (int colCounter = 0; colCounter < 4; colCounter++)
            {
                //checks rows for wins on all matches   
                winningValues = comparePiecesInRow(pieces[rowCounter], pieces[rowCounter+1], pieces[rowCounter+2], pieces[rowCounter+3], colCounter, winningValues, pieceBinary, 1);
                winningValues = comparePiecesInRow(pieces[rowCounter], pieces[rowCounter + 1], pieces[rowCounter + 2], pieces[rowCounter + 3], colCounter, winningValues, pieceBinary, 0);

                //checks columns for wins on all matches
                winningValues = comparePiecesInRow(pieces[colCounter], pieces[colCounter + 4], pieces[colCounter + 8], pieces[colCounter + 12], colCounter + 4, winningValues, pieceBinary, 1);
                winningValues = comparePiecesInRow(pieces[colCounter], pieces[colCounter + 4], pieces[colCounter + 8], pieces[colCounter + 12], colCounter + 4, winningValues, pieceBinary, 0);

                rowCounter += 4;
            }


            //check first diagonal for wins
            winningValues = comparePiecesInRow(pieces[0], pieces[5], pieces[10], pieces[15], 8, winningValues, pieceBinary, 1);
            winningValues = comparePiecesInRow(pieces[0], pieces[5], pieces[10], pieces[15], 8, winningValues, pieceBinary, 0);

            //check second diagonal for wins
            winningValues = comparePiecesInRow(pieces[3], pieces[6], pieces[9], pieces[12], 9, winningValues, pieceBinary, 1);
            winningValues = comparePiecesInRow(pieces[3], pieces[6], pieces[9], pieces[12], 9, winningValues, pieceBinary, 0);

            return winningValues.finalResult;
        }

        public static winningValues comparePiecesInRow(byte piece1, byte piece2, byte piece3, byte piece4, int rowNumber, winningValues winningValues, byte pieceBinary, int oddBit)
        {
            var slot1 = 0;
            var slot2 = 0;
            var slot3 = 0;
            var slot4 = 0;
            var potentialSlot = 0;
            int oppositeBit;

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

                    winningValues.nullPiece = countNullPieces(piece1, piece2, piece3, piece4);

                    if (winningValues.nullPiece[1] > 1) { }

                    else
                    {

                        if (winningValues.nullPiece[0] == 1)
                            slot1 = oppositeBit;
                        else if (winningValues.nullPiece[0] == 2)
                            slot2 = oppositeBit;
                        else if (winningValues.nullPiece[0] == 3)
                            slot3 = oppositeBit;
                        else if (winningValues.nullPiece[0] == 4)
                            slot4 = oppositeBit;

                        if (winningValues.isPlaying && (slot1 % 2 == oddBit && slot2 % 2 == oddBit && slot3 % 2 == oddBit && potentialSlot % 2 == oddBit)
                            || (slot1 % 2 == oddBit && slot2 % 2 == oddBit && slot4 % 2 == oddBit && potentialSlot % 2 == oddBit)
                            || (slot1 % 2 == oddBit && slot3 % 2 == oddBit && slot4 % 2 == oddBit && potentialSlot % 2 == oddBit)
                            || (slot2 % 2 == oddBit && slot3 % 2 == oddBit && slot4 % 2 == oddBit && potentialSlot % 2 == oddBit))
                        {
                            if (winningValues.rowWon[rowNumber] == false)
                            {
                                winningValues.finalResult++;
                                winningValues.rowWon[rowNumber] = true;
                            }
                        }

                        else if (!winningValues.isPlaying && (slot1 % 2 == oddBit && slot2 % 2 == oddBit && slot3 % 2 == oddBit)
                            || (slot1 % 2 == oddBit && slot2 % 2 == oddBit && slot4 % 2 == oddBit)
                            || (slot1 % 2 == oddBit && slot3 % 2 == oddBit && slot4 % 2 == oddBit)
                            || (slot2 % 2 == oddBit && slot3 % 2 == oddBit && slot4 % 2 == oddBit))
                        {
                            if (winningValues.rowWon[rowNumber] == false)
                            {
                                winningValues.finalResult++;
                                winningValues.rowWon[rowNumber] = true;
                            }
                        }

                    }
            }
            return winningValues;
        }

        public static winningValues checkWinRows(byte piece1, byte piece2, byte piece3, byte piece4, int rowNumber, winningValues winningValues, byte pieceBinary, int oddBit)
        {
            var slot1 = 0;
            var slot2 = 0;
            var slot3 = 0;
            var slot4 = 0;
            var potentialSlot = 0;
            int oppositeBit;

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

                winningValues.nullPiece = countNullPieces(piece1, piece2, piece3, piece4);

                if (winningValues.nullPiece[1] > 1) { }

                else
                {

                    if (winningValues.nullPiece[0] == 1)
                        slot1 = oppositeBit;
                    else if (winningValues.nullPiece[0] == 2)
                        slot2 = oppositeBit;
                    else if (winningValues.nullPiece[0] == 3)
                        slot3 = oppositeBit;
                    else if (winningValues.nullPiece[0] == 4)
                        slot4 = oppositeBit;

                    if (winningValues.isPlaying && (slot1 % 2 == oddBit && slot2 % 2 == oddBit && slot3 % 2 == oddBit && potentialSlot % 2 == oddBit)
                        || (slot1 % 2 == oddBit && slot2 % 2 == oddBit && slot4 % 2 == oddBit && potentialSlot % 2 == oddBit)
                        || (slot1 % 2 == oddBit && slot3 % 2 == oddBit && slot4 % 2 == oddBit && potentialSlot % 2 == oddBit)
                        || (slot2 % 2 == oddBit && slot3 % 2 == oddBit && slot4 % 2 == oddBit && potentialSlot % 2 == oddBit))
                    {
                        if (winningValues.rowWon[rowNumber] == false)
                        {
                            winningValues.finalResult++;
                            winningValues.rowWon[rowNumber] = true;
                        }
                    }

                }
            }
            return winningValues;
        }
    }
}
