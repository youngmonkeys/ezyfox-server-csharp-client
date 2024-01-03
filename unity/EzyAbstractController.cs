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
	public abstract class EzyAbstractController : MonoBehaviour
	{
		protected EzySocketProxy socketProxy;
		protected EzyAppProxy appProxy;
		
		private readonly List<Object> socketHandlers = new();
		private readonly List<Tuple<String, Object>> appHandlers = new();
		
		protected static readonly EzyLogger LOGGER = EzyUnityLoggerFactory
			.getLogger<EzyAbstractController>();

		protected void OnEnable()
		{
			var socketConfig = GetSocketConfig();
			var socketProxyManager = EzySocketProxyManager.getInstance();
			if (!socketProxyManager.hasInited())
			{
				socketProxyManager.init();
			}
			socketProxy = socketProxyManager.getSocketProxy(
				socketConfig.ZoneName
			);
			if (socketProxy.getClient() == null)
			{
				var config = EzyClientConfig.builder()
					.clientName(socketConfig.ZoneName)
					.zoneName(socketConfig.ZoneName)
					.enableSSL(socketConfig.EnableSSL)
					.build();
				EzyClientFactory
					.getInstance()
					.getOrCreateClient(
						config,
						socketConfig.UdpUsage
					);
			}
			appProxy = socketProxy.getAppProxy(
				socketConfig.AppName,
				true
			);
		}

		protected abstract SocketConfig GetSocketConfig();

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

		protected void Disconnect() {
			socketProxy.disconnect();
		}

		protected virtual void OnDestroy()
		{
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
