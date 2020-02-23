using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GUIController : MonoBehaviour
{
    [SerializeField] public bool IsNetworkedGame = false;
    public GameObject NetworkControllerPrefab;

    public GameObject OpponentControllerObject;
    private IOpponent OpponentController;

    // Start is called before the first frame update
    void Start()
    {
        if (IsNetworkedGame)
            InstantiateNetworkController();

        OpponentController = OpponentControllerObject.GetComponent<IOpponent>();
        GameCoreController.Instance.Opponent = OpponentController;
        StartCoroutine(GameCoreController.Instance.PlayGame());
    }

    void InstantiateNetworkController()
    {
        OpponentControllerObject = PhotonNetwork.Instantiate("NetworkController", Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
