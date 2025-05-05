using UnityEngine;

public class GlobalGameManager : MonoBehaviour
{
    // Fields
    [SerializeField] private CharacterTable characterTable;

    // Keys for PlayerPrefs
    public const string CoinKey = "Current_Coin";
    public const string ScoreKey = "Max_Score";
    public const string SkinKey = "Skin_Index";
    public const string MainSceneName = "Main";
    public const string MiniGameName_1 = "FlyingHeros";

    // Attributes
    public Vector2 WorldSpawnPosition = new(0f, -0.5f);
    public bool IsFirstLoadingInWorld = true;
    public bool IsGameNormallyEnded = false;
    public int SelectedSkinIndex { get; private set; } = 0;

    // Properties
    public int CurrentCoin { get; private set; } = 0;
    public int GainedCoin { get; private set; } = 0;
    public int MaxScore { get; private set; } = 0;
    public int GainedScore { get; private set; } = 0;
    
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

        MaxScore = PlayerPrefs.GetInt(ScoreKey, 0);
        CurrentCoin = PlayerPrefs.GetInt(CoinKey, 0);
        SelectedSkinIndex = PlayerPrefs.GetInt(SkinKey, 0);
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

    /// <summary>
    /// Resets gained coin and score when reward has given to player
    /// This method triggers when player moved scene like this ( MiniGame -> World )
    /// </summary>
    public void ResetGainedCoinNScore() { GainedCoin = 0; GainedScore = 0; }

    /// <summary>
    /// Updates earned score from mini game and max score of minigame
    /// </summary>
    /// <param name="score"></param>
    public void UpdateGameScore(int score)
    {
        if(score > MaxScore) { MaxScore = score; PlayerPrefs.SetInt(ScoreKey, MaxScore); }
        GainedScore = score;
    }

    /// <summary>
    /// Updates currently owned coin
    /// </summary>
    /// <param name="coin"></param>
    /// <param name="isIncrease"></param>
    public void UpdateCurrentCoin(int coin, bool isIncrease)
    {
        GainedCoin += (isIncrease) ? coin : 0;
        CurrentCoin += (isIncrease) ? coin : -coin;
        PlayerPrefs.SetInt(CoinKey, CurrentCoin);
    }

    /// <summary>
    /// Updates currently applied skin index
    /// </summary>
    /// <param name="index"></param>
    public void UpdateSkinIndex(int index)
    {
        SelectedSkinIndex = index;
        PlayerPrefs.SetInt(SkinKey, SelectedSkinIndex);
    }
}
