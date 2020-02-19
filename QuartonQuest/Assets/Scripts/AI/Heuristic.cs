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

        // Calculates how many wins each characteristic of the piece to be played can win on any given board.
        public static int calculateHeuristic(string[] gameBoard, string pieceToPlay)
        {
            bool isPlaying = true;
            var slot1 = 0;
            var slot2 = 0;
            var slot3 = 0;
            var slot4 = 0;
            var potentialSlot = 0;
            int finalResult = 0;
            byte pieceBinary;
            byte[] pieces = new byte[16];
            int[] nullPiece = new int[2];
            bool[] rowWon = new bool[10];
            for (int i = 0; i < 10; i++)
                rowWon[i] = false;
            // Array.Fill is not working in Unity as of 2/18/2020
            // Array.Fill(rowWon, false);

            pieceBinary = convertToBinaryRepresentation(pieceToPlay);
            for (int i = 0; i < MAXGAMEBOARD; i++)
            {
                pieces[i] = convertToBinaryRepresentation(gameBoard[i]);
            }

            if (pieceToPlay == null)
            {
                isPlaying = false;
            }

            //checks columns and rows for wins
            for (int k = 3; k >= 0; k--)
            {
                int rowCounter = 0;
                for (int j = 0; j < 4; j++)
                {

                    //checks rows for wins on all matches
                    slot1 = pieces[rowCounter] >> k;
                    slot2 = pieces[rowCounter + 1] >> k;
                    slot3 = pieces[rowCounter + 2] >> k;
                    slot4 = pieces[rowCounter + 3] >> k;
                    potentialSlot = pieceBinary >> k;

                    nullPiece = countNullPieces(pieces[rowCounter], pieces[rowCounter + 1], pieces[rowCounter + 2], pieces[rowCounter + 3]);

                    if (nullPiece[1] > 1) { }

                    else
                    {

                        if (nullPiece[0] == 1)
                            slot1 = 0;
                        else if (nullPiece[0] == 2)
                            slot2 = 0;
                        else if (nullPiece[0] == 3)
                            slot3 = 0;
                        else if (nullPiece[0] == 4)
                            slot4 = 0;

                        if (isPlaying && (slot1 % 2 == 1 && slot2 % 2 == 1 && slot3 % 2 == 1 && potentialSlot % 2 == 1)
                            || (slot1 % 2 == 1 && slot2 % 2 == 1 && slot4 % 2 == 1 && potentialSlot % 2 == 1)
                            || (slot1 % 2 == 1 && slot3 % 2 == 1 && slot4 % 2 == 1 && potentialSlot % 2 == 1)
                            || (slot2 % 2 == 1 && slot3 % 2 == 1 && slot4 % 2 == 1 && potentialSlot % 2 == 1))
                        {
                            if (rowWon[j] == false)
                            {
                                finalResult++;
                                rowWon[j] = true;
                            }
                        }

                        else if (!isPlaying && (slot1 % 2 == 1 && slot2 % 2 == 1 && slot3 % 2 == 1)
                            || (slot1 % 2 == 1 && slot2 % 2 == 1 && slot4 % 2 == 1)
                            || (slot1 % 2 == 1 && slot3 % 2 == 1 && slot4 % 2 == 1)
                            || (slot2 % 2 == 1 && slot3 % 2 == 1 && slot4 % 2 == 1))
                        {
                            if (rowWon[j] == false)
                            {
                                finalResult++;
                                rowWon[j] = true;
                            }
                        }

                        if (nullPiece[0] == 1)
                            slot1 = 1;
                        else if (nullPiece[0] == 2)
                            slot2 = 1;
                        else if (nullPiece[0] == 3)
                            slot3 = 1;
                        else if (nullPiece[0] == 4)
                            slot4 = 1;

                        if (isPlaying && (slot1 % 2 == 0 && slot2 % 2 == 0 && slot3 % 2 == 0 && potentialSlot % 2 == 0)
                            || (slot1 % 2 == 0 && slot2 % 2 == 0 && slot4 % 2 == 0 && potentialSlot % 2 == 0)
                            || (slot1 % 2 == 0 && slot3 % 2 == 0 && slot4 % 2 == 0 && potentialSlot % 2 == 0)
                            || (slot2 % 2 == 0 && slot3 % 2 == 0 && slot4 % 2 == 0 && potentialSlot % 2 == 0))
                        {
                            if (rowWon[j] == false)
                            {
                                finalResult++;
                                rowWon[j] = true;
                            }
                        }

                        else if (!isPlaying && (slot1 % 2 == 0 && slot2 % 2 == 0 && slot3 % 2 == 0)
                            || (slot1 % 2 == 0 && slot2 % 2 == 0 && slot4 % 2 == 0)
                            || (slot1 % 2 == 0 && slot3 % 2 == 0 && slot4 % 2 == 0)
                            || (slot2 % 2 == 0 && slot3 % 2 == 0 && slot4 % 2 == 0))
                        {
                            if (rowWon[j] == false)
                            {
                                finalResult++;
                                rowWon[j] = true;
                            }
                        }
                    }
                    rowCounter += 4;

                    //checks columns for wins on all matches
                    slot1 = pieces[j] >> k;
                    slot2 = pieces[j + 4] >> k;
                    slot3 = pieces[j + 8] >> k;
                    slot4 = pieces[j + 12] >> k;

                    nullPiece = countNullPieces(pieces[j], pieces[j + 4], pieces[j + 8], pieces[j + 12]);

                    if (nullPiece[1] > 1) { }

                    else
                    {

                        if (nullPiece[0] == 1)
                            slot1 = 0;
                        else if (nullPiece[0] == 2)
                            slot2 = 0;
                        else if (nullPiece[0] == 3)
                            slot3 = 0;
                        else if (nullPiece[0] == 4)
                            slot4 = 0;

                        if (isPlaying && (slot1 % 2 == 1 && slot2 % 2 == 1 && slot3 % 2 == 1 && potentialSlot % 2 == 1)
                           || (slot1 % 2 == 1 && slot2 % 2 == 1 && slot4 % 2 == 1 && potentialSlot % 2 == 1)
                           || (slot1 % 2 == 1 && slot3 % 2 == 1 && slot4 % 2 == 1 && potentialSlot % 2 == 1)
                           || (slot2 % 2 == 1 && slot3 % 2 == 1 && slot4 % 2 == 1 && potentialSlot % 2 == 1))
                        {
                            if (rowWon[j + 4] == false)
                            {
                                finalResult++;
                                rowWon[j + 4] = true;
                            }
                        }

                        else if (!isPlaying && (slot1 % 2 == 1 && slot2 % 2 == 1 && slot3 % 2 == 1)
                            || (slot1 % 2 == 1 && slot2 % 2 == 1 && slot4 % 2 == 1)
                            || (slot1 % 2 == 1 && slot3 % 2 == 1 && slot4 % 2 == 1)
                            || (slot2 % 2 == 1 && slot3 % 2 == 1 && slot4 % 2 == 1))
                        {
                            if (rowWon[j + 4] == false)
                            {
                                finalResult++;
                                rowWon[j + 4] = true;
                            }
                        }

                        if (nullPiece[0] == 1)
                            slot1 = 1;
                        else if (nullPiece[0] == 2)
                            slot2 = 1;
                        else if (nullPiece[0] == 3)
                            slot3 = 1;
                        else if (nullPiece[0] == 4)
                            slot4 = 1;

                        if (isPlaying && (slot1 % 2 == 0 && slot2 % 2 == 0 && slot3 % 2 == 0 && potentialSlot % 2 == 0)
                            || (slot1 % 2 == 0 && slot2 % 2 == 0 && slot4 % 2 == 0 && potentialSlot % 2 == 0)
                            || (slot1 % 2 == 0 && slot3 % 2 == 0 && slot4 % 2 == 0 && potentialSlot % 2 == 0)
                            || (slot2 % 2 == 0 && slot3 % 2 == 0 && slot4 % 2 == 0 && potentialSlot % 2 == 0))
                        {
                            if (rowWon[j + 4] == false)
                            {
                                finalResult++;
                                rowWon[j + 4] = true;
                            }
                        }

                        else if (!isPlaying && (slot1 % 2 == 0 && slot2 % 2 == 0 && slot3 % 2 == 0)
                            || (slot1 % 2 == 0 && slot2 % 2 == 0 && slot4 % 2 == 0)
                            || (slot1 % 2 == 0 && slot3 % 2 == 0 && slot4 % 2 == 0)
                            || (slot2 % 2 == 0 && slot3 % 2 == 0 && slot4 % 2 == 0))
                        {
                            if (rowWon[j + 4] == false)
                            {
                                finalResult++;
                                rowWon[j + 4] = true;
                            }
                        }
                    }
                }
            }

            //check diagonals for wins
            for (int k = 3; k >= 0; k--)
            {

                slot1 = pieces[0] >> k;
                slot2 = pieces[5] >> k;
                slot3 = pieces[10] >> k;
                slot4 = pieces[15] >> k;

                nullPiece = countNullPieces(pieces[0], pieces[5], pieces[10], pieces[15]);

                if (nullPiece[1] > 1) { }


                else
                {

                    if (nullPiece[0] == 1)
                        slot1 = 0;
                    else if (nullPiece[0] == 2)
                        slot2 = 0;
                    else if (nullPiece[0] == 3)
                        slot3 = 0;
                    else if (nullPiece[0] == 4)
                        slot4 = 0;
                    if (isPlaying && (slot1 % 2 == 1 && slot2 % 2 == 1 && slot3 % 2 == 1 && potentialSlot % 2 == 1)
                        || (slot1 % 2 == 1 && slot2 % 2 == 1 && slot4 % 2 == 1 && potentialSlot % 2 == 1)
                        || (slot1 % 2 == 1 && slot3 % 2 == 1 && slot4 % 2 == 1 && potentialSlot % 2 == 1)
                        || (slot2 % 2 == 1 && slot3 % 2 == 1 && slot4 % 2 == 1 && potentialSlot % 2 == 1))
                    {
                        if (rowWon[8] == false)
                        {
                            finalResult++;
                            rowWon[8] = true;
                        }
                    }

                    else if (!isPlaying && (slot1 % 2 == 1 && slot2 % 2 == 1 && slot3 % 2 == 1)
                        || (slot1 % 2 == 1 && slot2 % 2 == 1 && slot4 % 2 == 1)
                        || (slot1 % 2 == 1 && slot3 % 2 == 1 && slot4 % 2 == 1)
                        || (slot2 % 2 == 1 && slot3 % 2 == 1 && slot4 % 2 == 1))
                    {
                        if (rowWon[8] == false)
                        {
                            finalResult++;
                            rowWon[8] = true;
                        }
                    }

                    if (nullPiece[0] == 1)
                        slot1 = 1;
                    else if (nullPiece[0] == 2)
                        slot2 = 1;
                    else if (nullPiece[0] == 3)
                        slot3 = 1;
                    else if (nullPiece[0] == 4)
                        slot4 = 1;

                    if (isPlaying && (slot1 % 2 == 0 && slot2 % 2 == 0 && slot3 % 2 == 0 && potentialSlot % 2 == 0)
                        || (slot1 % 2 == 0 && slot2 % 2 == 0 && slot4 % 2 == 0 && potentialSlot % 2 == 0)
                        || (slot1 % 2 == 0 && slot3 % 2 == 0 && slot4 % 2 == 0 && potentialSlot % 2 == 0)
                        || (slot2 % 2 == 0 && slot3 % 2 == 0 && slot4 % 2 == 0 && potentialSlot % 2 == 0))
                    {
                        if (rowWon[8] == false)
                        {
                            finalResult++;
                            rowWon[8] = true;
                        }
                    }

                    else if (!isPlaying && (slot1 % 2 == 0 && slot2 % 2 == 0 && slot3 % 2 == 0)
                        || (slot1 % 2 == 0 && slot2 % 2 == 0 && slot4 % 2 == 0)
                        || (slot1 % 2 == 0 && slot3 % 2 == 0 && slot4 % 2 == 0)
                        || (slot2 % 2 == 0 && slot3 % 2 == 0 && slot4 % 2 == 0))
                    {
                        if (rowWon[8] == false)
                        {
                            finalResult++;
                            rowWon[8] = true;
                        }
                    }
                }

                slot1 = pieces[3] >> k;
                slot2 = pieces[6] >> k;
                slot3 = pieces[9] >> k;
                slot4 = pieces[12] >> k;

                nullPiece = countNullPieces(pieces[3], pieces[6], pieces[9], pieces[12]);

                if (nullPiece[1] > 1) { }


                else
                {

                    if (nullPiece[0] == 1)
                        slot1 = 0;
                    else if (nullPiece[0] == 2)
                        slot2 = 0;
                    else if (nullPiece[0] == 3)
                        slot3 = 0;
                    else if (nullPiece[0] == 4)
                        slot4 = 0;

                    if (isPlaying && (slot1 % 2 == 1 && slot2 % 2 == 1 && slot3 % 2 == 1 && potentialSlot % 2 == 1)
                           || (slot1 % 2 == 1 && slot2 % 2 == 1 && slot4 % 2 == 1 && potentialSlot % 2 == 1)
                           || (slot1 % 2 == 1 && slot3 % 2 == 1 && slot4 % 2 == 1 && potentialSlot % 2 == 1)
                           || (slot2 % 2 == 1 && slot3 % 2 == 1 && slot4 % 2 == 1 && potentialSlot % 2 == 1))
                    {
                        if (rowWon[9] == false)
                        {
                            finalResult++;
                            rowWon[9] = true;
                        }
                    }

                    else if (!isPlaying && (slot1 % 2 == 1 && slot2 % 2 == 1 && slot3 % 2 == 1)
                        || (slot1 % 2 == 1 && slot2 % 2 == 1 && slot4 % 2 == 1)
                        || (slot1 % 2 == 1 && slot3 % 2 == 1 && slot4 % 2 == 1)
                        || (slot2 % 2 == 1 && slot3 % 2 == 1 && slot4 % 2 == 1))
                    {
                        if (rowWon[9] == false)
                        {
                            finalResult++;
                            rowWon[9] = true;
                        }
                    }

                    if (nullPiece[0] == 1)
                        slot1 = 1;
                    else if (nullPiece[0] == 2)
                        slot2 = 1;
                    else if (nullPiece[0] == 3)
                        slot3 = 1;
                    else if (nullPiece[0] == 4)
                        slot4 = 1;

                    if (isPlaying && (slot1 % 2 == 0 && slot2 % 2 == 0 && slot3 % 2 == 0 && potentialSlot % 2 == 0)
                            || (slot1 % 2 == 0 && slot2 % 2 == 0 && slot4 % 2 == 0 && potentialSlot % 2 == 0)
                            || (slot1 % 2 == 0 && slot3 % 2 == 0 && slot4 % 2 == 0 && potentialSlot % 2 == 0)
                            || (slot2 % 2 == 0 && slot3 % 2 == 0 && slot4 % 2 == 0 && potentialSlot % 2 == 0))
                    {
                        if (rowWon[9] == false)
                        {
                            finalResult++;
                            rowWon[9] = true;
                        }
                    }

                    else if (!isPlaying && (slot1 % 2 == 0 && slot2 % 2 == 0 && slot3 % 2 == 0)
                        || (slot1 % 2 == 0 && slot2 % 2 == 0 && slot4 % 2 == 0)
                        || (slot1 % 2 == 0 && slot3 % 2 == 0 && slot4 % 2 == 0)
                        || (slot2 % 2 == 0 && slot3 % 2 == 0 && slot4 % 2 == 0))
                    {
                        if (rowWon[9] == false)
                        {
                            finalResult++;
                            rowWon[9] = true;
                        }
                    }
                }

            }

            return finalResult;
        }
    }
}
