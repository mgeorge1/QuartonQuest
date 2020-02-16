using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore_V2
{
    class GameCoreController
    {
        GameCoreModel model = new GameCoreModel();
        //model.NewGame();
        private enum Turn { PLAYER1, PLAYER2};
        Turn currentTurn;
        public string PieceToBePlaced="";

        public IGameCoreConsumer Player1 { get; set; }
        public IGameCoreConsumer Player2 { get; set; }
        public IGameCoreConsumer CurrentPlayer { get; set; }

        public void PlayGame ()
        {
            model.NewGame();
            PickFirstTurn();
            //Player who is picked selects a piece, beginning play loop
            // Keep requesting piece until a correct piece is selected
            while (SetChosenPiece(CurrentPlayer.PickFirstPiece()));

            while (model.GameOver != true )
            {
                //Choose a position to play the piece
                //Choose piece for opponent to play
                SwapTurn();
            }
        }
        
        public bool SetChosenPiece(string chosenPiece)
        {
            if(model.CheckForPlayablePiece(chosenPiece))
            {
                PieceToBePlaced = chosenPiece;
                return true;
            }
            else
            {
                Console.WriteLine("Error, Piece does not exist in the playable pieces list");
                return false;
            }
        }

        public bool PlacePiece(string boardSlotId, string pieceId)
        {
             return model.Move(boardSlotId, pieceId);
        }

        public void SwapTurn()
        {
            CurrentPlayer = CurrentPlayer == Player1 ? Player2 : Player2;
        }

        public void PickFirstTurn()
        {
            //Can be changed to manual turn picking later
            Random rand = new Random();
            CurrentPlayer = rand.Next(0, 1) == 1 ? Player1 : Player2;
        }

        public string GetChosenPiece()
        {
            return PieceToBePlaced;
        }

        public string[,] GetBoard()
        {
            return model.GameBoard;
        }
        
        //Only for console application version
        public void PrintBoard()
        {
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    Console.Write(model.GameBoard[x, y]);
                }
                Console.WriteLine("");
            }

        }

        public HashSet<string> GetPlayablePiecesList()
        {
            return model.PlayablePieces;
        }
        
    }
}
