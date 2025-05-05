using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomCharacterUI : MonoBehaviour
{
    [SerializeField] private Button prevBtn;
    [SerializeField] private Button nextBtn;
    [SerializeField] private Button applyBtn;
    [SerializeField] private Button cancelBtn;
    [SerializeField] private Image spriteImage;
    private CharacterData currentCharacterData;
    private GlobalGameManager globalGameManager;
    private WorldManager worldManager;
    private int currentCharacterIndex;


    public void Init()
    {
        worldManager = WorldManager.Instance;
        globalGameManager = GlobalGameManager.Instance;

        currentCharacterIndex = globalGameManager.SelectedSkinIndex;
        currentCharacterData = globalGameManager.GetCharacterDataById(currentCharacterIndex);
        spriteImage.sprite = currentCharacterData.characterSprite;

        prevBtn.onClick.AddListener(MoveToPrevCharacter);
        nextBtn.onClick.AddListener(MoveToNextCharacter);
        applyBtn.onClick.AddListener(ApplySelectedCharacter);
        cancelBtn.onClick.AddListener(CancelCharacterSelection);
    }

    public void MoveToNextCharacter()
    {
        currentCharacterData = globalGameManager.GetCharacterDataById(++currentCharacterIndex);
        if(currentCharacterData == null)
        {
            currentCharacterIndex = 0;
            currentCharacterData = globalGameManager.GetCharacterDataById(currentCharacterIndex);
        }
        spriteImage.sprite = currentCharacterData.characterSprite;
    }

    public void MoveToPrevCharacter()
    {
        currentCharacterData = globalGameManager.GetCharacterDataById(--currentCharacterIndex);
        if (currentCharacterData == null)
        {
            currentCharacterIndex = globalGameManager.GetCharacterDataCount() - 1;
            currentCharacterData = globalGameManager.GetCharacterDataById(currentCharacterIndex);
        }
        spriteImage.sprite = currentCharacterData.characterSprite;
    }

    public void ApplySelectedCharacter()
    {
        globalGameManager.UpdateSkinIndex(currentCharacterIndex);
        worldManager.PlayerController.ChangeCharacter(currentCharacterData);
        gameObject.SetActive(false);
    }

    public void CancelCharacterSelection()
    {
        gameObject.SetActive(false);
    }
}
