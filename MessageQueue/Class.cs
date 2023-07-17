namespace MessageQueue;

public interface IQueueMessage
{
    string Message { get; }
}

public class QueueMessage : IQueueMessage
{
    public string Message { get; private set; }

    public QueueMessage(string message)
    {
        Message = message;
    }
}

public interface IMessageQueue
{
    void Enqueue(IQueueMessage message);
    IQueueMessage Dequeue();
}

public class MessageWriter
{
    private readonly IDataSource _dataSource;

    public MessageWriter(IDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public void Write(IQueueMessage message)
    {
        using var stream = _dataSource.OpenWrite();
        using var writer = new StreamWriter(stream);

        writer.Write(message.Message);
    }
}

public class MessageReader
{
    private readonly IDataSource _dataSource;

    public MessageReader(IDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public IQueueMessage Read()
    {
        using var stream = _dataSource.OpenRead();
        using var reader = new StreamReader(stream);

        var message = reader.ReadToEnd();
        return new QueueMessage(message);
    }
}

