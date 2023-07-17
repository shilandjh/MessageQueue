



public class SystemMessageProcessor : ISystemMessageProcessor
{
    private readonly SystemMessageQueue _queue;

    public SystemMessageProcessor(SystemMessageQueue queue)
    {
        _queue = queue;
    }

    public void OnNewMessage()
    {
        var message = _queue.Dequeue();
        // Process the message here
    }
}
