using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int currentWaveIndex = 0;
    [SerializeField] private AudioClip gameMusic;

    private ResourceController playerResourceController;
    private GameUIManager gameUIManager;
    private EnemyManager enemyManager;
    private ItemManager itemManager;

    public MiniGamePlayerController Player { get; private set; }
    public bool IsGameActive { get; private set; } = false;
    public int CurrentScore { get; private set; }
    public int MaxScore { get; private set; }

    // Singleton
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = Helper.GetComponent_Helper<GameManager>(GameObject.FindWithTag(nameof(GameManager)));
            return instance;
        }
    }

    // Awake is called when scripts being loaded
    private void Awake()
    {
        if(Instance != this) { Destroy(gameObject); }

        // Initialize Player Controller
        Player = FindFirstObjectByType<MiniGamePlayerController>();
        Player.Init(this);

        // Initialize Item Manager
        itemManager = FindFirstObjectByType<ItemManager>();
        itemManager.Init(this);

        // Initialize Enemy Manager
        enemyManager = FindFirstObjectByType<EnemyManager>();
        enemyManager.Init(this, itemManager);

        // Initialize UIManager and ResourceController of Player
        playerResourceController = Helper.GetComponent_Helper<ResourceController>(Player.gameObject);
        gameUIManager = FindFirstObjectByType<GameUIManager>();
        
        // Set Event when hp is changed
        playerResourceController.RemoveHealthChangeEvent(gameUIManager.ChangePlayerHP);
        playerResourceController.AddHealthChangeEvent(gameUIManager.ChangePlayerHP);
    }

    private void Start()
    {
        // Set max score
        MaxScore = GlobalGameManager.Instance.MaxScore;
        
        // Initialize Player HP Sprites
        gameUIManager.SetPlayerHP(playerResourceController.MaxHealth);
        UpdateScore(0);

        SoundManager.Instance.ChangeBackgroundMusic(gameMusic);
    }

    public void StartGame() { IsGameActive = true; gameUIManager.SetPlayGame(); StartNextWave(); }
    public void ContinueGame() { IsGameActive = true; gameUIManager.SetPlayGame(); }
    public void PauseGame() { IsGameActive = false; gameUIManager.SetGamePause(); }
    public void GameOver()
    {
        // Stops Game
        IsGameActive = false;

        // Update Best score, coin, and UI
        UpdateScoreNCoin_InGlobalGameManager();
        gameUIManager.ChangeScoreInGameOverUI(CurrentScore, MaxScore);

        // Stop spawning enemies
        enemyManager.StopWave();

        // Change UI to GameOverUI
        gameUIManager.SetGameOver();
    }

    public void UpdateScoreNCoin_InGlobalGameManager()
    {
        if (CurrentScore > MaxScore) { MaxScore = CurrentScore; }
        GlobalGameManager.Instance.UpdateGameScore(CurrentScore);
        GlobalGameManager.Instance.UpdateCurrentCoin(CurrentScore / 10, true);
    }

    public void UpdateScore(int score)
    {
        CurrentScore += score;
        gameUIManager.ChangeScoreInGameUI(CurrentScore);
    }
    
    public void EndOfWave() { StartNextWave(); }
    
    private void StartNextWave()
    {
        currentWaveIndex++;
        if(currentWaveIndex % 5 == 0) { gameUIManager.ShowBossAppearUI(); }
        enemyManager.StartWave(currentWaveIndex);
        gameUIManager.ChangeWave(currentWaveIndex);
    }
}
