using System;
using Cysharp.Threading.Tasks;

internal abstract class BaseDictation : IDisposable {

    public event Action<string> OnHypothesisUpdated = default;
    public event Action<string> OnComplete = default;
    public event Action<string> OnError = default;

    public BaseDictation() { }

    public abstract void StartListening();
    public abstract void ForceStopListening();

    protected void PublishOnHypothesisUpdated(string text) {
        OnHypothesisUpdated?.Invoke(text);
    }
    protected void PublishOnComplete(string text) {
        OnComplete?.Invoke(text);
    }
    protected void PublishOnError(string text) {
        OnError?.Invoke(text);
    }

    public abstract void Dispose();
}
