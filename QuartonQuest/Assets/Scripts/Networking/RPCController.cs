﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RPCController : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;
    private static bool messageReceived = false;
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

    public IEnumerator WaitForTurn()
    {
        Debug.Log("Waiting for the opponent's turn to finish...");
        while (messageReceived == false) yield return null;
        messageReceived = false;
    }

    public IEnumerator WaitForPickFirstPiece()
    {
        Debug.Log("Waiting for the opponent to pick the first piece...");
        while (messageReceived == false) yield return null;
        messageReceived = false;
    }

    public IEnumerator GameOver(bool didWin)
    {
        throw new System.NotImplementedException();
    }

    public void SendFirstTurn(GameCoreController.GameTurnState turn)
    {
        Debug.Log("Sending who goes first");
        photonView.RPC("SetCurrentTurn", RpcTarget.All, turn);
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