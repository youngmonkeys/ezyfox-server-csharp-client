using System;
using com.tvd12.ezyfoxserver.client.logger;
using UnityEngine;

namespace com.tvd12.ezyfoxserver.client.unity
{
	[CreateAssetMenu]
	public class SocketConfigVariable : GenericVariable<SocketConfigVariable.SocketConfigModel>
	{
		[Serializable]
		public class SocketConfigModel
		{
			[SerializeField]
			private string zoneName;
			[SerializeField]
			private string appName;

			public string ZoneName => zoneName;
			public string AppName => appName;
		}
	}
}
