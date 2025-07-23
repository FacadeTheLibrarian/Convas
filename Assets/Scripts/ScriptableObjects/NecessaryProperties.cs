using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NecessaryProperties", menuName = "ScriptableObjects/NecessaryProperties")]
internal sealed class NecessaryProperties : ScriptableObject {
    
    //NOTE: maxは番兵のために未指定
    public enum e_properties
    {
        GeminiApiKey = 0,
        YahooAppId = 1,
        max,
    }

    [SerializeField] private List<string> _properties = new List<string>((int)e_properties.max);

    public string this[int index]
    {
        get => _properties[index];
        set         {
            if (index < 0 || _properties.Count <= index)
            {
                Debug.LogError($"Index {index} is out of range for NecessaryProperties.");
                return;
            }
            _properties[index] = value;
        }
    }
}