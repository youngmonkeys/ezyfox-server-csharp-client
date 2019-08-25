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
        protected readonly EzyClient client;
        protected readonly EzyRequest pingRequest;
        protected readonly EzyPingManager pingManager;
        protected EzyScheduleAtFixedRate schedule;
        protected EzySocketEventQueue socketEventQueue;

		public EzyPingSchedule(EzyClient client)
		{
			this.client = client;
            this.pingRequest = new EzyPingRequest();
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
            EzyScheduleAtFixedRate answer = new EzyScheduleAtFixedRate("ezyfox-ping-schedule");
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
                EzyEvent evt = new EzyDisconnectionEvent((int)EzyDisconnectReason.SERVER_NOT_RESPONDING);
                socketEventQueue.addEvent(evt);
			}
			else
			{
				
				client.send(pingRequest);
			}
			if (lostPingCount > 1)
			{
                logger.info("lost ping count: " + lostPingCount);
                EzyEvent evt = new EzyLostPingEvent(lostPingCount);
                socketEventQueue.addEvent(evt);
			}
		}

        public void setSocketEventQueue(EzySocketEventQueue socketEventQueue)
        {
            this.socketEventQueue = socketEventQueue;
        }
	}
}
