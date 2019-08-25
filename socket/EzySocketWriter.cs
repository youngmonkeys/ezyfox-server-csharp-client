using System;
using System.Threading;
using com.tvd12.ezyfoxserver.client.concurrent;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.util;

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
                writeToSocket(packet);
                packet.release();
            }
        }

        protected abstract void writeToSocket(EzyPacket packet);

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
