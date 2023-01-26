using System;
using com.tvd12.ezyfoxserver.client.constant;
using UnityEngine;

namespace com.tvd12.ezyfoxserver.client.unity
{
	[CreateAssetMenu]
	public class EzySocketConfigVariable : EzyScriptableVariable<EzySocketConfigVariable.EzySocketConfigModel>
	{
		[Serializable]
		public class EzySocketConfigModel
		{
			[SerializeField]
			private string zoneName;
			
			[SerializeField]
			private string appName;
			
			[SerializeField]
			private string webSocketUrl;

			[SerializeField]
			private string tcpUrl;
			
			[SerializeField]
			private int udpPort;

			[SerializeField]
			private bool udpUsage;

			public string ZoneName => zoneName;
			public string AppName => appName;
			public string WebSocketUrl => webSocketUrl;
			public string TcpUrl => tcpUrl;
			public int UdpPort => udpPort;
			public bool UdpUsage => udpUsage;
		}
	}
}
