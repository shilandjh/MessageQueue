namespace MessageQueue;

public class DataSource : IDataSource
{
    // Implement your datasource specific read and write streams
    public Stream OpenRead()
    {
        // Return your datasource specific read stream
    }

    public Stream OpenWrite()
    {
        // Return your datasource specific write stream
    }
}

