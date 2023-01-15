using System;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.logger;
using com.tvd12.ezyfoxserver.client.support;

namespace com.tvd12.ezyfoxserver.client.unity
{
	public partial class EzySingletonSocketManager
	{
		private class EzyDefaultSocketManager : AbstractEzySocketManager
		{
			private EzySocketProxy socketProxy;
			private EzyAppProxy appProxy;

			protected override void setupLogger(EzyLoggerLevel loggerLevel)
			{
				EzyLoggerFactory.setLoggerLevel(loggerLevel);
				EzyLoggerFactory.setLoggerSupply(type => new UnityLogger(type));
				logger = EzyLoggerFactory.getLogger<EzyDefaultSocketManager>();
			}

			protected override void setupSocketProxy()
			{
				logger.debug("Setting up socket proxy");
				SocketProxyManager socketProxyManager = SocketProxyManager.getInstance();
				socketProxyManager.setDefaultZoneName(socketConfig.Value.ZoneName);
				socketProxyManager.init();

				socketProxy = socketProxyManager.getDefaultSocketProxy()
					.setTransportType(EzyTransportType.UDP)
					.setDefaultAppName(socketConfig.Value.AppName);

				appProxy = socketProxy.getDefaultAppProxy();
			}

			public override void login(
				String host,
				String username,
				String password)
			{
				logger.debug(loginSuccessHandler.ToString());
				logger.debug(appAccessedHandler.ToString());
				socketProxy.onLoginSuccess(loginSuccessHandler);
				socketProxy.onAppAccessed(appAccessedHandler);

				SocketProxyManager.getInstance()
					.getDefaultSocketProxy()
					.setHost(host)
					.setLoginUsername(username)
					.setLoginPassword(password)
					.connect();
			}

			public override Object on<T>(String cmd, EzyAppProxyDataHandler<T> handler)
			{
				return appProxy.on(cmd, handler);
			}

			public override void removeAppHandler(String cmd, Object handler)
			{
				appProxy.unbind(cmd, handler);
			}
			
			public override void send(string cmd)
			{
				appProxy.send(cmd);
			}
			
			public override void send(string cmd, object data)
			{
				appProxy.send(cmd, data);
			}
			
			public override void processEvents()
			{
				socketProxy.getClient().processEvents();
			}
		}
	}
}
