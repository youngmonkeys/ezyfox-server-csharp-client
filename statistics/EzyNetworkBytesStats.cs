namespace com.tvd12.ezyfoxserver.client.statistics
{
    public interface EzyNetworkBytesStats : EzyNetworkRoBytesStats
    {
        void addReadBytes(long var1);

        void addWrittenBytes(long var1);
    }
}