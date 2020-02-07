using System;

namespace GameCore_V2
{
    class Program
    {
        static void Main(string[] args)
        {
            QuartoGame quarto = new QuartoGame();
            quarto.NewGame();

            string command = "";

            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    Console.Write(quarto.GameBoard[x, y]);
                }
                Console.WriteLine("");
            }

            Console.WriteLine("Playable Pieces: ");
            Console.WriteLine(string.Join(", ", quarto.PlayablePieceList));
            Console.WriteLine("Commands: quit, move");

            while (quarto.GameOver!=true || command !="quit")
            {
                Console.Write("Command: ");
                command = Console.ReadLine();
                if(command=="move")
                {
                    string pieceName = "";
                    string boardSlot = "";
                    Console.Write("Piece to move: ");
                    pieceName = Console.ReadLine();
                    Console.Write("What board slot would you like to place the piece on? ");
                    boardSlot = Console.ReadLine();

                    quarto.Move(boardSlot, pieceName);


                    for (int x = 0; x < 4; x++)
                    {
                        for (int y = 0; y < 4; y++)
                        {
                            Console.Write(quarto.GameBoard[x, y]);
                        }
                        Console.WriteLine("");
                    }

                    Console.WriteLine("Playable Pieces: ");
                    Console.WriteLine(string.Join(", ", quarto.PlayablePieceList));
                    Console.WriteLine("Commands: quit, move");
                }
            }

        }
    }
}
