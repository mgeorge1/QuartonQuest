using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace Networking {
    // Source: https://www.youtube.com/watch?v=gxUCMOlISeQ
    public class PlayerNameInput : MonoBehaviour
    {
        [SerializeField] private TMP_InputField nameInputField = null;
        [SerializeField] private Button startButton = null;

        private const string PlayerPrefsNameKey = "PlayerNetworkName";

        private void Start() => SetUpInputField();

        private void SetUpInputField()
        {
            if (!PlayerPrefs.HasKey(PlayerPrefsNameKey)) { return; }

            string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);

            nameInputField.text = defaultName;

            SetStartButtonInteractable(defaultName);
        }

        public void SetStartButtonInteractable(string name)
        {
            startButton.interactable = !string.IsNullOrEmpty(name);
        }

        public void SavePlayerName()
        {
            string playerName = nameInputField.text;

            PhotonNetwork.NickName = playerName;

            PlayerPrefs.SetString(PlayerPrefsNameKey, playerName);
        }
    }
}