using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace MessageQueue;

public interface IConsumer
{
    void Consume();
}

public interface IChannelConfiguration
{
    string Name { get; }
}

public interface IReader
{
    string Read();
}
public interface IWriter
{
    void Write();
}

public interface IConsumerEngine
{
    IConsumerEngineTask Start(IConsumer consumer);
    bool Stop(IConsumerEngineTask watcherTask);
}

public interface IConsumerEngineTask
{
    void Cancel();
}

public class WatcherTask : IConsumerEngineTask
{
    public WatcherTask(Task watchingTask, CancellationTokenSource cancellationTokenSource)
    {

    }

    public void Cancel()
    {
        throw new NotImplementedException();
    }
}

public interface IChannel : IReader, IWriter
{
    IConsumerEngine Watcher { get; }
    IChannelConfiguration Configuration { get; }
}


public class Consumer : IConsumer
{
    private readonly IAction messageProcessor;
    private readonly IChannel channel;

    public Consumer(IAction messageProcessor, IChannel channel)
    {
        this.messageProcessor = messageProcessor;
        this.channel = channel;
    }

    public void Consume()
    {
        throw new NotImplementedException();
    }

    private void Start()
    {
        channel.Watcher.Start(this);
        throw new NotImplementedException();
    }

    private void Stop()
    {
        throw new NotImplementedException();
    }
}

public class QueueConfiguration : IChannelConfiguration
{
    private QueueManager queueManager;

    public QueueConfiguration(string name)
    {
        Name = name;
        queueManager = QueueManager.GetInstance();
        queueManager.RegisterQueue(name);
    }
    public string Name { get; init; }
}

public class QueueChannel : IChannel
{
    public QueueChannel(IDataSource dataSource, IChannelConfiguration configuration, IConsumerEngine watcher,
        ISerialize serialize)
    {

    }
    public IConsumerEngine Watcher => throw new NotImplementedException();

    public IChannelConfiguration Configuration => throw new NotImplementedException();

    public string Read()
    {
        throw new NotImplementedException();
    }

    public void Write()
    {
        throw new NotImplementedException();
    }
}

public class ShortPollWatcher : IConsumerEngine
{
    private int _pollingInterval;

    public IConsumerEngineTask Start(IConsumer consumer)
    {
        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        var task = new Task(async () =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                consumer.Consume();

                await Task.Delay(_pollingInterval, cancellationToken);
            }

        }, cancellationToken);

        return new WatcherTask(task, cancellationTokenSource);
    }

    public bool Stop(IConsumerEngineTask watcherTask)
    {
        watcherTask.Cancel();

        return true;
    }
}