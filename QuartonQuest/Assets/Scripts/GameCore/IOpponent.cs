using System.Collections;
using System.Collections.Generic;

public interface IOpponent
{
    IEnumerator WaitForTurn(GameCoreController instance, string lastTile, string onDeckPiece);
    IEnumerator PickFirstPiece(GameCoreController instance);
    IEnumerator GameOver(bool didWin);
    Move NextMove {get; set;}
}

public class Move
{
    public string Piece { get; set; }
    public string Tile { get; set; }
    public string OnDeckPiece { get; set; }
}