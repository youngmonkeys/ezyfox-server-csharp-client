using System;
using com.tvd12.ezyfoxserver.client.logger;
using com.tvd12.ezyfoxserver.client.support;
using UnityEngine;
using Object = System.Object;

namespace com.tvd12.ezyfoxserver.client.unity
{
	public abstract class AbstractEzySocketManager
	{
		protected readonly SocketConfigVariable socketConfig = (SocketConfigVariable)Resources.Load("EzySocketConfig");
		protected EzyAppProxyDataHandler<Object> appAccessedHandler;
		protected EzySocketProxyDataHandler<Object> loginSuccessHandler;
		
		private readonly EzyLogger logger = EzyUnityLoggerManager.getInstance().getLogger(typeof(AbstractEzySocketManager));

		protected AbstractEzySocketManager()
		{
			init();
		}

		void init()
		{
			logger.debug("Initializing EzySocketManager");
			
			// Set up socket proxy and app proxy
			setupSocketProxy();
		}

		protected abstract void setupSocketProxy();

		public void setLoginSuccessHandler(EzySocketProxyDataHandler<Object> handler)
		{
			loginSuccessHandler = handler;
		}

		public void setAppAccessedHandler(EzyAppProxyDataHandler<Object> handler)
		{
			appAccessedHandler = handler;
		}
		
		public EzyLogger getLogger(Type type)
		{
			return EzyLoggerFactory.getLogger(type);
		}

		public abstract void login(
			String host,
			String username,
			String password
		);

		public abstract Object on<T>(String cmd, EzyAppProxyDataHandler<T> handler);

		public abstract void removeAppHandler(String cmd, Object handler);

		public abstract void send(String cmd);

		public abstract void send(String cmd, Object data);
		
		public abstract void processEvents();
	}
}
