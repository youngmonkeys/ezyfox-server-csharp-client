using System;
using com.tvd12.ezyfoxserver.client.evt;
using com.tvd12.ezyfoxserver.client.util;
using com.tvd12.ezyfoxserver.client.socket;
using com.tvd12.ezyfoxserver.client.handler;
using com.tvd12.ezyfoxserver.client.request;
using com.tvd12.ezyfoxserver.client.entity;
using static com.tvd12.ezyfoxserver.client.socket.EzySocketStatus;

namespace com.tvd12.ezyfoxserver.client
{

	public class EzyClient : EzyLoggable, EzyRequestDeliver
	{
		private EzyEventHandlers eventHandlers;
		private EzySocketClient socketTcpClient;
		private EzyRequestSerializer<EzyArray> requestSerializer;

		public EzyClient()
		{
			this.eventHandlers = new EzyEventHandlers();
			this.requestSerializer = new EzyArrayRequestSerializer();
		}

		public void handleEvent(EzyEvent evt)
		{
			eventHandlers.handleEvent(evt);
		}

		public void addEventHandler<E>(int eventType, EzyEventHandler<E> handler) where E : EzyEvent
		{
			if (handler is EzyClientAware)
			{
				((EzyClientAware)handler).setClient(this);
			}
			if (handler is EzyRequestDeliverAware)
			{
				((EzyRequestDeliverAware)handler).setRequestDeliver(this);
			}
			eventHandlers.addEventHandler<E>(eventType, handler);
		}

		public void connect(String host, int port)
		{
			socketTcpClient = new EzySocketTcpClient();
			socketTcpClient.setDataEventHandler(newDataEventHandler());
			socketTcpClient.setStatusEventHanlder(newStatusEventHandler());
			socketTcpClient.connect(host, port);
		}

		public void processEvents()
		{
			socketTcpClient.processEvents();
		}

		public void send(EzyRequest request)
		{
			var message = requestSerializer.serialize(request);
			socketTcpClient.sendMessage(message);
		}

		private EzySocketDataEventHandler newDataEventHandler()
		{
			EzySocketTcpDataEventHandler handler = new EzySocketTcpDataEventHandler();
			handler.setEventHandlers(eventHandlers);
			return handler;
		}

		private EzySocketStatusEventHandler newStatusEventHandler()
		{
			EzySocketTcpStatusEventHandler handler = new EzySocketTcpStatusEventHandler();
			handler.setEventHandlers(eventHandlers);
			return handler;
		}

		private class EzySocketTcpDataEventHandler : EzySocketDataEventHandler
		{
			private EzyEventHandlers eventHandlers;

			public void handle(EzySocketDataEvent evt)
			{
				EzyArray data = evt.getData();
				int command = data.get<int>(0);
				EzyArray parameters = data.get<EzyArray>(1);
			}

			public void setEventHandlers(EzyEventHandlers eventHandlers)
			{
				this.eventHandlers = eventHandlers;
			}
		}

		private class EzySocketTcpStatusEventHandler : EzySocketStatusEventHandler
		{
			private EzyEventHandlers eventHandlers;

			public void handle(EzySocketStatusEvent evt)
			{
				int status = evt.getSocketStatus();
				switch (status)
				{
					case CONNECTED:
						EzyConnectionSuccessEvent hevent = new EzyConnectionSuccessEvent();
						eventHandlers.handleEvent(hevent);
						break;
					default:
						break;
				}
			}

			public void setEventHandlers(EzyEventHandlers eventHandlers)
			{
				this.eventHandlers = eventHandlers;
			}
		}

	}
}
