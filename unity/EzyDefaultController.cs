using System;
using UnityEngine;

namespace com.tvd12.ezyfoxserver.client.unity
{
	[Obsolete("Please use EzyAbstractController instead.")]
	public abstract class EzyDefaultController : EzyAbstractController
	{
		[SerializeField]
		private EzySocketConfigHolderVariable socketConfigHolderVariable;

		protected override EzySocketConfig GetSocketConfig()
		{
			var configVariable = socketConfigHolderVariable.Value;
			return EzySocketConfig.GetBuilder()
				.ZoneName(configVariable.Value.ZoneName)
				.AppName(configVariable.Value.AppName)
				.WebSocketUrl(configVariable.Value.WebSocketUrl)
				.TcpUrl(configVariable.Value.TcpUrl)
				.UdpPort(configVariable.Value.UdpPort)
				.UdpUsage(configVariable.Value.UdpUsage)
				.EnableSSL(configVariable.Value.EnableSSL)
				.Build();
		}
	}
}
