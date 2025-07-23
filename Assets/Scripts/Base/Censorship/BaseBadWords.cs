using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal abstract class BaseBadWords
{
    public BaseBadWords() { }
    public abstract List<string> GetBadWords();
}
