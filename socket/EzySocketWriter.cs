using System;
using System.Threading;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public class EzySocketWriter : EzyAbstractSocketEventHandler
	{
		protected readonly EzyPacketQueue packetQueue;
		protected readonly EzySocketDataHandler dataHandler;

		public EzySocketWriter(EzyPacketQueue packetQueue,
							   EzySocketDataHandler dataHandler)
		{
			this.packetQueue = packetQueue;
			this.dataHandler = dataHandler;
		}

		public override void handleEvent()
		{
			try
			{
				EzyPacket packet = packetQueue.take();
				dataHandler.firePacketSend(packet);
				packet.release();
			}
			catch (Exception e)
			{
				Console.WriteLine("socket-writer thread interrupted: " + Thread.CurrentThread + " error: " + e);
			}
		}

		public override void destroy()
		{
			packetQueue.clear();
		}

		public override void reset()
		{
		}

	}
}
