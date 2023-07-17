public interface IQueueConfiguration
{
    string QueueName { get; }
}

public interface ISystemMessageChannelConfiguration
{
    string Name { get; }
    int Size { get; }
    IAction MessageProcessor { get; }
}