using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore_V2
{
    class TurnInfo
    {
        public string[,] BoardState { get; set; }
        public string PieceToPlace { get; set; }
        public HashSet<string> PlayablePieces { get; set; }
    }
}
