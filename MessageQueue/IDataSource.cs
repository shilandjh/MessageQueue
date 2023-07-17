namespace MessageQueue;

public interface IDataSource
{
    Stream OpenRead();
    Stream OpenWrite();
}

