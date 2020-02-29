using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GUIController : MonoBehaviour
{
    // https://wiki.unity3d.com/index.php/Singleton
    // This allows for GUIController to be a singleton
    private static GUIController _instance;
    private static bool _shuttingDown = false;
    private static object _lock = new object();
    public static GUIController Instance 
    {
        get
        {
            if (_shuttingDown)
                return null;

            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = (GUIController)FindObjectOfType(typeof(GUIController));

                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<GUIController>();
                        singleton.name = typeof(GUIController).ToString() + "Singleton";

                        DontDestroyOnLoad(singleton);
                    }
                }

                return _instance;
            }
        }
    }

    [SerializeField] public bool IsNetworkedGame = false;
    public bool IsPlayerFirst = true;

    public GameObject OpponentControllerObject;
    private IOpponent OpponentController;

    private static bool playerGoesFirst = true;
    private static bool playerDidSelectTurn = false;
    public bool PlayerGoesFirst
    {
        get
        {
            return playerGoesFirst;
        }

        set
        {
            Debug.Log("Setting PlayerGoesFirst");
            playerDidSelectTurn = true;
            playerGoesFirst = value;
        }
    }

    public struct SceneNames
    {
        public const string Level1 = "Level1Scene";
        public const string Level2 = "Level2Scene";
        public const string Level3 = "Level3Scene";
        public const string MainMenu = "MainMenuScene";
        public const string QuickPlayMenu = "QuickPlayMenuScene";
        public const string NetworkMenu = "NetworkMenuScene";
        public const string NetworkGame = "NetGameScene";
        public const string Story1 = "Story1Scene";
        public const string Story2 = "Story2Scene";
        public const string Credits = "CreditsScene";
    }

   void Awake()
    {
        Debug.Log("GUIController initializing");
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        Debug.Log("Disabling GUIController");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Checking scene");
        switch (scene.name)
        {
            case SceneNames.Level1:
            case SceneNames.Level2:
            case SceneNames.Level3:
            case SceneNames.NetworkGame:
                StartGame();
                break;
        }
    }

    void StartGame()
    {
        if (IsNetworkedGame)
            InstantiateNetworkController();
        else
            InstantiateAIController();
           
        OpponentController = OpponentControllerObject.GetComponent<IOpponent>();
        GameCoreController.Instance.Opponent = OpponentController;

        Debug.Log("PlayerDidSelectTurn = " + playerDidSelectTurn);
        if (playerDidSelectTurn)
        {
            StartCoroutine(GameCoreController.Instance.PlayGame(PlayerGoesFirst));
            playerDidSelectTurn = false;
        }
        else
            StartCoroutine(GameCoreController.Instance.PlayGame());
    }

    void InstantiateNetworkController()
    {
        OpponentControllerObject = PhotonNetwork.Instantiate("NetworkController", Vector3.zero, Quaternion.identity);
    }

    void InstantiateAIController()
    {
        OpponentControllerObject = new GameObject();
        OpponentControllerObject.AddComponent(typeof(AIController));
        OpponentControllerObject.name = typeof(AIController).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnApplicationQuit()
    {
        _shuttingDown = true;
    }

    void OnDestory()
    {
        _shuttingDown = true;
    }
}
