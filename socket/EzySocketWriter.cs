using System;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.concurrent;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public abstract class EzySocketWriter : EzySocketAdapter
	{
		protected EzyBlockingQueue<EzyArray> ticketsQueue;

		protected override void process()
		{
			EzyArray data = ticketsQueue.take();
			getLogger().debug("send: {0}", data);
			Object bytes = encodeData(data);
			writeBytes(bytes);
		}

		protected abstract void writeBytes(Object bytes);
		protected abstract Object encodeData(EzyArray data);

		protected override string getThreadName()
		{
			return "socket-writer";
		}

		public abstract void setEncoder(Object encoder);


		public void setTicketsQueue(EzyBlockingQueue<EzyArray> ticketsQueue)
		{
			this.ticketsQueue = ticketsQueue;
		}
	}
}
