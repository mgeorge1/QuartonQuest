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

    // Start is called before the first frame update
    void Start()
    {
        gameOverCanvasScript = GameOverCanvas.GetComponent<GameOverCanvas>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameCoreController.Instance.CurrentTurn)
        {
            case GameCoreController.GameTurnState.PLAYER:
                TurnText.text = "Your Turn";
                break;
            case GameCoreController.GameTurnState.PLAYERCHOOSEPIECE:
                TurnText.text = "Select a piece";
                break;
            case GameCoreController.GameTurnState.PLAYERCHOOSETILE:
                TurnText.text = "Select a tile";
                break;
            case GameCoreController.GameTurnState.OPPONENT:
                TurnText.text = "Opponent's Turn";
                break;
            case GameCoreController.GameTurnState.PLAYERWON:
                TurnText.text = "You won!";
                break;
            case GameCoreController.GameTurnState.OPPONENTFORFEIT:
                TurnText.text = "You won!";
                break;
            case GameCoreController.GameTurnState.OPPONENTWON:
                TurnText.text = "Opponent Won";
                break;
            case GameCoreController.GameTurnState.PLAYERFORFEIT:
                TurnText.text = "Opponent won!";
                break;
            case GameCoreController.GameTurnState.GAMETIED:
                TurnText.text = "Tie!";
                break;
        }
    }

    public void DisplayHelpPanel()
    {
        if (HelpPanel.activeSelf)
            HelpPanel.SetActive(false);
        else
            HelpPanel.SetActive(true);
    }

    public void DisplayGameOverCanvas(string gameOverText)
    {
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

    public void DisplayRematchRequest(string text)
    {
        gameOverCanvasScript.DisplayRematchRequest(text);
    }
}
