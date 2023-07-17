using Microsoft.AspNetCore.Mvc.ActionConstraints;

public interface ISystemMessageInformation
{
}

public interface ISerialize
{
    string Serialize(object obj);
    T Deserialize<T>(string str);
}

public interface IDataSource
{
    Stream OpenRead();
    Stream OpenWrite();
}

public interface IConsumer
{
    bool Start();

    bool Stop();
    ISystemMessageSource Source { get; }

    void ProcessMessage(string message);
}

public interface IAction
{
    void Execute();
}

public interface ISystemMessageSource
{

}
