using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverCanvas : MonoBehaviour
{
    public TextMeshProUGUI GameOverText;
    public TextMeshProUGUI RematchRequestText;
    public GameObject RematchRequestPanel;
    public GameObject GameOverPanel;
    public Button RematchButton;

    public void SetGameOverText(string text)
    {
        GameOverText.text = text;
    }

    public void RematchButtonClicked()
    {
        RematchButton.interactable = false;
        GUIController.Instance.RequestRematchFromOpponent();
    }

    public void MainMenuButtonClicked()
    {
        StartCoroutine(ReturnToMainMenu());
    }

    public IEnumerator ReturnToMainMenu()
    {
        yield return GameCoreController.Instance.Disconnect();
        GUIController.Instance.LoadScene(GUIController.SceneNames.MainMenu);
    }

    public void DisplayRematchRequest(string text)
    {
        GameOverPanel.SetActive(false);
        RematchRequestText.text = text;
        RematchRequestPanel.SetActive(true);
    }

    public void HideRematchRequest()
    {
        RematchRequestPanel.SetActive(false);
    }

    public void AcceptButtonClicked()
    {
        GameCoreController.Instance.ReplayGame();
    }
}
