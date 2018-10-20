using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.evt;
using com.tvd12.ezyfoxserver.client.handler;
using com.tvd12.ezyfoxserver.client.manager;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public class EzySocketDataEventHandler : EzyAbstractSocketEventHandler
	{
		private readonly EzyMainThreadQueue mainThreadQueue;
		private readonly EzySocketDataHandler dataHandler;
		private readonly EzyPingManager pingManager;
		private readonly EzyHandlerManager handlerManager;
		private readonly EzySocketEventQueue socketEventQueue;
		private readonly ISet<Object> unloggableCommands;

		public EzySocketDataEventHandler(EzyMainThreadQueue mainThreadQueue,
		                                 EzySocketDataHandler dataHandler,
										 EzyPingManager pingManager,
										 EzyHandlerManager handlerManager,
										 EzySocketEventQueue socketEventQueue,
										 ISet<Object> unloggableCommands)
		{
			this.mainThreadQueue = mainThreadQueue;
			this.dataHandler = dataHandler;
			this.pingManager = pingManager;
			this.handlerManager = handlerManager;
			this.socketEventQueue = socketEventQueue;
			this.unloggableCommands = unloggableCommands;
		}

		public override void handleEvent()
		{
			try
			{
				EzySocketEvent evt = socketEventQueue.take();
				EzySocketEventType eventType = evt.getType();
				Object eventData = evt.getData();
				if (eventType == EzySocketEventType.EVENT)
					processEvent((EzyEvent)eventData);
				else
					processResponse((EzyResponse)eventData);
			}
			catch (Exception e)
			{
                logger.error("can't take socket response", e);
			}
		}

		private void processEvent(EzyEvent evt)
		{
			EzyEventType eventType = evt.getType();
			EzyEventHandler handler = handlerManager.getEventHandler(eventType);
			if (handler != null)
				mainThreadQueue.add(evt, handler);
			else
                logger.warn("has no handler with event: " + eventType);
		}

		private void processResponse(EzyResponse response)
		{
			pingManager.setLostPingCount(0);
			Object cmd = response.getCommand();
			EzyArray data = response.getData();
			EzyArray responseData = data.get<EzyArray>(1, null);
			if (!unloggableCommands.Contains(cmd))
                logger.debug("received command: " + cmd + " and data: " + responseData);
			if (EzyCommand.DISCONNECT.Equals(cmd))
				handleDisconnection(responseData);
			else
				handleResponseData(cmd, responseData);
		}

		private void handleDisconnection(EzyArray responseData)
		{
			int reasonId = responseData.get<int>(0, 0);
			EzyDisconnectReason disconnectReason = (EzyDisconnectReason)reasonId;
			dataHandler.fireSocketDisconnected(disconnectReason);
		}

		private void handleResponseData(Object cmd, EzyArray responseData)
		{
			EzyDataHandler handler = handlerManager.getDataHandler(cmd);
			if (handler != null)
				mainThreadQueue.add(responseData, handler);
			else
                logger.warn("has no handler with command: " + cmd);
		}
	}
}
