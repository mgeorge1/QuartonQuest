using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using AI;
using HeuristicCalculator;

// Hash needs to be stores with the position the piece was placed!
namespace HashFunction
{
    public class ZobristHash
    {
        public const int MAXINTVALUE = 2147483647;
        int[,] randomNumbers = new int[QuartoSearchTree.MAXGAMEBOARD + 1, QuartoSearchTree.MAXGAMEBOARD + 1];
        String filePath = @"randomHashSeeds.txt";
        public bool init_zobristHash()
        {
            Encoding unicode = Encoding.Unicode;
            string numToPrint;

            // Check if file seed file is there.
            // If nothing in the file then produce random values.
            // If it is, then just read random values from it.
            // If not create the file and produce random hashes.
            //fullPath = Path.GetFullPath(@"mydir");


            if (File.Exists(filePath))
            {
                if (!readRandomHashFile())
                    return false;
            }

            FileStream randomSeeds;
            randomSeeds = File.Create(filePath);
            var rnd = new System.Random();
            for (int i = 0; i < QuartoSearchTree.MAXGAMEBOARD + 1; i++)
            {
                for (int j = 0; j < QuartoSearchTree.MAXGAMEBOARD + 1; j++)
                {
                    randomNumbers[i, j] = rnd.Next(0, MAXINTVALUE);

                    numToPrint = randomNumbers[i, j].ToString();
                    randomSeeds.Write(unicode.GetBytes(numToPrint), 0, unicode.GetByteCount(numToPrint));
                    byte[] newline = Encoding.ASCII.GetBytes(Environment.NewLine);
                    randomSeeds.Write(newline, 0, newline.Length);
                }
            }

            randomSeeds.Close();
            return true;
        }

        public bool readRandomHashFile()
        {
            FileStream randomSeeds;
            randomSeeds = new FileStream(filePath, FileMode.Open);
            if (new FileInfo(filePath).Length != 0)
            {
                for (int i = 0; i < QuartoSearchTree.MAXGAMEBOARD + 1; i++)
                {
                    for (int j = 0; j < QuartoSearchTree.MAXGAMEBOARD + 1; j++)
                    {
                        byte[] buffer = null; // wont read!!!!!!!!
                        randomNumbers[i, j] = randomSeeds.Read(buffer, 0, Int32.MaxValue);
                    }
                }
                randomSeeds.Close();
                return true;
            }

            else
            {
                randomSeeds.Close();
                return false;
            }
        }
        //Creates the hash for a gameBoard and a pieceToPlay using the XOR operation on random 32 bit numbers.
        public int zobristHash(string[] gameBoard, int pieceToPlay)
        {
            int hash = 0;
            byte[] tempPiece = new byte[1];
            int piece;

            for (int i = 0; i < QuartoSearchTree.MAXGAMEBOARD + 1; i++)
            {
                if (gameBoard[i] != null)
                {
                    if (i == QuartoSearchTree.MAXGAMEBOARD)
                        piece = pieceToPlay;

                    else
                    {
                        tempPiece[0] = Heuristic.convertToBinaryRepresentation(gameBoard[i]);
                        piece = BitConverter.ToInt32(tempPiece, 0);
                    }

                    hash ^= randomNumbers[i, piece];
                }
            }
            return hash;
        }

        public void HashToTranspositionTable(int hash)
        {
            //If no transposition table file, create one.
            //If there is, input new hash.
        }
        public int searchTranspositionTable(int hash)
        {
            // Searches transposition table using provided hash
            // If it is not there return -1
            // if it is return the position to play
            return 0;
        }
    }
}
