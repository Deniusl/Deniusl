using System;

public class LoadingObject : IDisposable
{
    public bool IsLoading { get; private set; }

    private readonly Action _loadingComplete;

    
    public LoadingObject(Action loadingStart = null, Action loadingComplete = null)
    {
        IsLoading = true;
        loadingStart?.Invoke();
        _loadingComplete = loadingComplete;
    }
    
    public LoadingObject(Action loadingComplete = null)
    {
        IsLoading = true;
        _loadingComplete = loadingComplete;
    }
    
    public void Dispose()
    {
        IsLoading = false;
        _loadingComplete?.Invoke();
    }
}