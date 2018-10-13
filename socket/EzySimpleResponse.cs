using System;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public class EzySimpleResponse : EzyResponse
	{
		private EzyArray data;
		private DateTime timestamp;
		private EzyCommand command;

		public EzySimpleResponse(EzyArray data)
		{
			this.data = data;
			int cmdId = data.get<int>(0);

			this.command = (EzyCommand)cmdId;
			this.timestamp = DateTime.Now;
		}

		public EzyArray getData()
		{
			return data;
		}

		public DateTime getTimestamp()
		{
			return timestamp;
		}

		public EzyCommand getCommand()
		{
			return command;
		}

		public void release()
		{
			this.data = null;
		}

	}
}