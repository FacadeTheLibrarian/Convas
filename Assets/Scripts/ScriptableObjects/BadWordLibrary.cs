using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BadWordLibrary", menuName = "ScriptableObjects/BadWordLibrary")]
internal sealed class BadWordLibrary : ScriptableObject, IBadWords
{
    [SerializeField] private List<string> _badWords = new List<string>();
    public List<string> GetBadWords() {
        return _badWords;
    }
}
