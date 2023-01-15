using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.logger;
using com.tvd12.ezyfoxserver.client.support;
using Object = System.Object;

namespace com.tvd12.ezyfoxserver.client.unity
{
	public partial class EzySingletonSocketManager
	{
		private class EzyWebGLSocketManager : AbstractEzySocketManager
		{
			private delegate void HandleDelegate(String cmd, String jsonString);
			private delegate void NoArgDelegate();

			private readonly IDictionary<String, IDictionary<Object, AppProxyDataHandler>>
				appHandlersByCommand = new Dictionary<String, IDictionary<Object, AppProxyDataHandler>>();
			
			private readonly EzyLogger logger = EzyUnityLoggerManager.getInstance().getLogger(typeof(EzyWebGLSocketManager));

			private EzySocketProxy socketProxy;
			private EzyAppProxy appProxy;

			[DllImport("__Internal")]
			private static extern void addCommand(String command, HandleDelegate callback);

			[DllImport("__Internal")]
			private static extern void init(String zoneName, String appName);

			[DllImport("__Internal")]
			private static extern void clientConnect(String host, String username, String password, NoArgDelegate callback);

			[DllImport("__Internal")]
			private static extern void sendCommand(String command);

			[DllImport("__Internal")]
			private static extern void sendCommandData(String command, String dataJson);

			protected override void setupSocketProxy()
			{
				logger.debug("Setting up socket proxy");
				init(socketConfig.Value.ZoneName, socketConfig.Value.AppName);
			}

			public override void login(
				String host,
				String username,
				String password
			)
			{
				clientConnect(host, username, password, staticAppAccessedCallback);
			}

			[MonoPInvokeCallback(typeof(NoArgDelegate))]
			public static void staticAppAccessedCallback()
			{
				((EzyWebGLSocketManager)getInstance()).appAccessedCallback();
			}

			private void appAccessedCallback()
			{
				appAccessedHandler?.Invoke(null, null);
			}

			public override Object on<T>(String cmd, EzyAppProxyDataHandler<T> handler)
			{
				logger.debug("On " + cmd);
				AppProxyDataHandler dataHandler = (appProxy, data) => { handler.Invoke(appProxy, (T)data); };
				IDictionary<Object, AppProxyDataHandler> handlers =
					appHandlersByCommand.ContainsKey(cmd)
						? appHandlersByCommand[cmd]
						: null;
				if (handlers == null)
				{
					handlers = new Dictionary<Object, AppProxyDataHandler>();
					appHandlersByCommand[cmd] = handlers;
				}
				addCommand(cmd, staticAppDataCallback);
				handlers[handler] = dataHandler;
				return handler;
			}

			[MonoPInvokeCallback(typeof(HandleDelegate))]
			public static void staticAppDataCallback(String cmd, String jsonString)
			{
				((EzyWebGLSocketManager)getInstance()).handleAppData(cmd, jsonString);
			}

			private void handleAppData(String cmd, String jsonString)
			{
				IDictionary<Object, AppProxyDataHandler> handlers = appHandlersByCommand.ContainsKey(cmd)
					? appHandlersByCommand[cmd]
					: null;

				EzyData data = EzyJsonUtils.deserialize(jsonString);

				logger.debug("HandleAppData " + cmd + " " + data);

				if (handlers != null)
				{
					foreach (KeyValuePair<Object, AppProxyDataHandler> entry in handlers)
					{
						entry.Value.Invoke(null, data);
					}
				}
			}

			public override void removeAppHandler(string cmd, object handler)
			{
				if (appHandlersByCommand.ContainsKey(cmd))
				{
					appHandlersByCommand[cmd].Remove(handler);
				}
			}

			public override void send(string cmd)
			{
				sendCommand(cmd);
			}

			public override void send(string cmd, object data)
			{
				String dataJson = EzyJsonUtils.serialize(data);
				logger.debug(dataJson);
				sendCommandData(cmd, dataJson);
			}

			public override void processEvents()
			{
				// do nothing
			}
		}
	}
}
