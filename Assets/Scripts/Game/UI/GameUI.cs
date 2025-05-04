using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : BaseGameUI
{
    private readonly int isChanged = Animator.StringToHash("IsChanged_trig");

    [SerializeField] private Button exitBtn;
    [SerializeField] private Transform hpLayout;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private Sprite halfHeart;
    [SerializeField] private Sprite emptyHeart;
    [SerializeField] private GameObject bossUI;

    private Animator hpLayoutAnimator;

    private readonly List<Image> hearts = new();

    public override void Init(GameUIManager uiManager)
    {
        base.Init(uiManager);
        hpLayoutAnimator = Helper.GetComponent_Helper<Animator>(hpLayout.gameObject);

        exitBtn.onClick.AddListener(OnClickExitBtn);
    }

    public void InitHPSprite(int maxHP)
    {
        for(int i = 0; i < (maxHP / 2) + (maxHP % 2); i++)
        {
            GameObject heart = Instantiate(heartPrefab, hpLayout);
            Image img = Helper.GetComponent_Helper<Image>(heart);
            hearts.Add(img);
        }
        if(maxHP % 2 != 0) { hearts[maxHP / 2].sprite = halfHeart; }
    }

    public void UpdateHPSprite(int currentHP)
    {
        bool isHalfExists = currentHP % 2 != 0;
        int index;

        if (isHalfExists) { hearts[currentHP / 2].sprite = halfHeart; index = currentHP / 2 + 1; }
        else index = currentHP / 2;

        for(int i = index; i < hearts.Count; i++) hearts[i].sprite = emptyHeart;
        hpLayoutAnimator.SetTrigger(isChanged);
    }

    public void UpdateWaveText(int wave)
    {
        waveText.text = wave.ToString();
    }

    public void UpdateScoreText(int score)
    {
        scoreText.text = score.ToString();
    }

    public void ShowBossAppearUI()
    {
        bossUI.SetActive(true);
    }
    
    public void OnClickExitBtn()
    {
        GameManager.Instance.PauseGame();
    }

    protected override UIState GetUIState()
    {
        return UIState.Game;
    }
}
