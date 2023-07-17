using Microsoft.AspNetCore.DataProtection.KeyManagement;

public class ShortPollListener : ISystemMessageListener
{
    private Dictionary<string, CancellationTokenSource> _listeningTasks;

    private IList<ISystemMessageSource> sources = new List<ISystemMessageSource>();

    private Dictionary<ISystemMessageSource, IList<IConsumer>> subscriptions;

    public ShortPollListener(IList<ISystemMessageSource> sources)
    {
        foreach(var source in sources) { 
            this.subscriptions.Add(source,new List<IConsumer>());
        }
    }

    public bool RegisterConsumer(IConsumer consumer, ISystemMessageSource source, string key)
    {
        if (!sources.Contains(source))
        {
            throw new ArgumentException($"Listening task with key {key} already exists");
        }
        if (_listeningTasks.ContainsKey(key))
        {
            throw new ArgumentException($"Listening task with key {key} already exists");
        }

        var cancellationTokenSource = new CancellationTokenSource();
        _listeningTasks[key] = cancellationTokenSource;
        var cancellationToken = cancellationTokenSource.Token;

        Task.Run(async () =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var message = consumer..Execute();
                if (message != null)
                {
                    consumer.
                    ProcessMessage(message);
                }

                await Task.Delay(_pollingInterval, cancellationToken);
            }

        }, cancellationToken);
        return true;
    }

    public bool UnregisterConsumer(IConsumer consumer)
    {
        consumers.Remove(consumer);
        return true;
    }

    public bool Start(IConsumer consumer)
    {
        throw new NotImplementedException();
    }

    public bool Stop()
    {
        throw new NotImplementedException();
    }

    private Task ShortPoll(IConsumer consumer)
    {

    }
}
public class MessageListener
{
    private readonly SystemMessageDequeue _dequeue;
    private readonly TimeSpan _pollingInterval;
    private Dictionary<string, CancellationTokenSource> _listeningTasks;

    public MessageListener(SystemMessageDequeue dequeue, TimeSpan pollingInterval)
    {
        _dequeue = dequeue;
        _pollingInterval = pollingInterval;
        _listeningTasks = new Dictionary<string, CancellationTokenSource>();
    }

    public void StartListening(string key)
    {
        if (_listeningTasks.ContainsKey(key))
        {
            throw new ArgumentException($"Listening task with key {key} already exists");
        }

        var cancellationTokenSource = new CancellationTokenSource();
        _listeningTasks[key] = cancellationTokenSource;
        var cancellationToken = cancellationTokenSource.Token;

        Task.Run(async () =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var message = _dequeue.Dequeue();
                if (message != null)
                {
                    ProcessMessage(message);
                }

                await Task.Delay(_pollingInterval, cancellationToken);
            }

        }, cancellationToken);
    }

    public void StopListening(string key)
    {
        if (_listeningTasks.TryGetValue(key, out var cancellationTokenSource))
        {
            cancellationTokenSource.Cancel();
            _listeningTasks.Remove(key);
        }
        else
        {
            throw new KeyNotFoundException($"No listening task found with key {key}");
        }
    }

    private void ProcessMessage(SystemMessage message)
    {
        // Process the message here
    }
}
