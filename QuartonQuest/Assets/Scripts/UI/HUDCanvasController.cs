using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDCanvasController : MonoBehaviour
{
    public TextMeshProUGUI TurnText;

    // Start is called before the first frame update
    void Start()
    {
        
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
            case GameCoreController.GameTurnState.OPPONENTWON:
                TurnText.text = "Opponent Won";
                break;
            case GameCoreController.GameTurnState.GAMETIED:
                TurnText.text = "Tie!";
                break;
        }
    }
}
