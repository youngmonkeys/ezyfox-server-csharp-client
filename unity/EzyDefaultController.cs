using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.config;
using com.tvd12.ezyfoxserver.client.logger;
using com.tvd12.ezyfoxserver.client.support;
using UnityEngine;
using Object = System.Object;

namespace com.tvd12.ezyfoxserver.client.unity
{
	public class EzyDefaultController : MonoBehaviour
	{
		[SerializeField]
		protected EzySocketConfigVariable socketConfigVariable;
		protected EzySocketProxy socketProxy;
		protected EzyAppProxy appProxy;
		
		private readonly List<Tuple<String, Object>> handlers = new();
		
		protected static readonly EzyLogger LOGGER = EzyUnityLoggerFactory.getLogger<EzyDefaultController>();

		protected void Start()
		{
			LOGGER.debug("Start");
			var socketProxyManager = EzySocketProxyManager.getInstance();
			if (!socketProxyManager.hasInited())
			{
				socketProxyManager.init();
			}
			socketProxy = socketProxyManager.getSocketProxy(socketConfigVariable.Value.ZoneName);
			if (socketProxy.getClient() == null)
			{
				LOGGER.debug("Creating ezyClient");
				var config = EzyClientConfig.builder()
					.clientName(socketConfigVariable.Value.ZoneName)
					.zoneName(socketConfigVariable.Value.ZoneName)
					.build();
				EzyClientFactory.getInstance()
					.getOrCreateClient(config);
			}
			appProxy = socketProxy.getAppProxy(socketConfigVariable.Value.AppName, true);
		}

		protected void on<T>(String cmd, EzyAppProxyDataHandler<T> handler)
		{
			handlers.Add(
				new Tuple<String, Object>(cmd, appProxy.on(cmd, handler))
			);
		}

		protected virtual void OnDestroy()
		{
			LOGGER.debug("OnDestroy");
			foreach (Tuple<String, Object> tuple in handlers)
			{
				appProxy.unbind(tuple.Item1, tuple.Item2);
			}
		}
	}
}
