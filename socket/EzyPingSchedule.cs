using System;
using System.Threading;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.evt;
using com.tvd12.ezyfoxserver.client.manager;
using com.tvd12.ezyfoxserver.client.request;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public class EzyPingSchedule
	{
		private Thread thread;
		private EzySocketDataHandler dataHandler;
		private readonly EzyClient client;
		private readonly EzyPingManager pingManager;
		private volatile bool active = true;

		public EzyPingSchedule(EzyClient client)
		{
			this.client = client;
			this.pingManager = client.getPingManager();

		}

		public void start()
		{
			thread = new Thread(loop);
			active = true;
			thread.Name = "ping-schedule";
			thread.Start();
		}

		public void loop()
		{
			while (active)
				handle();
		}

		public void stop()
		{
			this.active = false;
		}

		private void handle()
		{
			try
			{
				int periodMillis = pingManager.getPingPeriod();
				Thread.Sleep(periodMillis);
				sendPingRequest();
			}
			catch (Exception e)
			{
				Console.WriteLine("ping thread has interrupted: " + e);
			}
		}

		private void sendPingRequest()
		{
			int lostPingCount = pingManager.increaseLostPingCount();
			int maxLostPingCount = pingManager.getMaxLostPingCount();
			if (lostPingCount >= maxLostPingCount)
			{
				dataHandler.fireSocketDisconnected(EzyDisconnectReason.SERVER_NOT_RESPONDING);
			}
			else
			{
				EzyRequest request = new EzyPingRequest();
				client.send(request);
			}
			if (lostPingCount > 1)
			{
				Console.WriteLine("lost ping count: " + lostPingCount);
				EzyLostPingEvent evt = new EzyLostPingEvent(lostPingCount);
				EzySocketEvent socketEvent = new EzySimpleSocketEvent(EzySocketEventType.EVENT, evt);
				dataHandler.fireSocketEvent(socketEvent);
			}
		}

		public void setDataHandler(EzySocketDataHandler dataHandler)
		{
			this.dataHandler = dataHandler;
		}
	}
}
