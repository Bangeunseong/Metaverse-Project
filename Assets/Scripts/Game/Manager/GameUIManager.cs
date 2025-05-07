using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    private GameIntroUI gameIntroUI;
    private GameUI gameUI;
    private GameOverUI gameOverUI;
    private GamePauseUI gamePauseUI;

    private UIState currentState;

    /// <summary>
    /// Awake is called once when scripts being loaded
    /// </summary>
    void Awake()
    {
        gameIntroUI = Helper.GetComponentInChildren_Helper<GameIntroUI>(gameObject, true);
        gameUI = Helper.GetComponentInChildren_Helper<GameUI>(gameObject, true);
        gameOverUI = Helper.GetComponentInChildren_Helper<GameOverUI>(gameObject, true);
        gamePauseUI = Helper.GetComponentInChildren_Helper<GamePauseUI>(gameObject, true);

        gameIntroUI.Init(this);
        gameUI.Init(this);
        gameOverUI.Init(this);
        gamePauseUI.Init(this);

        ChangeState(UIState.GameIntro);
    }

    /// <summary>
    /// Change UI State to GameUI
    /// </summary>
    public void SetPlayGame()
    {
        ChangeState(UIState.Game);
    }

    /// <summary>
    /// Change UI State to PauseUI
    /// </summary>
    public void SetGamePause()
    {
        ChangeState(UIState.PauseGame);
    }

    /// <summary>
    /// Change UI State to GameOverUI
    /// </summary>
    public void SetGameOver()
    {
        ChangeState(UIState.GameOver);
    }

    /// <summary>
    /// Change UI Active by state
    /// </summary>
    /// <param name="state"></param>
    public void ChangeState(UIState state)
    {
        currentState = state;
        gameIntroUI.SetActive(currentState);
        gameUI.SetActive(currentState);
        gameOverUI.SetActive(currentState);
        gamePauseUI.SetActive(currentState);
    }

    /// <summary>
    /// Change UI when the wave is ended
    /// </summary>
    /// <param name="waveIndex"></param>
    public void ChangeWave(int waveIndex)
    {
        gameUI.UpdateWaveText(waveIndex);
    }

    /// <summary>
    /// Change Score UI when the enemy dies
    /// </summary>
    /// <param name="score"></param>
    public void ChangeScoreInGameUI(int score)
    {
        gameUI.UpdateScoreText(score);
    }

    /// <summary>
    /// Change Score and Best Score UI when game overs
    /// </summary>
    /// <param name="score"></param>
    /// <param name="bestScore"></param>
    public void ChangeScoreInGameOverUI(int score, int bestScore)
    {
        gameOverUI.ChangeScoreText(score);
        gameOverUI.ChangeBestScoreText(bestScore);
    }

    /// <summary>
    /// Initialize HP sprites
    /// </summary>
    /// <param name="maxHP"></param>
    public void SetPlayerHP(float maxHP)
    {
        gameUI.InitHPSprite(Mathf.CeilToInt(maxHP));
    }

    /// <summary>
    /// Change HP sprites using currentHP and maxHP
    /// </summary>
    /// <param name="currentHP"></param>
    public void ChangePlayerHP(float currentHP)
    {
        gameUI.UpdateHPSprite(Mathf.CeilToInt(currentHP));
    }

    public void ShowBossAppearUI()
    {
        gameUI.ShowBossAppearUI();
    }
}
