public class Consumer : IConsumer
{
    private readonly ISystemMessageSource queue;
    private readonly IAction processAction;
    private readonly ISystemMessageListener listener;
    private bool consuming;

    public Consumer(ISystemMessageSource queue, IAction processAction, ISystemMessageListener listener, bool startConsuming)
    {
        this.queue = queue;
        this.processAction = processAction;
        this.listener = listener;

        if (startConsuming)
        {
            this.Start();
        }
    }

    public ISystemMessageSource Source { get; }

    public void ProcessMessage(string message)
    {

    }

    public bool Start()
    {
        this.consuming = true;
        return consuming;
    }

    public bool Stop()
    {
        this.consuming = false;
        return this.consuming;
    }
}
