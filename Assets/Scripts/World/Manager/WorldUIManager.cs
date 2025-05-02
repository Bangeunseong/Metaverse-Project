using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIState
{
    Intro, World, GameIntro, Game, GameOver,
}

public class WorldUIManager : MonoBehaviour
{
    private IntroUI introUI;
    private WorldUI worldUI;
    private UIState currentState;

    /// <summary>
    /// Awake is called once when scripts being loaded
    /// </summary>
    void Awake()
    {
        introUI = Helper.GetComponentInChildren_Helper<IntroUI>(gameObject, true);
        worldUI = Helper.GetComponentInChildren_Helper<WorldUI>(gameObject, true);

        introUI.Init(this);
        worldUI.Init(this);

        ChangeState(UIState.Intro);
    }

    /// <summary>
    /// Change UI State to World
    /// </summary>
    public void GoToWorld()
    {
        if (!GlobalGameManager.Instance.IsFirstLoadingInWorld)
            GlobalGameManager.Instance.IsFirstLoadingInWorld = false;
        ChangeState(UIState.World);
    }

    /// <summary>
    /// Change UI Active by state
    /// </summary>
    /// <param name="state"></param>
    public void ChangeState(UIState state)
    {
        currentState = state;
        introUI.SetActive(currentState);
        worldUI.SetActive(currentState);
    }
}
