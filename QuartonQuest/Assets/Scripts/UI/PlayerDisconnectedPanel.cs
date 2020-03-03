using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerDisconnectedPanel : MonoBehaviour
{
    public TextMeshProUGUI PlayerDisconnectedText;

    public void SetPlayerName(string name)
    {
        PlayerDisconnectedText.text = name + " has disconnected.";
    }

    public void ReturnToMainMenu()
    {
        StartCoroutine(NetworkController.Instance.Disconnect(() => {
            GUIController.Instance.LoadScene(GUIController.SceneNames.MainMenu);
       }));
    }
}
