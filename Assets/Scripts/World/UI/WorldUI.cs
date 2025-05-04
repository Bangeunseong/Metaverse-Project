using UnityEngine;
using UnityEngine.UI;

public class WorldUI : BaseWorldUI
{
    [SerializeField] private Button characterCustomButton;
    [SerializeField] private Button currentCoinButton;
    [SerializeField] private Button exitBtn;
    [SerializeField] private GameObject customCharacterPanel;
    [SerializeField] private GameObject currentCoinPanel;
    private CustomCharacterUI customCharacterUI;
    private RemainCoinUI remainCoinUI;

    public override void Init(WorldUIManager uiManager)
    {
        base.Init(uiManager);

        characterCustomButton.onClick.AddListener(OnClickCharacterCustomButton);
        currentCoinButton.onClick.AddListener(OnClickCurrentCoinButton);
        exitBtn.onClick.AddListener(OnClickExitBtn);

        customCharacterUI = Helper.GetComponent_Helper<CustomCharacterUI>(customCharacterPanel);
        customCharacterUI.Init();

        remainCoinUI = Helper.GetComponent_Helper<RemainCoinUI>(currentCoinPanel);
        remainCoinUI.Init();
    }
    
    protected override UIState GetUIState()
    {
        return UIState.World;
    }

    public void OnClickCharacterCustomButton()
    {
        if(!customCharacterPanel.activeInHierarchy)
            customCharacterPanel.SetActive(true);
        else customCharacterPanel.SetActive(false);
    }

    public void OnClickCurrentCoinButton()
    {
        if(!currentCoinPanel.activeInHierarchy) 
            currentCoinPanel.SetActive(true);
        else currentCoinPanel.SetActive(false);
    }

    public void UpdateCoinUI()
    {
        remainCoinUI.UpdateCoinUI();
    }

    public void OnClickExitBtn()
    {
        Application.Quit();
    }
}
