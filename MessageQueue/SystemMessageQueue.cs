

using MessageQueue;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

public interface ISystemMessageQueue
{
    string Read();

    string Name { get; set; }
}

public class QueueManager
{
    private static QueueManager _queueManager;

    private Dictionary<string, QueueTask?> _queues;
    private int _pollingInterval;

    private class QueueTask
    {
        public Task Task { get; }
        public CancellationTokenSource CancellationTokenSource { get; }

        public QueueTask(Task task, CancellationTokenSource cancellationTokenSource)
        {
            Task = task;
            CancellationTokenSource = cancellationTokenSource;
        }
        public void Cancel()
        {
            if (!CancellationTokenSource.IsCancellationRequested)
            {
                CancellationTokenSource.Cancel();
            }

            CancellationTokenSource.Dispose();
        }
    }

    private QueueManager()
    {

    }

    public static QueueManager GetInstance()
    {
        if(_queueManager == null)
        {
            _queueManager= new QueueManager();
        }

        return _queueManager;
    }

    public void RegisterQueue(string queueName)
    {
        if(GetQueue(queueName).Equals(default(KeyValuePair<string, QueueTask>)))
        {
            _queues.Add(queueName, null);
        }
    }

    public void UnregisterQueue(string queueName)
    {
        if (GetQueue(queueName).Equals(default(KeyValuePair<string, QueueTask>)))
        {
            _queues.Remove(queueName);
        }
    }

    public void StartQueue(string queueName, ISystemMessageQueue queue)
    {
        var registeredQueue = GetQueue(queueName);

        if (registeredQueue.Equals(default(KeyValuePair<string, QueueTask>)))
        {
            // Not found
            return;
        }

        var queueTask = registeredQueue.Value as QueueTask;

        if((queueTask?.Task?.Status ?? TaskStatus.Canceled) == TaskStatus.Running)
        {
            // queue is running
            return;
        }

        _queues[queueName] = CreateConsumptionTask(queue);

        // queue started
        return;
    }

    public void StopQueue(string queueName)
    {
        var registeredQueueTask = GetQueue(queueName);

        if (registeredQueueTask.Equals(default(KeyValuePair<string, QueueTask>)))
        {
            // Not found
            return;
        }

        var queueTask = registeredQueueTask.Value;

        queueTask.Cancel();

        _queues[queueName] = null;

        // 
        return;
    }

    private KeyValuePair<string, QueueTask> GetQueue(string queueName) =>
        _queues.TryGetValue(queueName, out var queueTask)
        ? new KeyValuePair<string, QueueTask>(queueName, queueTask)
        : default(KeyValuePair<string, QueueTask>);

    private QueueTask CreateConsumptionTask(ISystemMessageQueue queue)
    {
        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        var task = Task.Run(async () =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var message = queue.Read();

                await Task.Delay(_pollingInterval, cancellationToken);
            }

        }, cancellationToken);

        return new QueueTask(task, cancellationTokenSource);
    }
}

public class SystemMessageQueue : IReader, IWriter
{
    private readonly IDataSource _dataSource;
    private readonly ISerialize _serializer;
    private readonly IQueueConfiguration queueConfiguration;
    private readonly QueueManager queueManager;

    public SystemMessageQueue(IDataSource dataSource, ISerialize serializer,ISystemMessageListener listener,
        bool startConsuming, IQueueConfiguration queueConfiguration)
    {
        _dataSource = dataSource;
        _serializer = serializer;
        this.queueConfiguration = queueConfiguration;
        queueManager = QueueManager.GetInstance();
        queueManager.RegisterQueue(queueConfiguration.QueueName);

        if (startConsuming)
        {
            this.Start();
        }
    }

    public void Start()
    {

    }

    public void Stop()
    {

    }

    public void Read()
    {

    }

    public void Write() { }

    public void Enqueue(SystemMessage systemMessage, string streamName)
    {
        var serializedMessage = _serializer.Serialize(systemMessage);
        using var stream = _dataSource.OpenWrite();
        using var writer = new StreamWriter(stream);
        writer.Write(serializedMessage);
    }

    public SystemMessage Dequeue()
    {
        using var stream = _dataSource.OpenRead();
        using var reader = new StreamReader(stream);
        var serializedMessage = reader.ReadToEnd();

        return _serializer.Deserialize<SystemMessage>(serializedMessage);
    }

    public void Acknowledge()
    {
        // Implement acknowledgment logic here
    }
}
