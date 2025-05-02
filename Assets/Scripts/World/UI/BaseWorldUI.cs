using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWorldUI : MonoBehaviour
{
    protected WorldUIManager uiManager;

    public virtual void Init(WorldUIManager uiManager)
    {
        this.uiManager = uiManager;
    }

    protected abstract UIState GetUIState();

    public void SetActive(UIState state)
    {
        gameObject.SetActive(GetUIState() == state);
    }
}
