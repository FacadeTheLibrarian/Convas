using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal sealed class BasicBadWords : BaseBadWords
{
    private List<string> _badWords = new List<string>
    {
        "NG",
        "えぬ",
        "エヌジー"
    };
    public BasicBadWords() : base() { }
    public override List<string> GetBadWords() => _badWords;
}
