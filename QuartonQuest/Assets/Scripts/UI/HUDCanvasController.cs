using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDCanvasController : MonoBehaviour
{
    public TextMeshProUGUI TurnText;
    public GameObject HelpPanel;
    public GameObject GameOverCanvas;
    private GameOverCanvas gameOverCanvasScript;
    public string OpponentName;

    // Start is called before the first frame update
    void Start()
    {
        gameOverCanvasScript = GameOverCanvas.GetComponent<GameOverCanvas>();
    }

    // Update is called once per frame
    void Update()
    {
        TurnText.text = DecideTurnText();
    }

    public string DecideTurnText()
    {
        switch (GameCoreController.Instance.CurrentTurn)
        {
            case GameCoreController.GameTurnState.PLAYER:
                return $"Your Turn";
            case GameCoreController.GameTurnState.PLAYERCHOOSEPIECE:
                return "Select a piece";
            case GameCoreController.GameTurnState.PLAYERCHOOSETILE:
                return "Select a tile";
            case GameCoreController.GameTurnState.OPPONENT:
                return $"{OpponentName}'s Turn";
            case GameCoreController.GameTurnState.PLAYERWON:
            case GameCoreController.GameTurnState.OPPONENTFORFEIT:
                return $"You won!";
            case GameCoreController.GameTurnState.OPPONENTWON:
            case GameCoreController.GameTurnState.PLAYERFORFEIT:   
                return $"{OpponentName} won!";
            case GameCoreController.GameTurnState.GAMETIED:
                return "Tie!";
            default:
                return null;
        }
    }

    public void DisplayHelpPanel()
    {
        if (HelpPanel.activeSelf)
            HelpPanel.SetActive(false);
        else
            HelpPanel.SetActive(true);
    }

    public void DisplayGameOverCanvas(string gameOverText = null)
    {
        if (gameOverText == null)
        {
            gameOverText = DecideTurnText();
        }
        gameOverCanvasScript.SetGameOverText(gameOverText);
        GameOverCanvas.SetActive(true);
    }

    public void HideGameOverCanvas()
    {
        gameOverCanvasScript.HideRematchRequest();
        GameOverCanvas.SetActive(false);
    }

    public void SetGameOverText(string newText)
    {
        gameOverCanvasScript.SetGameOverText(newText);
    }

    public void DisplayRematchRequest()
    {
        gameOverCanvasScript.DisplayRematchRequest($"{OpponentName} has requested a rematch");
    }

    public void HideAllPanels()
    {
        HelpPanel.SetActive(false);
        HideGameOverCanvas();
    }
}
