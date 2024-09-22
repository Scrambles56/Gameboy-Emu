using System.Diagnostics;

public class Stopwatcher : IDisposable
{
    private Stopwatch _sw;
    private Action<TimeSpan> _callback;

    public Stopwatcher(Action<TimeSpan> callback)
    {
        _callback = callback;
        _sw = Stopwatch.StartNew();
    }
    
    
    public void Dispose()
    {
        _sw.Stop();
        _callback(_sw.Elapsed);
    }
}