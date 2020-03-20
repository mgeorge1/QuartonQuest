using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerForfeitedCanvas : MonoBehaviour
{
    public TextMeshProUGUI PlayerForfeitedText;

    public void SetPlayerName(string name)
    {
        PlayerForfeitedText.text = name + " has forfeited.";
    }

    public void ReturnToMainMenu()
    {
        StartCoroutine(NetworkController.Instance.Disconnect(() => {
            GUIController.Instance.LoadScene(GUIController.SceneNames.MainMenu);
       }));
    }
}
