using System;
using System.Threading;
using com.tvd12.ezyfoxserver.client.util;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public abstract class EzySocketReader : EzySocketAdapter
	{
		protected Object decoder;
		protected EzyQueue<EzySocketDataEvent> dataEventQueue;
		protected EzyQueue<EzySocketStatusEvent> statusEventQueue;

		protected override string getThreadName()
		{
			return "socket-reader";
		}

		protected override void process()
		{
			while (active)
			{
				Thread.Sleep(getSleepTime());
				readSocketData();
			}
		}

		protected int getSleepTime()
		{
			return 3;
		}

		protected abstract void readSocketData();

		public abstract void setDecoder(Object decoder);

		public void setDataEventQueue(EzyQueue<EzySocketDataEvent> dataEventQueue)
		{
			this.dataEventQueue = dataEventQueue;
		}

		public void setStatusEventQueue(EzyQueue<EzySocketStatusEvent> statusEventQueue)
		{
			this.statusEventQueue = statusEventQueue;
		}

	}
}
