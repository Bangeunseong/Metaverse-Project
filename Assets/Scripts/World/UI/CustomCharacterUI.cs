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
    private int currentCharacterIndex;

    public void Init()
    {
        currentCharacterIndex = GlobalGameManager.Instance.SelectedSkinIndex;
        currentCharacterData = GlobalGameManager.Instance.GetCharacterDataById(currentCharacterIndex);
        spriteImage.sprite = currentCharacterData.characterSprite;

        prevBtn.onClick.AddListener(MoveToPrevCharacter);
        nextBtn.onClick.AddListener(MoveToNextCharacter);
        applyBtn.onClick.AddListener(ApplySelectedCharacter);
        cancelBtn.onClick.AddListener(CancelCharacterSelection);
    }

    public void MoveToNextCharacter()
    {
        currentCharacterData = GlobalGameManager.Instance.GetCharacterDataById(++currentCharacterIndex);
        if(currentCharacterData == null)
        {
            currentCharacterIndex = 0;
            currentCharacterData = GlobalGameManager.Instance.GetCharacterDataById(currentCharacterIndex);
        }
        spriteImage.sprite = currentCharacterData.characterSprite;
    }

    public void MoveToPrevCharacter()
    {
        currentCharacterData = GlobalGameManager.Instance.GetCharacterDataById(--currentCharacterIndex);
        if (currentCharacterData == null)
        {
            currentCharacterIndex = GlobalGameManager.Instance.GetCharacterDataCount() - 1;
            currentCharacterData = GlobalGameManager.Instance.GetCharacterDataById(currentCharacterIndex);
        }
        spriteImage.sprite = currentCharacterData.characterSprite;
    }

    public void ApplySelectedCharacter()
    {
        GlobalGameManager.Instance.SelectedSkinIndex = currentCharacterIndex;
        WorldManager.Instance.PlayerController.ChangeCharacter(currentCharacterData);
        gameObject.SetActive(false);
    }

    public void CancelCharacterSelection()
    {
        gameObject.SetActive(false);
    }
}
