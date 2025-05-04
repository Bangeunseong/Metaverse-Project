using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : BaseGameUI
{
    [SerializeField] private Button restartBtn;
    [SerializeField] private Button exitBtn;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI bestText;

    public override void Init(GameUIManager uiManager)
    {
        base.Init(uiManager);

        restartBtn.onClick.AddListener(OnClickRestartButton);
        exitBtn.onClick.AddListener(OnClickExitButton);
    }

    public void ChangeScoreText(int score) 
    {
        scoreText.text = score.ToString();
    }

    public void ChangeBestScoreText(int score)
    {
        bestText.text = score.ToString();
    }

    public void OnClickRestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnClickExitButton()
    {
        GlobalGameManager.Instance.IsGameNormallyEnded = true;
        SceneManager.LoadScene(GlobalGameManager.MainSceneName);
    }

    protected override UIState GetUIState()
    {
        return UIState.GameOver;
    }
}
