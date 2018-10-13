using System;
using com.tvd12.ezyfoxserver.client.socket;
using com.tvd12.ezyfoxserver.client.util;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public abstract class EzySocketEventLoopHandler :
		EzyThreadPoolSizeAware,
		EzyStartable,
		EzyDestroyable,
		EzyResettable
	{
		protected int threadPoolSize = 1;
		protected EzySocketEventLoop eventLoop;

		public void start()
		{
			this.eventLoop = newEventLoop0();
			this.eventLoop.start();
		}

		private EzySimpleSocketEventLoop newEventLoop0()
		{
			EzySimpleSocketEventLoop eventLoop = newEventLoop();
			eventLoop.setThreadName(getThreadName());
			eventLoop.setThreadListSize(threadPoolSize);
			return eventLoop;
		}

		protected abstract String getThreadName();

		protected abstract EzySimpleSocketEventLoop newEventLoop();

		public void setThreadPoolSize(int threadPoolSize)
		{
			this.threadPoolSize = threadPoolSize;
		}

		public void reset()
		{
			eventLoop.reset();
		}

		public void destroy()
		{
			if (eventLoop != null)
			{
				eventLoop.destroy();
			}
		}
	}
}
