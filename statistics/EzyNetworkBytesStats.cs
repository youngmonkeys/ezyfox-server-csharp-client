namespace com.tvd12.ezyfoxserver.client.statistics
{
    public interface EzyNetworkBytesStats : EzyNetworkRoBytesStats
    {
        void addReadBytes(long bytes);

        void addWrittenBytes(long bytes);
    }
}
