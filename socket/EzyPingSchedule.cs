using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.evt;
using com.tvd12.ezyfoxserver.client.manager;
using com.tvd12.ezyfoxserver.client.request;
using com.tvd12.ezyfoxserver.client.util;
using com.tvd12.ezyfoxserver.client.concurrent;

namespace com.tvd12.ezyfoxserver.client.socket
{
    public class EzyPingSchedule : EzyLoggable
	{
        private EzyScheduleAtFixedRate schedule;
		private EzySocketDataHandler dataHandler;
		private readonly EzyClient client;
		private readonly EzyPingManager pingManager;

		public EzyPingSchedule(EzyClient client)
		{
			this.client = client;
			this.pingManager = client.getPingManager();

		}

		public void start()
		{
            lock(this) 
            {
                int periodMillis = pingManager.getPingPeriod();
                this.schedule = newSchedule();
                this.schedule.schedule(sendPingRequest, periodMillis, periodMillis);
            }
		}

        private EzyScheduleAtFixedRate newSchedule() {
            EzyScheduleAtFixedRate answer = new EzyScheduleAtFixedRate("ping-schedule");
            return answer;
        }

		public void stop()
		{
            lock (this)
            {
                if (schedule != null)
                    this.schedule.stop();
                this.schedule = null;
            }
		}

		private void sendPingRequest()
		{
			int lostPingCount = pingManager.increaseLostPingCount();
			int maxLostPingCount = pingManager.getMaxLostPingCount();
			if (lostPingCount >= maxLostPingCount)
			{
                dataHandler.fireSocketDisconnected((int)EzyDisconnectReason.SERVER_NOT_RESPONDING);
			}
			else
			{
				EzyRequest request = new EzyPingRequest();
				client.send(request);
			}
			if (lostPingCount > 1)
			{
                logger.info("lost ping count: " + lostPingCount);
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
