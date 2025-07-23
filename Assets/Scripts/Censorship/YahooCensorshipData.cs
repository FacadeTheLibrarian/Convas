using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YahooCensorshipData
{
    private readonly string ID = default;
    private readonly List<string> HIRAGANA_TEXTS = default;

    public string GetId { get { return ID; } }
    public List<string> GetHiraganaTexts { get { return HIRAGANA_TEXTS; } }
    public YahooCensorshipData(string id, in List<string> hiraganaTexts) {
        ID = id;
        HIRAGANA_TEXTS = hiraganaTexts;
    }
}
