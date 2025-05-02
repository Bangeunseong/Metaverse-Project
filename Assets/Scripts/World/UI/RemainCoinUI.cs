using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RemainCoinUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI remainCoin;

    public void Init()
    {
        remainCoin.text = $": {GlobalGameManager.Instance.CurrentCoin}";
    }

    public void UpdateCoinUI()
    {
        remainCoin.text = $": {GlobalGameManager.Instance.CurrentCoin}";
    }
}
