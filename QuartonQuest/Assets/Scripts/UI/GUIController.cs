using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIController : MonoBehaviour
{
    public GameObject GameCoreControllerObject;
    private GameCoreController GameCoreController;
    public GameObject AIControllerObject;
    private AIController AIController;

    // Start is called before the first frame update
    void Start()
    {
        GameCoreController = GameCoreControllerObject.GetComponent<GameCoreController>();
        AIController = AIControllerObject.GetComponent<AIController>();
        GameCoreController.Instance.Opponent = AIController;

        StartCoroutine(GameCoreController.PlayGame());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
