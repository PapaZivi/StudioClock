using System.IO.Pipes;

namespace StudioClock.Services;

public sealed class SingleInstanceService : IDisposable
{
    private readonly string _name;
    private readonly Mutex _mutex;
    private CancellationTokenSource? _cts;
    public bool IsPrimary { get; }
    public event Action? ActivationRequested;

    public SingleInstanceService(string name)
    {
        _name = name + "-" + Environment.UserName;
        _mutex = new Mutex(true, _name, out var created);
        IsPrimary = created;
    }

    public void StartListening()
    {
        if (!IsPrimary) return;
        _cts = new CancellationTokenSource();
        _ = ListenAsync(_cts.Token);
    }

    private async Task ListenAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            try
            {
                await using var server = new NamedPipeServerStream(_name, PipeDirection.In, 1,
                    PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
                await server.WaitForConnectionAsync(token);
                ActivationRequested?.Invoke();
            }
            catch (OperationCanceledException) { break; }
            catch { await Task.Delay(250, token).ConfigureAwait(false); }
        }
    }

    public void NotifyPrimary()
    {
        try
        {
            using var client = new NamedPipeClientStream(".", _name, PipeDirection.Out);
            client.Connect(1500);
            client.WriteByte(1);
        }
        catch { }
    }

    public void Dispose()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        if (IsPrimary) _mutex.ReleaseMutex();
        _mutex.Dispose();
    }
}

