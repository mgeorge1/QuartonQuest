using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace Networking {
    // Source: https://www.youtube.com/watch?v=gxUCMOlISeQ
    public class PlayerNameInput : MonoBehaviour
    {
        [SerializeField] private TMP_InputField nameInputField = null;
        [SerializeField] private Button hostbutton = null;
        [SerializeField] private Button joinButton = null;

        private const string PlayerPrefsNameKey = "PlayerNetworkName";

        private void Start() => SetUpInputField();

        private void SetUpInputField()
        {
            string defaultName = "";
            if (!PlayerPrefs.HasKey(PlayerPrefsNameKey))
            {
                defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);
                nameInputField.text = defaultName;
            }
            SetButtonsInteractable(defaultName);
        }

        public void SetButtonsInteractable(string name)
        {
            hostbutton.interactable = joinButton.interactable = !string.IsNullOrEmpty(name);
        }

        public void SavePlayerName()
        {
            string playerName = nameInputField.text;

            PhotonNetwork.NickName = playerName;

            PlayerPrefs.SetString(PlayerPrefsNameKey, playerName);
        }
    }
}