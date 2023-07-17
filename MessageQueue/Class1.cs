namespace MessageQueue;
// IStreamDataSource represents a data source that supports streaming
public interface IStreamDataSource
{
    Stream OpenRead();
    Stream OpenWrite();
}

public class StreamDataSource : IStreamDataSource
{
    // Implement your data source specific read and write streams
    public Stream OpenRead()
    {
        // Return your data source specific read stream
    }

    public Stream OpenWrite()
    {
        // Return your data source specific write stream
    }
}

// IQueueItem represents a generic item that can be enqueued and dequeued
public interface IQueueItem
{
    string Item { get; }
}

public class QueueItem : IQueueItem
{
    public string Item { get; private set; }

    public QueueItem(string item)
    {
        Item = item;
    }
}

// IItemQueue represents a queue that can enqueue and dequeue IQueueItems
public interface IItemQueue
{
    void Enqueue(IQueueItem item);
    IQueueItem Dequeue();
}

public class ItemQueue : IItemQueue
{
    private readonly Queue<IQueueItem> _queue = new Queue<IQueueItem>();

    public void Enqueue(IQueueItem item)
    {
        _queue.Enqueue(item);
    }

    public IQueueItem Dequeue()
    {
        return _queue.Dequeue();
    }
}

// ItemWriter is responsible for writing IQueueItems to the IStreamDataSource
public class ItemWriter
{
    private readonly IStreamDataSource _dataSource;

    public ItemWriter(IStreamDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public void Write(IQueueItem item)
    {
        using var stream = _dataSource.OpenWrite();
        using var writer = new StreamWriter(stream);

        writer.Write(item.Item);
    }
}

// ItemReader is responsible for reading IQueueItems from the IStreamDataSource
public class ItemReader
{
    private readonly IStreamDataSource _dataSource;

    public ItemReader(IStreamDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public IQueueItem Read()
    {
        using var stream = _dataSource.OpenRead();
        using var reader = new StreamReader(stream);

        var item = reader.ReadToEnd();
        return new QueueItem(item);
    }
}

