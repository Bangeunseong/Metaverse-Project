using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameIntroUI : BaseGameUI
{
    [SerializeField] private Button startBtn;
    [SerializeField] private Button exitBtn;

    /// <summary>
    /// Initialize Game Intro UI
    /// </summary>
    /// <param name="uiManager"></param>
    public override void Init(GameUIManager uiManager)
    {
        base.Init(uiManager);

        startBtn.onClick.AddListener(OnClickStartButton);
        exitBtn.onClick.AddListener(OnClickExitButton);
    }

    public void OnClickStartButton()
    {
        GameManager.Instance.StartGame();
    }

    public void OnClickExitButton()
    {
        SceneManager.LoadScene(GlobalGameManager.MainSceneName);
    }

    protected override UIState GetUIState()
    {
        return UIState.GameIntro;
    }
}
