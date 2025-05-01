using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class GameManager : MonoBehaviour
{
    private const string ScoreKey = "MiniGame_Score";

    private static GameManager instance;
    private static int currentScene = 0;
    public static Vector2 worldSpawnPosition = new Vector2(0f, -0.5f);
    public static bool isFirstLoadingInWorld = true;
    public static bool isFirstLoadingInGame = true;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = Helper.GetComponent_Helper<GameManager>(GameObject.FindWithTag(nameof(GameManager)));
            return instance;
        }
    }

    private void Awake()
    {
        if (Instance != this) Destroy(gameObject);
        else DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (currentScene == 0)
        {
            if (!isFirstLoadingInWorld) { }
            else isFirstLoadingInWorld = false;
        }
        else
        {
            if (!isFirstLoadingInGame) { }
            else isFirstLoadingInGame = false;
        }
    }
}
