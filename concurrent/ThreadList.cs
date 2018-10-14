using System;
using System.Collections.Generic;
using System.Threading;
using com.tvd12.ezyfoxserver.client.util;

namespace com.tvd12.ezyfoxserver.client.concurrent
{
	public class ThreadList : EzyStartable, EzyDestroyable
	{
		private readonly IList<Thread> threads = new List<Thread>();

		public ThreadList(int size, String threadName, ThreadStart task)
		{
			for (int i = 0; i < size; i++)
			{
				Thread thread = new Thread(task);
				thread.Name = threadName;
				threads.Add(thread);
			}
		}

		public void start()
		{
			foreach (Thread thread in threads)
				thread.Start();
		}

		public void destroy()
		{
			threads.Clear();
		}
	}
}
