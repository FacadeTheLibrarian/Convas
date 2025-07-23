using System;

internal abstract class Audio<T> : IWrappable<T>, IDisposable where T : class {
    protected T _audio = default;
    public Audio(T audio) {
        _audio = audio ?? throw new ArgumentNullException(nameof(audio), "Audio object cannot be null");
    }
    public T GetObject() => _audio;
    public abstract bool IsValid();
    public void Dispose() {
        OnDispose();
    }
    protected abstract void OnDispose();
}
