using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

internal abstract class BaseAIChatBot : IDisposable {
    public BaseAIChatBot() { }
    public abstract UniTask<string> Ask(string prompt, CancellationToken token);
    public abstract void Dispose();
}
