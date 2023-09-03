using System;

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
                {
                    return;
                }
                EzyPacket packet = packetQueue.take();
                if (packet == null)
                {
                    return;
                }
                int writtenBytes = writeToSocket(packet);
                
                packet.release();
                if (writtenBytes <= 0)
                {
                    return;
                }
                addSocketWriteStats(writtenBytes);
            }
        }

        public override bool call()
        {
            try
            {
                if (!active)
                {
                    return false;
                }
                EzyPacket packet = packetQueue.take();
                if (packet == null)
                {
                    return true;
                }
                int writtenBytes = writeToSocket(packet);

                packet.release();
                if (writtenBytes <= 0)
                {
                    return false;
                }

                addSocketWriteStats(writtenBytes);
                return true;
            }
            catch (Exception e)
            {
                logger.info("problems in socket-writer event loop", e);
                return false;
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
