using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace Networking
{
    // Source: https://www.youtube.com/watch?v=gxUCMOlISeQ
    public class Matchmaking : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject Panel_JoinGame = null;
        [SerializeField] private GameObject Panel_CreateGame = null;
        [SerializeField] private TextMeshProUGUI waitingStatusText = null;

        private bool isConnecting = false;

        private const string GameVersion = "0.1";
        private const int MaxPlayersPerRoom = 2;

        private void Awake() => PhotonNetwork.AutomaticallySyncScene = true;

        public void JoinGame()
        {
            isConnecting = true;

            Panel_JoinGame.SetActive(false);
            Panel_CreateGame.SetActive(true);

            waitingStatusText.text = "Searching. . .";

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                PhotonNetwork.GameVersion = GameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        public override void OnConnectedToMaster()
        {
            //base.OnConnectedToMaster();

            Debug.Log("Connected To Master");

            if (isConnecting)
            {
                PhotonNetwork.JoinRandomRoom();
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            //base.OnDisconnected(cause);

            Panel_CreateGame.SetActive(false);
            Panel_JoinGame.SetActive(true);

            Debug.Log($"Disconnected due to: {cause}");
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            //base.OnJoinRandomFailed(returnCode, message);

            Debug.Log("No clients are waiting for an opponent, creating a new room");

            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = MaxPlayersPerRoom });
        }

        public override void OnJoinedRoom()
        {
            //base.OnJoinedRoom();

            Debug.Log("Client successfully joined a room");

            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

            if(playerCount != MaxPlayersPerRoom)
            {
                waitingStatusText.text = "Waiting For Opponent";
                Debug.Log("Client is waiting for an opponent");
            }
            else
            {
                waitingStatusText.text = "Opponent Found";
                Debug.Log("Match is ready to begin");
            }
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            //base.OnPlayerEnteredRoom(newPlayer);

            if(PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayersPerRoom)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;

                waitingStatusText.text = "Opponent Found";
                Debug.Log("Match is ready to begin");

                PhotonNetwork.LoadLevel("SampleScene");
            }
        }
    }
}