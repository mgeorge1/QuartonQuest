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

    public void SendForfeit()
    {
        rpcController.Forfeit();
    }

    public void RequestRematch()
    {
        rpcController.RequestRematch();
    }

    public IEnumerator Disconnect()
    {
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
            yield return null;
        Debug.Log("Disconnected from the PhotonNetwork");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Player " + otherPlayer.NickName + " has left the room.");
        //GUIController.Instance.DisplayGameOverCanvas(otherPlayer.NickName);
    }

    public void ReplayGame()
    {
        rpcController.ReplayGame();
    }

    public void ReloadGameScene()
    {
        // Normally, only the master should be able to run
        // this. There is no guard here because when a rematch
        // is requested, every client needs to reload the scene
        // themselves. Photon clients seem to ignore the master
        // when it reloads the exact same scene
        PhotonNetwork.LoadLevel(GUIController.Instance.CurrentScene);
    }

    #region PhotonCallbacks
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Disconnected due to: {cause}");
        switch (cause)
        {
            case DisconnectCause.ExceptionOnConnect:
                GUIController.Instance.DisplayErrorCanvas("Could not connect to multiplayer");
                break;
            case DisconnectCause.DisconnectByClientLogic:
                // Don't do anything, probably happened on purpose
                break;
            default:
                GUIController.Instance.DisplayErrorCanvas($"Disconnected from multiplayer{System.Environment.NewLine}Try again later");
                break;
        }
    }
    #endregion PhotonCallbacks
}
