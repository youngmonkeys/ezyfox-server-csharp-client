using System;
using System.Runtime.InteropServices;

namespace com.tvd12.ezyfoxserver.client.unity
{
	public class EzyWSProxy
	{
		[DllImport("__Internal")]
		public static extern void setEventHandlerCallback(EzyDelegates.EventHandlerDelegate callback);
		
		[DllImport("__Internal")]
		public static extern void setDataHandlerCallback(EzyDelegates.DataHandlerDelegate callback);
		
		[DllImport("__Internal")]
		public static extern void run3(String clientName, String functionName, EzyDelegates.Delegate2 callback);

		[DllImport("__Internal")]
		public static extern void run4(String clientName, String functionName, String jsonData, EzyDelegates.Delegate2 callback);
	}
}
