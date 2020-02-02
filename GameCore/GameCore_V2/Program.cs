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
                    int pieceX;
                    int pieceY;
                    Console.Write("Piece to move: ");
                    pieceName = Console.ReadLine();
                    Console.Write("X positon of the space to move to? (Counting starts at 0.): ");
                    pieceX = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Y positon of the space to move to? (Counting starts at 0.): ");
                    pieceY = Convert.ToInt32(Console.ReadLine());

                    quarto.Move(pieceX, pieceY, pieceName);


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
