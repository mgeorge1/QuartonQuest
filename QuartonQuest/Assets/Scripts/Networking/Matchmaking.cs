using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        private const string MASTERPLAYERNAMEPROPERTY = "MasterPlayerName";
        private Dictionary<string, GameObject> rooms;

        private void Awake() => PhotonNetwork.AutomaticallySyncScene = true;

        private void Start()
        {
            HostPanelController hpc = HostGamePanel.GetComponent<HostPanelController>();
            hpc.OnCancel += CancelMatchmaking;

            JoinPanelController jpc = JoinGamePanel.GetComponent<JoinPanelController>();
            jpc.OnCancel += CancelMatchmaking;

            rooms = new Dictionary<string, GameObject>();
        }

        // This probably shouldn't live here forever
        public void ReturnToMainMenu()
        {
            SceneManager.LoadScene("MainMenuScene");
        }

        private void CancelMatchmaking()
        {
            Disconnect();
            NameInputPanel.SetActive(true);
        }

        private void Disconnect()
        {
            if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.IsMasterClient)
            {
                Debug.Log("Leaving Room: " + PhotonNetwork.CurrentRoom.Name);
                PhotonNetwork.CurrentRoom.IsVisible = false;
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.LeaveRoom();
            }

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
            Debug.Log("Roomlist update");
            RemoveUnavailableRooms(roomList);
            AddNewRoomListItems(roomList);
        }

        private void AddNewRoomListItems(List<RoomInfo> roomList)
        {
            foreach (RoomInfo roomInfo in roomList)
            {
                if (!rooms.ContainsKey(roomInfo.Name) && roomInfo.IsOpen && roomInfo.IsVisible)
                {
                    Debug.Log("Creating room list item...");
                    GameObject newItem = Instantiate<GameObject>(RoomListItemPrefab, RoomListItemsPanel.transform, false);
                    RoomListItemController controller = newItem.GetComponent<RoomListItemController>();
                    rooms.Add(roomInfo.Name, newItem);
                    controller.RoomId = roomInfo.Name;
                    Debug.Log(roomInfo.CustomProperties[MASTERPLAYERNAMEPROPERTY]);
                    string opponent = roomInfo.CustomProperties[MASTERPLAYERNAMEPROPERTY].ToString();
                    controller.MasterPlayerName = opponent;
                    controller.buttonText.text = controller.MasterPlayerName;
                    controller.button.onClick.AddListener(delegate { OnRoomButtonClicked(roomInfo.Name, opponent); });
                }
            }
        }

        private void RemoveUnavailableRooms(List<RoomInfo> roomList)
        {
            List<string> keysToRemove = new List<string>();
            foreach (string roomName in rooms.Keys)
            {
                Debug.Log("Rooms on server:");
                foreach (var info in roomList)
                    Debug.Log(info.Name);

                if (!roomList.Exists(roomInfo => roomInfo.Name == roomName))
                {
                    DestroyRoomListItem(roomName);
                    keysToRemove.Add(roomName);
                } 
                else
                {
                    RoomInfo ri = roomList.Find(roomInfo => roomInfo.Name == roomName);
                    Debug.Log(ri.IsVisible + " " + ri.IsOpen + " " + ri.RemovedFromList);
                    if (!ri.IsVisible || !ri.IsOpen || ri.RemovedFromList)
                    {
                        DestroyRoomListItem(roomName);
                    }
                }
            }

            foreach (string key in keysToRemove)
                rooms.Remove(key);
        }

        private void DestroyRoomListItem(string roomName)
        {
            Debug.Log(("Destroying room that doesn't exist on server..."));
            GameObject item = rooms[roomName];
            Destroy(item);
        }

        public void OnRoomButtonClicked(string roomId, string opponentName)
        {
            NetworkController.OpponentName = opponentName;
            GUIController.Opponent = GUIController.OpponentType.NETWORK;
            PhotonNetwork.JoinRoom(roomId);
        }

        private void CreateRoom()
        {
            var props = new ExitGames.Client.Photon.Hashtable();
            props.Add(MASTERPLAYERNAMEPROPERTY, PhotonNetwork.NickName);

            PhotonNetwork.CreateRoom(null, new RoomOptions
            {
                MaxPlayers = MAXPLAYERSPERROOM,
                IsVisible = true,
                IsOpen = true,
                CustomRoomProperties = props,
                CustomRoomPropertiesForLobby = new string[] { MASTERPLAYERNAMEPROPERTY }
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

        public override void OnJoinedRoom()
        {
            // Currently, this code does not always finish because the master client may load the scene 
            // during this function. 
            Debug.Log("Client successfully joined a room");

            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

            if(playerCount != MAXPLAYERSPERROOM)
            {
                waitingStatus.text = "Waiting For Opponent...";
                Debug.Log("Client is waiting for an opponent");
            }
            else
            {
                string opponent = PhotonNetwork.CurrentRoom.CustomProperties[MASTERPLAYERNAMEPROPERTY].ToString();
                waitingStatus.text = "Opponent Found: " + opponent;
                Debug.Log("Match is ready to begin");
            }
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            base.OnJoinRoomFailed(returnCode, message);
            Debug.Log("Client failed to join the room");
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            //base.OnPlayerEnteredRoom(newPlayer);

            if(PhotonNetwork.CurrentRoom.PlayerCount == MAXPLAYERSPERROOM)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;

                waitingStatus.text = "Opponent Found: " + newPlayer.NickName;
                Debug.Log("Match is ready to begin with " + newPlayer.NickName);

                NetworkController.OpponentName = newPlayer.NickName;
                GUIController.Opponent = GUIController.OpponentType.NETWORK;

                if (PhotonNetwork.IsMasterClient)
                    PhotonNetwork.LoadLevel(GUIController.SceneNames.Level1);
            }
        }
    }
}