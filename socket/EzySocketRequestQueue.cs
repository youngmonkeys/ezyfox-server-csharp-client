using System;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public interface EzySocketRequestQueue
	{
		int size();

		void clear();

		bool isFull();

		bool isEmpty();

		bool add(EzySocketRequest request);

		void remove(EzySocketRequest request);

		EzySocketRequest take();
	}
}
