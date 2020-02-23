using System.Collections;
using System.Collections.Generic;

public interface IOpponent
{
    Move NextMove { get; set; }
    bool IsMaster { get; }
    bool HasFirstTurn { get; set; }
    IEnumerator WaitForPickFirstTurn(GameCoreController.GameTurnState turn);
    IEnumerable SendFirstMove();
    IEnumerable SendMove();
    IEnumerator WaitForTurn(GameCoreController instance, string lastTile, string onDeckPiece);
    IEnumerator WaitForPickFirstPiece(GameCoreController instance);
    IEnumerator GameOver(bool didWin);
}

public class Move
{
    public string Piece { get; set; }
    public string Tile { get; set; }
    public string OnDeckPiece { get; set; }
}