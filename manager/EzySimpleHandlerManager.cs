using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.evt;
using com.tvd12.ezyfoxserver.client.handler;
using com.tvd12.ezyfoxserver.client.socket;

namespace com.tvd12.ezyfoxserver.client.manager
{
	public class EzySimpleHandlerManager : EzyHandlerManager
	{

        protected readonly EzyClient client;
        protected readonly EzyPingSchedule pingSchedule;
        protected readonly EzyEventHandlers eventHandlers;
        protected readonly EzyDataHandlers dataHandlers;
        protected readonly IDictionary<String, EzyAppDataHandlers> appDataHandlersByAppName;
		protected readonly IDictionary<String, EzyPluginDataHandlers> pluginDataHandlersByPluginName;

		public EzySimpleHandlerManager(EzyClient client)
		{
			this.client = client;
            this.pingSchedule = client.getPingSchedule();
			this.eventHandlers = newEventHandlers();
			this.dataHandlers = newDataHandlers();
			this.appDataHandlersByAppName = new Dictionary<String, EzyAppDataHandlers>();
			this.pluginDataHandlersByPluginName = new Dictionary<String, EzyPluginDataHandlers>();
		}

		private EzyEventHandlers newEventHandlers()
		{
			EzyEventHandlers handlers = new EzyEventHandlers(client);
			handlers.addHandler(EzyEventType.CONNECTION_SUCCESS, new EzyConnectionSuccessHandler());
			handlers.addHandler(EzyEventType.CONNECTION_FAILURE, new EzyConnectionFailureHandler());
			handlers.addHandler(EzyEventType.DISCONNECTION, new EzyDisconnectionHandler());
			return handlers;
		}

		private EzyDataHandlers newDataHandlers()
		{
			EzyDataHandlers handlers = new EzyDataHandlers(client);
			handlers.addHandler(EzyCommand.PONG, new EzyPongHandler());
            handlers.addHandler(EzyCommand.LOGIN, new EzyLoginSuccessHandler());
            handlers.addHandler(EzyCommand.LOGIN_ERROR, new EzyLoginErrorHandler());
            handlers.addHandler(EzyCommand.APP_ACCESS, new EzyAppAccessHandler());
			handlers.addHandler(EzyCommand.APP_REQUEST, new EzyAppResponseHandler());
            handlers.addHandler(EzyCommand.APP_EXIT, new EzyAppExitHandler());
			handlers.addHandler(EzyCommand.PLUGIN_INFO, new EzyPluginInfoHandler());
			handlers.addHandler(EzyCommand.PLUGIN_REQUEST, new EzyPluginResponseHandler());
			handlers.addHandler(EzyCommand.UDP_HANDSHAKE, new EzyUdpHandshakeHandler());
			return handlers;
		}

        public EzyEventHandlers getEventHandlers() 
        {
            return eventHandlers;    
        }
        public EzyDataHandlers getDataHandlers() 
        {
            return dataHandlers;    
        }

		public EzyDataHandler getDataHandler(Object cmd)
		{
			return dataHandlers.getHandler(cmd);
		}

		public EzyEventHandler getEventHandler(Object eventType)
		{
			return eventHandlers.getHandler(eventType);
		}

		public EzyAppDataHandlers getAppDataHandlers(String appName)
		{
			EzyAppDataHandlers answer = null;
			if (appDataHandlersByAppName.ContainsKey(appName))
			{
				answer = appDataHandlersByAppName[appName];
			}
			if (answer == null)
			{
				answer = new EzyAppDataHandlers();
				appDataHandlersByAppName[appName] = answer;
			}
			return answer;
		}

		public EzyPluginDataHandlers getPluginDataHandlers(String pluginName)
		{
			EzyPluginDataHandlers answer = null;
			if (pluginDataHandlersByPluginName.ContainsKey(pluginName))
			{
				answer = pluginDataHandlersByPluginName[pluginName];
			}
			if (answer == null)
			{
				answer = new EzyPluginDataHandlers();
				pluginDataHandlersByPluginName[pluginName] = answer;
			}
			return answer;
		}

		public void addDataHandler(Object cmd, EzyDataHandler dataHandler)
		{
			dataHandlers.addHandler(cmd, dataHandler);
		}

		public void addEventHandler(Object eventType, EzyEventHandler eventHandler)
		{
			eventHandlers.addHandler(eventType, eventHandler);
		}
	}
}
