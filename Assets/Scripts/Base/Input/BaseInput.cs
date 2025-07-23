using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal abstract class BaseInput : IDisposable {

    public event Action OnStart = default;
    public event Action OnStop = default;
    public event Action OnRequestExitGame = default;
    public BaseInput() { }
    public abstract void Update();
    protected void PublishStart() {
        OnStart?.Invoke();
    }
    protected void PublishStop() {
        OnStop?.Invoke();
    }
    protected void PublishRequestEndGame() {
        OnRequestExitGame?.Invoke();
    }
    public abstract void Dispose();
}
