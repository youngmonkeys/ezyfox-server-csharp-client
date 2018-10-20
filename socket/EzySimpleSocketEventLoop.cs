using System;
using System.Threading;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public class EzySimpleSocketEventLoop : EzySocketEventLoop
	{
		protected int threadListSize = 1;
		protected String threadName;
		protected Action action;

		protected override sealed void eventLoop()
		{
            logger.info(currentThreadName() + " event loop has started");
            while (active)
            {
                action();
            }
            logger.info(currentThreadName() + " event loop has stopped");
		}

		public void setThreadListSize(int threadListSize)
		{
			this.threadListSize = threadListSize;
		}

		public void setThreadName(String threadName)
		{
			this.threadName = threadName;
		}

		public void setAction(Action action)
		{
			this.action = action;
		}

		protected override String getThreadName()
		{
			return threadName;
		}

		protected override int getThreadListSize()
		{
			return threadListSize;
		}

		private String currentThreadName()
		{
			return Thread.CurrentThread.Name;
		}
	}
}
