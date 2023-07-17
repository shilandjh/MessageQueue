


public class SystemMessage
{
    public ISystemMessageInformation SystemMessageInformation { get; set; }
    public string Message { get; set; }
    public string UniqueId { get; set; }
    public int VersionNumber { get; set; }
    public IClusterInformation ClusterInformation { get; set; }
}
