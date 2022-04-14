namespace com.tvd12.ezyfoxserver.client.statistics
{
    public interface EzyNetworkPacketsStats : EzyNetworkRoPacketsStats
    {
        void addReadPackets(long bytes);

        void addWrittenPackets(long bytes);
    }
}
