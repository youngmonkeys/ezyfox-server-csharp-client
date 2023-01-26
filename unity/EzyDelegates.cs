using System;
namespace com.tvd12.ezyfoxserver.client.unity
{
	public class EzyDelegates
	{
		public delegate void Delegate1(
			String clientName
		);
		public delegate void Delegate2(
			String clientName,
			String jsonData
		);
		public delegate void EventHandlerDelegate(
			String clientName,
			String eventType,
			String jsonData
		);
		public delegate void DataHandlerDelegate(
			String clientName,
			int commandId,
			String jsonData
		);
	}
}
