namespace com.tvd12.ezyfoxserver.client.statistics
{
    public class EzySimpleNetworkStats : EzyNetworkStats
    {
        protected long readBytes;
        protected long writtenBytes;
        protected long readPackets;
        protected long writtenPackets;

        public void addReadBytes(long bytes)
        {
            this.readBytes += bytes;
        }

        public void addWrittenBytes(long bytes)
        {
            this.writtenBytes += bytes;
        }

        public void addReadPackets(long packets)
        {
            this.readPackets += packets;
        }

        public void addWrittenPackets(long packets)
        {
            this.writtenPackets += packets;
        }

        public long getReadBytes()
        {
            return this.readBytes;
        }

        public long getWrittenBytes()
        {
            return this.writtenBytes;
        }

        public long getReadPackets()
        {
            return this.readPackets;
        }

        public long getWrittenPackets()
        {
            return this.writtenPackets;
        }
    }
}
