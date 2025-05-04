using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossUIEvent : MonoBehaviour
{
    public void OnShowBossUIEnd()
    {
        gameObject.SetActive(false);
    }
}
