using System.Collections;
using System.Collections.Generic;

public interface IOpponent
{
    Move NextMove { get; set; }
    bool IsMaster { get; }
    void SendFirstTurn(GameCoreController.GameTurnState turn);
    void SendFirstMove();
    void SendMove();
    void SendForfeit();
    void RequestRematch();
    void ReplayGame();
    IEnumerator WaitForTurn();
    IEnumerator WaitForPickFirstPiece();
    IEnumerator Disconnect();
}

public class Move
{
    public string Piece { get; set; }
    public string Tile { get; set; }
    public string OnDeckPiece { get; set; }
}