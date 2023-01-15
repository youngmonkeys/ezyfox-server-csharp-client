using System;
using com.tvd12.ezyfoxserver.client.logger;
using UnityEngine;

namespace com.tvd12.ezyfoxserver.client.unity
{
	public class EzyUnityLoggerManager
	{
		private static EzyUnityLoggerManager INSTANCE;

		public static EzyUnityLoggerManager getInstance()
		{
			if (INSTANCE == null)
			{
				LoggerConfigVariable loggerConfig = (LoggerConfigVariable)Resources.Load("EzyLoggerConfig");
				EzyLoggerFactory.setLoggerLevel(loggerConfig.Value);
				EzyLoggerFactory.setLoggerSupply(type => new EzyUnityLogger(type));
				INSTANCE = new EzyUnityLoggerManager();
			}
			return INSTANCE;
		}

		public EzyLogger getLogger(Type type)
		{
			return EzyLoggerFactory.getLogger(type);
		}
	}
}
