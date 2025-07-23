using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

internal abstract class BaseVoiceSynthesisServer : IDisposable {
    public abstract UniTask AwakeServerAsync(CancellationToken token);
    public abstract void Dispose();
}
