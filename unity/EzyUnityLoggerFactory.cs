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
		
		private static bool INITIALIZED = false;
		
		private void Awake()
		{
			EzyLoggerFactory.setLoggerLevel(loggerLevel);
			EzyWSClient.setJsDebug(jsDebug);
		}

		public static EzyLogger getLogger(Type type)
		{
			if (!INITIALIZED)
			{
				EzyLoggerFactory.setLoggerSupply(
					type => new EzyUnityLogger(type)
				);
				INITIALIZED = true;
			}
			return EzyLoggerFactory.getLogger(type);
		}

		public static EzyLogger getLogger<T>()
		{
			return getLogger(typeof(T));
		}
	}
}
