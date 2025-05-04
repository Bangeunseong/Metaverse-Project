using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGameUI : MonoBehaviour
{
    protected GameUIManager uiManager;

    public virtual void Init(GameUIManager uiManager)
    {
        this.uiManager = uiManager;
    }

    protected abstract UIState GetUIState();

    public void SetActive(UIState state)
    {
        gameObject.SetActive(GetUIState() == state);
    }
}
