using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

internal abstract class BaseVoiceSynthesisClient : IDisposable {
    public abstract UniTask<AudioClip> GetVoiceClipAsync(string text, CancellationToken token);
    public abstract UniTask<bool> ServerPingAsync(CancellationToken token);
    public abstract void Dispose();
}
