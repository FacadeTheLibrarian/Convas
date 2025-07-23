using System;

internal abstract class ExternalObjectWrapper<T> where T : class, IWrappable<T>, IDisposable {
    public T GetObject() => _externalObject.GetObject();
    public abstract bool IsValid();

    private T _externalObject = default(T);
    public ExternalObjectWrapper(T externalObject) {
        _externalObject = externalObject ?? throw new ArgumentNullException(nameof(externalObject), "External object cannot be null");
    }
    public void Dispose() {
        _externalObject.Dispose();
    }
}