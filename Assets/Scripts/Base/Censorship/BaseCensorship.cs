using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

internal abstract class BaseCensorship
{
    public enum e_censorshipResult {
        OK = 0,
        NG = 1,
        ERROR = 2,
    }
    protected IBadWords _badWords = default;
    public BaseCensorship(IBadWords badWords) {
        _badWords = badWords;
    }
    public abstract UniTask<e_censorshipResult> IsBadWordIncluded(string input, CancellationToken token);
}
