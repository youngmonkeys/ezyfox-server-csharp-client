using System;
using com.tvd12.ezyfoxserver.client.logger;
using UnityEngine;

namespace com.tvd12.ezyfoxserver.client.unity
{
	public class EzyUnityLoggerFactory : MonoBehaviour
	{
		[SerializeField]
		private EzyLoggerLevel loggerLevel = EzyLoggerLevel.INFO;

		[SerializeField]
		private bool jsDebug;
		
		private void Awake()
		{
			EzyLoggerFactory.setLoggerLevel(loggerLevel);
			EzyWSClient.setJsDebug(jsDebug);
			EzyLoggerFactory.setLoggerSupply(
				type => new EzyUnityLogger(type)
			);
		}

		public static EzyLogger getLogger(Type type)
		{
			return EzyLoggerFactory.getLogger(type);
		}

		public static EzyLogger getLogger<T>()
		{
			return getLogger(typeof(T));
		}
	}
}
