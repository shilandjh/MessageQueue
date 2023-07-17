namespace MessageQueue.Unused;

public class MessageQueue : IMessageQueue
{
    private readonly Queue<IQueueMessage> _queue = new Queue<IQueueMessage>();

    public void Enqueue(IQueueMessage message)
    {
        _queue.Enqueue(message);
    }

    public IQueueMessage Dequeue()
    {
        return _queue.Dequeue();
    }
}

