using System;
using System.Collections.Generic;
using System.Threading;
using com.tvd12.ezyfoxserver.client.concurrent;
using com.tvd12.ezyfoxserver.client.util;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public abstract class EzySocketEventLoop :
        EzyLoggable,
		EzyStartable,
		EzyDestroyable,
		EzyResettable
	{
		protected ThreadList threadList;
		protected volatile bool active;

		protected abstract String getThreadName();
		protected abstract int getThreadListSize();

		public void start()
		{
			initThreadList();
			setActive(true);
			startLoopService();
		}

		protected void setActive(bool value)
		{
			this.active = value;
		}

		private void startLoopService()
		{
			threadList.start();
		}

		protected abstract void eventLoop();

		protected void initThreadList()
		{
			this.threadList = new ThreadList(getThreadListSize(),
											 getThreadName(),
											 eventLoop);
		}

		public void reset()
		{
			active = true;
		}

		public void destroy()
		{
			threadList.destroy();
		}
	}
}
