using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal abstract class BaseAnimationHandler : IDisposable {

    public BaseAnimationHandler() { }
    public abstract void OnCompleteInitialization();
    public abstract void OnIdle();
    public abstract void OnListening();
    public abstract void OnStartPreparation();
    public abstract void OnCompletePreparation();
    public abstract void OnGreeting();
    public abstract void OnReading();
    public abstract void Dispose();
}
