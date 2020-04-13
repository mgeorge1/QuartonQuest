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
        [SerializeField] private GameObject ConnectingPanel = null;
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
        private Dictionary<string, RoomInfo> cachedRooms;

        private void Awake() => PhotonNetwork.AutomaticallySyncScene = true;

        private void Start()
        {
            HostPanelController hpc = HostGamePanel.GetComponent<HostPanelController>();
            hpc.OnCancel += CancelMatchmaking;

            JoinPanelController jpc = JoinGamePanel.GetComponent<JoinPanelController>();
            jpc.OnCancel += CancelMatchmaking;

            rooms = new Dictionary<string, GameObject>();
            cachedRooms = new Dictionary<string, RoomInfo>();
        }

        // This probably shouldn't live here forever
        public void ReturnToMainMenu()
        {
            GUIController.Instance.LoadSceneWithTransition(GUIController.SceneNames.MainMenu);
        }

        private System.Collections.IEnumerator CancelMatchmaking()
        {
            DestroyAllRoomListItems();
            Disconnect();
            while (PhotonNetwork.IsConnected)
                yield return null;
            isMasterClient = false;
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
            ConnectingPanel.SetActive(true);
            NameInputPanel.SetActive(false);
            CreateGame();
        }

        public void OnJoinButtonClicked()
        {
            waitingStatus.text = "Connecting to multiplayer...";
            ConnectingPanel.SetActive(true);
            NameInputPanel.SetActive(false);
            JoinGame();
        }

        public void CreateGame()
        {
            isMasterClient = true;
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
                AdvanceToJoinPanel();
            }
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("Roomlist update");
            DestroyAllRoomListItems();
            UpdateCachedRoomList(roomList);
            AddNewRoomListItems();
        }

        private void UpdateCachedRoomList(List<RoomInfo> roomList)
        {
            foreach (RoomInfo info in roomList)
            {
                // Remove room from cached room list if it got closed, became invisible or was marked as removed
                if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
                {
                    if (cachedRooms.ContainsKey(info.Name))
                    {
                        cachedRooms.Remove(info.Name);
                    }

                    continue;
                }

                // Update cached room info
                if (cachedRooms.ContainsKey(info.Name))
                {
                    cachedRooms[info.Name] = info;
                }
                // Add new room info to cache
                else
                {
                    cachedRooms.Add(info.Name, info);
                }
            }
        }

        private void AddNewRoomListItems()
        {
            foreach (RoomInfo roomInfo in cachedRooms.Values)
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

        private void DestroyAllRoomListItems()
        {
            foreach (GameObject room in rooms.Values)
            {
                Destroy(room);
            }
            rooms.Clear();
        }

        public void OnRoomButtonClicked(string roomId, string opponentName)
        {
            JoinPanelController jpc = JoinGamePanel.GetComponent<JoinPanelController>();
            jpc.SetJoiningStatus();
            jpc.DisableCancelButton();

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
                waitingStatus.text = "Creating match...";
                CreateRoom();
            } else
            {
                PhotonNetwork.JoinLobby();
                AdvanceToJoinPanel();
            }
        }

        public void AdvanceToJoinPanel()
        {
            JoinGamePanel.SetActive(true);
            ConnectingPanel.SetActive(false);
        }

        public override void OnCreatedRoom()
        {
            Debug.Log("Room " + PhotonNetwork.CurrentRoom.Name + " created.");
            ConnectingPanel.SetActive(false);
            HostGamePanel.SetActive(true);
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            // Set a limit to how many times this can happen?
            Debug.LogError("Creating room failed.");
            GUIController.Instance.DisplayErrorCanvas("Could not create game");
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Client successfully joined a room");

            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

            if(playerCount != MAXPLAYERSPERROOM)
            {
                Debug.Log("Client is waiting for an opponent");
            }
            else
            {
                // Currently, this code does not always finish because the master client may load the scene 
                // during this function. 
                string opponent = PhotonNetwork.CurrentRoom.CustomProperties[MASTERPLAYERNAMEPROPERTY].ToString();
                waitingStatus.text = "Opponent Found: " + opponent;
                Debug.Log("Match is ready to begin");
            }
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            base.OnJoinRoomFailed(returnCode, message);
            Debug.Log("Client failed to join the room");
            JoinPanelController jpc = JoinGamePanel.GetComponent<JoinPanelController>();
            jpc.ResetStatus();
            jpc.EnableCancelButton();

            string errorMessage;
            if (!isMasterClient)
            {
                errorMessage = $"Could not join {NetworkController.OpponentName}'s match";
                GUIController.Instance.DisplayErrorCanvas(errorMessage, () => GUIController.Instance.HideErrorCanvas());
            }
            else
            {
                errorMessage = "Could not create match";
                GUIController.Instance.DisplayErrorCanvas(errorMessage, () => {
                    StartCoroutine(CancelMatchmaking());
                    GUIController.Instance.HideErrorCanvas();
                });
            }
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