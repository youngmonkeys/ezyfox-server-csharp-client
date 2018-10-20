using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.evt;
using com.tvd12.ezyfoxserver.client.handler;
using com.tvd12.ezyfoxserver.client.util;

namespace com.tvd12.ezyfoxserver.client.socket
{
    public class EzyMainThreadQueue : EzyLoggable
	{
		private readonly EzyQueue<EzyEventHandlerExecutor> eventExecutors;
		private readonly EzyQueue<EzyDataHandlerExecutor> dataExecutors;

		public EzyMainThreadQueue()
		{
			this.eventExecutors = new EzyQueue<EzyEventHandlerExecutor>();
			this.dataExecutors = new EzyQueue<EzyDataHandlerExecutor>();
		}

		public void add(EzyEvent evt, EzyEventHandler handler)
		{
			lock (eventExecutors)
			{
				eventExecutors.add(new EzyEventHandlerExecutor(evt, handler));
			}
		}

		public void add(EzyArray data, EzyDataHandler handler)
		{
			lock (dataExecutors)
			{
				dataExecutors.add(new EzyDataHandlerExecutor(data, handler));
			}
		}

		public void polls()
		{
			IList<EzyEventHandlerExecutor> eventExecutors = dequeueEventHandlers();
			foreach (EzyEventHandlerExecutor executor in eventExecutors)
				executor.execute();
			IList<EzyDataHandlerExecutor> dataExecutors = dequeueDataHandlers();
			foreach (EzyDataHandlerExecutor executor in dataExecutors)
				executor.execute();
		}

		private IList<EzyEventHandlerExecutor> dequeueEventHandlers()
		{
			List<EzyEventHandlerExecutor> list = new List<EzyEventHandlerExecutor>();
			lock (eventExecutors)
			{
				while (!eventExecutors.isEmpty())
				{
					list.Add(eventExecutors.poll());
				}
			}
			return list;
		}

		private IList<EzyDataHandlerExecutor> dequeueDataHandlers()
		{
			List<EzyDataHandlerExecutor> list = new List<EzyDataHandlerExecutor>();
			lock (dataExecutors)
			{
				while (!dataExecutors.isEmpty())
				{
					list.Add(dataExecutors.poll());
				}
			}
			return list;
		}
	}

    public class EzyEventHandlerExecutor : EzyLoggable
	{
		private readonly EzyEvent evt;
		private readonly EzyEventHandler handler;

		public EzyEventHandlerExecutor(EzyEvent evt, EzyEventHandler handler)
		{
			this.evt = evt;
			this.handler = handler;
		}

		public void execute()
		{
			try
			{
				handler.handle(evt);
			}
			catch (Exception ex)
			{
                logger.error("handle event: " + evt + " error", ex);
			}
		}
	}

    public class EzyDataHandlerExecutor : EzyLoggable
	{
		private readonly EzyArray data;
		private readonly EzyDataHandler handler;

		public EzyDataHandlerExecutor(EzyArray data, EzyDataHandler handler)
		{
			this.data = data;
			this.handler = handler;
		}

		public void execute()
		{
			try
			{
				handler.handle(data);
			}
			catch (Exception ex)
			{
                logger.error("handle data: " + data + " error", ex);
			}
		}
	}

}
