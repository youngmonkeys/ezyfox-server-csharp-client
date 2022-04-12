namespace com.tvd12.ezyfoxserver.client.statistics
{
    public interface EzyNetworkPacketsStats : EzyNetworkRoPacketsStats
    {
        void addReadPackets(long var1);

        void addWrittenPackets(long var1);
    }
}
