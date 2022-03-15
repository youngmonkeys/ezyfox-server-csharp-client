using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.socket
{
    public abstract class EzySocketWriter : EzySocketAdapter
	{
		protected EzyPacketQueue packetQueue;

        protected override void update()
        {
            while(true)
            {
                if (!active)
                    return;
                EzyPacket packet = packetQueue.take();
                if (packet == null)
                    return;
                int writtenBytes = writeToSocket(packet);
                
                packet.release();
                if (writtenBytes <= 0)
                    return;

                networkStatistics.getSocketStats().getNetworkStats().addWrittenPackets(1);
                networkStatistics.getSocketStats().getNetworkStats().addWrittenBytes(writtenBytes);
            }
        }

        protected abstract int writeToSocket(EzyPacket packet);

        public void setPacketQueue(EzyPacketQueue packetQueue) 
        {
            this.packetQueue = packetQueue;    
        }

        protected override string getThreadName()
        {
            return "ezyfox-socket-writer";
        }
	}
}
