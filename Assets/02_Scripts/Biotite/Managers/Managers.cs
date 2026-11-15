using Cysharp.Threading.Tasks.Triggers;
using ResourceLoader;
using UnityEngine;

public class Managers : MonoBehaviour
{

    public static Managers Instance { get; private set; } = null;


    private static GameManager gameManager;
    private static ObjectManager objectManager;
    private static ResourceManager resourceManager;
    private static EventManager eventManager;
    private static InputManager inputManager;
    private static SoundManager soundManager;
    private static BattleManager battleManager;
    private static RewardManager rewardManager;


    public static GameManager Game => gameManager;
    public static ObjectManager Object => objectManager;
    public static ResourceManager Resource => resourceManager;
    public static EventManager Event => eventManager;
    public static InputManager Input => inputManager;
    public static SoundManager SoundManager => soundManager;
    public static BattleManager Battle => battleManager;
    public static RewardManager Reward => rewardManager;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitManagers();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private async void InitManagers()
    {
        rewardManager = new();

        eventManager = new EventManager();
        eventManager.Init();

        resourceManager = GetComponentInChildren<ResourceManager>();
        await resourceManager.LoadResources();

        inputManager = GetComponentInChildren<InputManager>();
        objectManager = GetComponentInChildren<ObjectManager>();
        soundManager = GetComponentInChildren<SoundManager>();
        battleManager = GetComponentInChildren<BattleManager>();

        soundManager.Init();
        objectManager.Init();

        gameManager = new GameManager();
        gameManager.Init();

        battleManager.Init();
        rewardManager.Init();
    }


    private void OnDestroy()
    {
        if (Instance != null)
        {
            Instance = null;
        }
    }

}
