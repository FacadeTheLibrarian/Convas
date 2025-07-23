internal interface IWrappable<T> where T : class {
    public T GetObject();
    public bool IsValid();
}
