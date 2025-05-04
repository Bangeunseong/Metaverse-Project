using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterTable", menuName ="Scriptable Objects/Character Table")]
public class CharacterTable : ScriptableObject
{
    [SerializeField] private List<CharacterData> chracters = new List<CharacterData>();
    private Dictionary<int, CharacterData> CharacterDictionary = new();

    private void OnEnable()
    {
        foreach (CharacterData m in chracters)
        {
            CharacterDictionary[m.id] = m;
        }
    }

    public CharacterData GetCharacterDataById(int _id)
    {
        if (_id < 0) return null;
        if (CharacterDictionary.TryGetValue(_id, out CharacterData m)) return m;

        Debug.Log($"Invalid Member Id! : {_id}");
        return null;
    }

    public int GetCharacterDataCount() { return chracters.Count; }
}
