using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace Networking
{
    // Source: https://www.youtube.com/watch?v=gxUCMOlISeQ
    public class Matchmaking : MonoBehaviourPunCallbacks, ILobbyCallbacks
    {
        [SerializeField] private GameObject JoinGamePanel = null;
        [SerializeField] private GameObject HostGamePanel = null;
        [SerializeField] private GameObject NameInputPanel = null;
        [SerializeField] private GameObject RoomListItemsPanel = null;
        [SerializeField] private GameObject RoomListItemPrefab = null;
        [SerializeField] private TextMeshProUGUI waitingStatus = null;
        [SerializeField] private TextMeshProUGUI masterPlayerName = null;

        private bool isConnecting = false;
        private bool isMasterClient = false;

        private const string GameVersion = "0.1";
        private const int MAXPLAYERSPERROOM = 2;

        private void Awake() => PhotonNetwork.AutomaticallySyncScene = true;

        private void Start()
        {
            HostPanelController hpc = HostGamePanel.GetComponent<HostPanelController>();
            hpc.OnCancel += CancelMatchmaking;
        }

        private void CancelMatchmaking()
        {
            NameInputPanel.SetActive(true);
            Disconnect();
        }

        private void Disconnect()
        {
            PhotonNetwork.Disconnect();
        }

        public void OnCreateButtonClicked()
        {
            HostGamePanel.SetActive(true);
            NameInputPanel.SetActive(false);
            CreateGame();
        }

        public void OnJoinButtonClicked()
        {
            JoinGamePanel.SetActive(true);
            NameInputPanel.SetActive(false);
            JoinGame();
        }

        public void CreateGame()
        {
            isMasterClient = true;
            waitingStatus.text = "Connecting to server...";
            PhotonNetwork.GameVersion = GameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }

        public void JoinGame()
        {
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.GameVersion = GameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
            else
            {
                PhotonNetwork.JoinLobby();
            }
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {

            foreach (RoomInfo roomInfo in roomList)
            {
                GameObject newItem = Instantiate<GameObject>(RoomListItemPrefab, RoomListItemsPanel.transform, false);
                RoomListItemController controller = newItem.GetComponent<RoomListItemController>();
                controller.RoomId = roomInfo.Name;
                controller.MasterPlayerName = roomInfo.CustomProperties["MasterPlayerName"].ToString();
                controller.buttonText.text = controller.MasterPlayerName;
                controller.button.onClick.AddListener(delegate { OnRoomButtonClicked(roomInfo.Name); });
            }
        }

        public void OnRoomButtonClicked(string roomId)
        {
            PhotonNetwork.JoinRoom(roomId);
        }

        private void CreateRoom()
        {
            var props = new ExitGames.Client.Photon.Hashtable();
            props.Add("MasterPlayerName", PhotonNetwork.NickName);

            PhotonNetwork.CreateRoom(null, new RoomOptions
            {
                MaxPlayers = MAXPLAYERSPERROOM,
                IsVisible = true,
                IsOpen = true,
                CustomRoomProperties = props,
                CustomRoomPropertiesForLobby = new string[] {"MasterPlayerName"}
            });
           
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected To Master");

            if (isMasterClient)
            {
                waitingStatus.text = "Creating room...";
                CreateRoom();
            } else
            {
                PhotonNetwork.JoinLobby();
            }
        }

        public override void OnCreatedRoom()
        {
            Debug.Log("Room " + PhotonNetwork.CurrentRoom.Name + " created.");
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            // Set a limit to how many times this can happen?
            Debug.LogError("Creating room failed. Trying again...");
            waitingStatus.text = "Creating room failed. Trying again...";
            CreateRoom();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log($"Disconnected due to: {cause}");
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Client successfully joined a room");

            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

            if(playerCount != MAXPLAYERSPERROOM)
            {
                waitingStatus.text = "Waiting For Opponent...";
                Debug.Log("Client is waiting for an opponent");
            }
            else
            {
                waitingStatus.text = "Opponent Found";
                Debug.Log("Match is ready to begin");
            }
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            //base.OnPlayerEnteredRoom(newPlayer);

            if(PhotonNetwork.CurrentRoom.PlayerCount == MAXPLAYERSPERROOM)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;

                waitingStatus.text = "Opponent Found";
                Debug.Log("Match is ready to begin");

                if (PhotonNetwork.IsMasterClient)
                    PhotonNetwork.LoadLevel("Level1Scene");
            }
        }
    }
}