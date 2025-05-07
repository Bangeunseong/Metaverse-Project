using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePauseUI : BaseGameUI
{
    [SerializeField] private Button cancelBtn;
    [SerializeField] private Button confirmBtn;

    public override void Init(GameUIManager uiManager)
    {
        base.Init(uiManager);

        cancelBtn.onClick.AddListener(OnClickCancelBtn);
        confirmBtn.onClick.AddListener(OnClickConfirmBtn);
    }

    public void OnClickCancelBtn()
    {
        GameManager.Instance.ContinueGame();
    }

    public void OnClickConfirmBtn()
    {
        GameManager.Instance.UpdateScoreNCoin_InGlobalGameManager();

        GlobalGameManager.Instance.IsGameNormallyEnded = true;
        SceneManager.LoadScene(GlobalGameManager.MainSceneName);
    }

    protected override UIState GetUIState()
    {
        return UIState.PauseGame;
    }
}
