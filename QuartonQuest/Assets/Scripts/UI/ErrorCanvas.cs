using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ErrorCanvas : MonoBehaviour
{
    public TextMeshProUGUI ErrorText;

    public void MainMenuButtonClicked()
    {
        GUIController.Instance.LoadScene(GUIController.SceneNames.MainMenu);
    }

    public void SetErrorText(string text)
    {
        ErrorText.text = text;
    }
}
