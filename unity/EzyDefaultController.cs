using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.config;
using com.tvd12.ezyfoxserver.client.evt;
using com.tvd12.ezyfoxserver.client.logger;
using com.tvd12.ezyfoxserver.client.support;
using UnityEngine;
using Object = System.Object;

namespace com.tvd12.ezyfoxserver.client.unity
{
	public class EzyDefaultController : MonoBehaviour
	{
		[SerializeField]
		private EzySocketConfigHolderVariable socketConfigHolderVariable;
		protected EzySocketConfigVariable socketConfigVariable;
		protected EzySocketProxy socketProxy;
		protected EzyAppProxy appProxy;
		
		private readonly List<Object> socketHandlers = new();
		private readonly List<Tuple<String, Object>> appHandlers = new();
		
		protected static readonly EzyLogger LOGGER = EzyUnityLoggerFactory
			.getLogger<EzyDefaultController>();

		protected void OnEnable()
		{
			LOGGER.debug("OnEnable");
			socketConfigVariable = socketConfigHolderVariable.Value;
			var socketProxyManager = EzySocketProxyManager.getInstance();
			if (!socketProxyManager.hasInited())
			{
				socketProxyManager.init();
			}
			socketProxy = socketProxyManager.getSocketProxy(
				socketConfigVariable.Value.ZoneName
			);
			if (socketProxy.getClient() == null)
			{
				LOGGER.debug("Creating ezyClient");
				var config = EzyClientConfig.builder()
					.clientName(socketConfigVariable.Value.ZoneName)
					.zoneName(socketConfigVariable.Value.ZoneName)
					.build();
				EzyClientFactory
					.getInstance()
					.getOrCreateClient(
						config,
						socketConfigVariable.Value.UdpUsage
					);
			}
			appProxy = socketProxy.getAppProxy(
				socketConfigVariable.Value.AppName,
				true
			);
		}

		protected void Disconnect()
		{
			socketProxy.disconnect();
		}

		protected void OnLoginSuccess<T>(EzySocketProxyDataHandler<T> handler)
		{
			socketHandlers.Add(
				socketProxy.onLoginSuccess(handler)
			);
		}

		protected void OnLoginError<T>(EzySocketProxyDataHandler<T> handler)
		{
			socketHandlers.Add(
				socketProxy.onLoginError(handler)
			);
		}

		protected void OnUdpHandshake<T>(EzySocketProxyDataHandler<T> handler)
		{
			socketHandlers.Add(
				socketProxy.onUdpHandshake(handler)
			);
		}

		protected void OnAppAccessed<T>(EzyAppProxyDataHandler<T> handler)
		{
			socketHandlers.Add(
				socketProxy.onAppAccessed(handler)
			);
		}

		protected void OnDisconnected(EzySocketProxyEventHandler<EzyDisconnectionEvent> handler)
		{
			socketHandlers.Add(
				socketProxy.onDisconnected(handler)
			);
		}

		protected void OnReconnecting(EzySocketProxyEventHandler<EzyDisconnectionEvent> handler)
		{
			socketHandlers.Add(
				socketProxy.onReconnecting(handler)
			);
		}

		protected void OnPingLost(EzySocketProxyEventHandler<EzyLostPingEvent> handler)
		{
			socketHandlers.Add(
				socketProxy.onPingLost(handler)
			);
		}

		protected void OnTryConnect(EzySocketProxyEventHandler<EzyTryConnectEvent> handler)
		{
			socketHandlers.Add(
				socketProxy.onTryConnect(handler)
			);
		}

		protected void AddHandler<T>(String cmd, EzyAppProxyDataHandler<T> handler)
		{
			appHandlers.Add(
				new Tuple<String, Object>(cmd, appProxy.on(cmd, handler))
			);
		}

		protected virtual void OnDestroy()
		{
			LOGGER.debug("OnDestroy");
			UnbindSocketHandlers();
			UnbindAppHandlers();
		}

		protected virtual void UnbindSocketHandlers()
		{
			foreach (var socketProxyHandler in socketHandlers)
			{
				socketProxy.unbind(socketProxyHandler);
			}
		}

		protected virtual void UnbindAppHandlers()
		{
			foreach (Tuple<String, Object> tuple in appHandlers)
			{
				appProxy.unbind(tuple.Item1, tuple.Item2);
			}
		}
	}
}
