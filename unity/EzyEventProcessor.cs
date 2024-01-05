using System;
using UnityEngine;

namespace com.tvd12.ezyfoxserver.client.unity
{
	[Obsolete("Please use EzyAbstractEventProcessor instead.")]
	public abstract class EzyEventProcessor : EzyAbstractEventProcessor
	{
		[SerializeField]
		private EzySocketConfigHolderVariable socketConfig;

		protected override string GetZoneName()
		{
			return socketConfig.Value.Value.ZoneName;
		}
	}
}
