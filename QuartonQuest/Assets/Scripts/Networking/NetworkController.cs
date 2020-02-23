using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkController : MonoBehaviour, IOpponent
{
    public static NetworkController Instance { get; private set; }
    [SerializeField] private PhotonView photonView;
    private static string messageToSend;
    public static Move move = new Move();
    public Move NextMove {
        get
        {
            return move;
        }
        set
        {
            move = value;
        }
    }
    public bool IsMaster { 
        get
        {
            return !PhotonNetwork.IsMasterClient;
        }
    }

    private static bool messageReceived = false;

    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
            return;

        if (Input.GetKeyDown(KeyCode.M))
        {
            photonView.RPC("GetMessage", RpcTarget.All, messageToSend);
        }
    }

    [PunRPC]
    void GetMessage(string message)
    {
        Debug.Log("Here is some message: " + message);
    }

    [PunRPC]
    void SetCurrentTurn(GameCoreController.GameTurnState turn)
    {
        if (photonView.IsMine)
            return;

        Debug.Log("Received turn from master client");
        GameCoreController.Instance.CurrentTurn = turn;
    }

    [PunRPC]
    void SetOnDeckPiece(string onDeckPiece)
    {
        if (photonView.IsMine)
            return;

        Debug.Log("Receiving on-deck piece = " + onDeckPiece);
        NextMove.OnDeckPiece = onDeckPiece;
        messageReceived = true;
    }

    [PunRPC]
    void SendMove(string lastTile, string onDeckPiece)
    {
        if (photonView.IsMine)
            return;

        Debug.Log("Receiving move");
        NextMove.OnDeckPiece = onDeckPiece;
        NextMove.Tile = lastTile;
        messageReceived = true;
    }

    public IEnumerator WaitForTurn(string lastTile, string onDeckPiece)
    {
        Debug.Log("NetworkController WaitforTurn");
        while (messageReceived == false) yield return null;
        messageReceived = false;
    }

    public IEnumerator WaitForPickFirstPiece()
    {
        while (messageReceived == false) yield return null;
        Debug.Log("Continuing execution");
        messageReceived = false;
    }

    public IEnumerator GameOver(bool didWin)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator WaitForPickFirstTurn(GameCoreController.GameTurnState turn)
    {
        Debug.Log("Sending who goes first");
        photonView.RPC("SetCurrentTurn", RpcTarget.All, turn);
        return null;
    }

    public IEnumerable SendFirstMove()   
    {
        Debug.Log("Sending first move");
        photonView.RPC("SetOnDeckPiece", RpcTarget.All, GameCoreController.Instance.OnDeckPiece);
        return null;
    }

    public IEnumerable SendMove()
    {
        Debug.Log("Sending move");
        photonView.RPC("SendMove", RpcTarget.All, GameCoreController.Instance.LastTileClicked, GameCoreController.Instance.OnDeckPiece);
        return null;
    }
}
