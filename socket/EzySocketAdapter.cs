using System;
using System.Threading;
using com.tvd12.ezyfoxserver.client.util;
using com.tvd12.ezyfoxserver.client.logger;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public abstract class EzySocketAdapter : EzyLoggable, EzyStartable, EzyStoppable
	{
		protected Thread thread;
		protected volatile bool active = false;

		protected void newThread()
		{
            this.thread = new Thread(run);
			this.thread.Name = getThreadName();
		}

		public void setActive(bool active)
		{
			this.active = active;
		}

		public void start()
		{
			thread.Start();
		}

		protected void run()
		{
			setActive(true);
			run0();
		}

		public virtual void run0()
		{
			while (active)
			{
				process();
			}
		}

		public void stop()
		{
			this.active = false;
		}

		protected abstract void process();
		protected abstract String getThreadName();
	}
}
