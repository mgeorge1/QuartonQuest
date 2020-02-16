using System;

namespace GameCore_V2
{
    class Program
    {
        static void Main(string[] args)
        {
            GameCoreModel quarto = new GameCoreModel();
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
            Console.WriteLine(string.Join(", ", quarto.PlayablePieces));
            Console.WriteLine("Commands: quit, move");

            while (quarto.GameOver!=true || command !="quit")
            {
                Console.Write("Command: ");
                command = Console.ReadLine();
                if(command=="move")
                {
                    Console.WriteLine("Playable Pieces: ");
                    Console.WriteLine(string.Join(", ", quarto.PlayablePieces));
                    Console.WriteLine("Commands: quit, move");
                }
            }

        }
    }
}
