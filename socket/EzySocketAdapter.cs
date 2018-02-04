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

		protected Thread newThread()
		{
            Thread thread = new Thread(run);
			thread.Name = getThreadName();
			return thread;
		}

		public void setActive(bool active)
		{
			this.active = active;
		}

		public void start()
		{
			this.thread = newThread();
			this.thread.Start();
		}

		protected void run()
		{
			this.setActive(true);
			this.run0();
		}

		public virtual void run0()
		{
			while (active)
			{
				this.process();
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
