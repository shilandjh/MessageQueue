public interface ISystemMessageListener
{
    bool Start(IConsumer consumer);

    bool Stop(IConsumer consumer);
}