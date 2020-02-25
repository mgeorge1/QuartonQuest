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
        if (GameCoreController.Instance.CurrentTurn == GameCoreController.GameTurnState.PLAYER)
        {
            TurnText.text = "Player's Turn";
        } 
        else if (GameCoreController.Instance.CurrentTurn == GameCoreController.GameTurnState.OPPONENT)
        {
            TurnText.text = "Opponent's Turn";
        }
    }
}
