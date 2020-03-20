using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
using Photon.Pun;
using Photon.Realtime;

public class NetworkController : MonoBehaviorPunSingleton<NetworkController>, IOpponent
{
    private RPCController rpcController;
    private static bool messageReceived = false;
    public delegate void DisconnectedCallback();
    public Move NextMove
    {
        get
        {
            return rpcController.NextMove;
        }
        set
        {
            rpcController.NextMove = value;
        }
    }
    public bool IsMaster
    {
        get
        {
            return !PhotonNetwork.IsMasterClient;
        }
    }

    public void InstantiateRPCController()
    {
        GameObject photonObject = PhotonNetwork.Instantiate("RPCController", Vector3.zero, Quaternion.identity);
        rpcController = photonObject.GetComponent<RPCController>();
    }

    public IEnumerator WaitForTurn()
    {
        yield return rpcController.WaitForTurn();
    }

    public IEnumerator WaitForPickFirstPiece()
    {
        yield return rpcController.WaitForPickFirstPiece();
    }

    public void SendFirstTurn(GameCoreController.GameTurnState turn)
    {
        rpcController.SendFirstTurn(turn);
    }

    public void SendFirstMove()
    {
        rpcController.SendFirstMove();
    }

    public void SendMove()
    {
        rpcController.SendMove();
    }

    public IEnumerator Disconnect(DisconnectedCallback callback = null)
    {
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
            yield return null;

        callback?.Invoke();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Player " + otherPlayer.NickName + " has left the room.");
        GUIController.Instance.DisplayPlayerForfeitedCanvas(otherPlayer.NickName);
    }
}
