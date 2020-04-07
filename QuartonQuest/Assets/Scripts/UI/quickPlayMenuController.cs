using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class quickPlayMenuController : MonoBehaviour
{
    public TextMeshProUGUI FirstTurnToggleLabel;
    private const string PLAYER = "Player";
    private const string OPPONENT = "Opponent";

    // Start is called before the first frame update
    void Start()
    {
        FirstTurnToggleLabel.text = PLAYER;
        GUIController.Instance.PlayerGoesFirst = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startGame_buttonClicked()
    {
        GUIController.Opponent = GUIController.OpponentType.AI;
        SceneManager.LoadScene(GUIController.SceneNames.Level1);
    }

    public void OnFirstTurnButtonClicked()
    {
        if(FirstTurnToggleLabel.text == PLAYER)
        {
            GUIController.Instance.PlayerGoesFirst = false;
            FirstTurnToggleLabel.text = OPPONENT;
        }
        else
        {
            GUIController.Instance.PlayerGoesFirst = true;
            FirstTurnToggleLabel.text = PLAYER;
        }
    }
}
