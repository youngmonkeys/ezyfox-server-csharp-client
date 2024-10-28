using System;
using System.Runtime.InteropServices;

namespace com.tvd12.ezyfoxserver.client.unity
{
	public class EzyWSProxy
	{
#if UNITY_WEBGL && !UNITY_EDITOR
		[DllImport("__Internal")]
		public static extern void setEventHandlerCallback(
			EzyDelegates.EventHandlerDelegate callback
			);
		
		[DllImport("__Internal")]
		public static extern void setDataHandlerCallback(
			EzyDelegates.DataHandlerDelegate callback
			);
		
		[DllImport("__Internal")]
		public static extern void setDebug(bool debug);

		[DllImport("__Internal")]
		public static extern bool isMobile();
		
		[DllImport("__Internal")]
		public static extern void run3(
			String clientName,
			String functionName,
			EzyDelegates.Delegate2 callback
		);

		[DllImport("__Internal")]
		public static extern void run4(
			String clientName,
			String functionName,
			String jsonData,
			EzyDelegates.Delegate2 callback
		);
#else
		public static void setEventHandlerCallback(
			EzyDelegates.EventHandlerDelegate callback
		) {}

		public static void setDataHandlerCallback(
			EzyDelegates.DataHandlerDelegate callback
		) {}

		public static void setDebug(bool debug) {}

		public static bool isMobile() {
			return false;
		}

		public static void run3(
			String clientName,
			String functionName,
			EzyDelegates.Delegate2 callback
		) {}

		public static void run4(
			String clientName,
			String functionName,
			String jsonData,
			EzyDelegates.Delegate2 callback
		) {}
#endif
	}
}
