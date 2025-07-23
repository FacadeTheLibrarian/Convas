using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

internal abstract class BaseChatSystem : IDisposable {
    public event Action<string> OnAnswerReady = default;
    protected IBadWords _badWords = default;

    public BaseChatSystem(IBadWords badWords) {
        _badWords = badWords;
    }
    public abstract UniTask<string> BuildAnswerAsync(string input, CancellationToken token);
    protected void PublishAnswerReady(string text) {
        OnAnswerReady?.Invoke(text);
    }
    public abstract void Dispose();
}
