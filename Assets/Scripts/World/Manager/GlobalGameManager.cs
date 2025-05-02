using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class GlobalGameManager : MonoBehaviour
{
    // Fields
    [SerializeField] private CharacterTable characterTable;

    // Keys for PlayerPrefs
    private const string CoinKey = "Current_Coin";
    private const string ScoreKey = "Max_Score";
    private const string DodgeGameName = "SurviveTheChaos";

    // Attributes
    public Vector2 WorldSpawnPosition = new Vector2(0f, -0.5f);
    public bool IsFirstLoadingInWorld = true;
    public bool IsFirstLoadingInGame = true;
    public int SelectedSkinIndex = 0;
    public int CurrentScene = 0;

    // Properties
    public int CurrentCoin { get; private set; } = 0;
    public int MaxScore { get; private set; } = 0;
    
    // Singleton
    private static GlobalGameManager instance;
    public static GlobalGameManager Instance
    {
        get
        {
            if (instance == null)
                instance = Helper.GetComponent_Helper<GlobalGameManager>(GameObject.FindWithTag(nameof(GlobalGameManager)));
            return instance;
        }
    }

    /// <summary>
    /// Awake is called once when scripts being loaded
    /// </summary>
    private void Awake()
    {
        if (Instance != this) Destroy(gameObject);
        else DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Returns registered character data count
    /// </summary>
    /// <returns>Returns integer value of character data count</returns>
    public int GetCharacterDataCount() { return characterTable.GetCharacterDataCount(); }

    /// <summary>
    /// Returns current character data
    /// </summary>
    /// <returns>Returns CharacterData currently assigned</returns>
    public CharacterData GetCurrentCharacterData() { return characterTable.GetCharacterDataById(SelectedSkinIndex); }

    /// <summary>
    /// Returns single character data by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Returns Character data by id. If character data not found, returns null value.</returns>
    public CharacterData GetCharacterDataById(int id) { return characterTable.GetCharacterDataById(id); }
}
