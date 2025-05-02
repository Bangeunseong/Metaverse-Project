using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroUI : BaseWorldUI
{
    [SerializeField] private Button startBtn;
    [SerializeField] private Button exitBtn;

    public override void Init(WorldUIManager uiManager)
    {
        base.Init(uiManager);

        startBtn.onClick.AddListener(OnClickStartButton);
        exitBtn.onClick.AddListener(OnClickExitButton);
    }

    public void OnClickStartButton()
    {
        WorldManager.Instance.StartWorld();
    }

    public void OnClickExitButton()
    {
        Application.Quit();
    }

    protected override UIState GetUIState()
    {
        return UIState.Intro;
    }
}
