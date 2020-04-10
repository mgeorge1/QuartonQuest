using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverCanvas : MonoBehaviour
{
    public TextMeshProUGUI GameOverText;
    public TextMeshProUGUI StoryGameOverText;
    public TextMeshProUGUI RematchRequestText;
    public GameObject RematchRequestPanel;
    public GameObject GameOverPanel;
    public GameObject StoryGameOverPanel;
    public Button RematchButton;

    public void SetGameOverText(string text)
    {
        GameOverText.text = text;
        StoryGameOverText.text = text;
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
        GUIController.Instance.LoadSceneWithTransition(GUIController.SceneNames.MainMenu);
    }

    public void DisplayRematchRequest(string text)
    {
        GameOverPanel.SetActive(false);
        RematchRequestText.text = text;
        RematchRequestPanel.SetActive(true);
    }

    public void DisplayStoryGameOverPanel()
    {
        GameOverPanel.SetActive(false);
        StoryGameOverPanel.SetActive(true);
    }

    public void HideRematchRequest()
    {
        RematchRequestPanel.SetActive(false);
    }

    public void AcceptButtonClicked()
    {
        GameCoreController.Instance.ReplayGame();
    }

    public void ContinueButtonClicked()
    {
        Debug.Log(GUIController.CurrentStoryScene.ToString());
        switch (GUIController.CurrentStoryScene)
        {
            case GUIController.SceneNames.Story1:
                GUIController.Instance.LoadSceneWithTransition(GUIController.SceneNames.Story2);
                break;
            case GUIController.SceneNames.Story2:
                GUIController.Instance.LoadSceneWithTransition(GUIController.SceneNames.Credits);
                break;
        }
    }
}
