using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "CharacterTable", menuName ="Scriptable Objects/Character Table")]
public class CharacterTable : ScriptableObject
{
    [FormerlySerializedAs("chracters")] [SerializeField] private List<CharacterData> characters = new List<CharacterData>();
    private Dictionary<int, CharacterData> characterDictionary = new();

    private void OnEnable()
    {
        foreach (CharacterData m in characters)
        {
            characterDictionary[m.id] = m;
        }
    }

    public CharacterData GetCharacterDataById(int _id)
    {
        if (_id < 0) return null;
        if (characterDictionary.TryGetValue(_id, out CharacterData m)) return m;

        Debug.Log($"Invalid Member Id! : {_id}");
        return null;
    }

    public int GetCharacterDataCount() { return characters.Count; }
}
