using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

internal abstract class BaseNewsFeed
{
    public event Action<string> OnNewsReady = default;
    public BaseNewsFeed() { }
    public abstract UniTask<string> GetNews(CancellationToken token);
    protected void PublishNewsReady(string news) {
        OnNewsReady?.Invoke(news);
    }
}
