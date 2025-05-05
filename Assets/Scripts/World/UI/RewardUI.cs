using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardUI : BaseWorldUI
{
    [SerializeField] private Button confirmBtn;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI scoreText;

    public override void Init(WorldUIManager uiManager)
    {
        base.Init(uiManager);

        confirmBtn.onClick.AddListener(OnClickConfirmBtn);
    }

    public void OnClickConfirmBtn()
    {
        uiManager.ChangeState(UIState.World);
    }

    public void ChangeGainedScoreText()
    {
        scoreText.text = GlobalGameManager.Instance.GainedScore.ToString();
    }

    public void ChangeGainedCoinText()
    {
        coinText.text = GlobalGameManager.Instance.GainedCoin.ToString();
    }

    protected override UIState GetUIState()
    {
        return UIState.Reward;
    }
}
