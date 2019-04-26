namespace Cassandra.Metrics
{
    public enum DriverRequestType
    {
        Batch,
        Execute,
        Query,
        AuthResponse,
        Credentials,
        Options,
        Prepare,
        RegisterForEvent,
        Startup,
    }

    public static class DriverRequestTypeExtensions
    {
        public static bool IsCqlRequest(this DriverRequestType requestType)
        {
            return requestType == DriverRequestType.Batch || requestType == DriverRequestType.Execute || requestType == DriverRequestType.Query;
        }
    }
}