using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.evt;
using com.tvd12.ezyfoxserver.client.manager;
using com.tvd12.ezyfoxserver.client.request;
using com.tvd12.ezyfoxserver.client.util;
using com.tvd12.ezyfoxserver.client.concurrent;

namespace com.tvd12.ezyfoxserver.client.socket
{
    public class EzyPingSchedule : EzyLoggable, EzyEventLoopEvent
    {
        protected readonly EzyClient client;
        protected readonly EzyRequest pingRequest;
        protected readonly EzyPingManager pingManager;
        protected readonly EzyEventLoopGroup eventLoopGroup;
        protected EzyScheduleAtFixedRate schedule;
        protected EzySocketEventQueue socketEventQueue;

		public EzyPingSchedule(
            EzyClient client,
            EzyEventLoopGroup eventLoopGroup
        )
		{
			this.client = client;
            this.pingRequest = new EzyPingRequest();
            this.eventLoopGroup = eventLoopGroup;
            this.pingManager = client.getPingManager();

		}

        public bool call()
        {
            sendPingRequest();
            return true;
        }

        public void onFinished()
        {
            // do nothing
        }

        public void onRemoved()
        {
            // do nothing
        }

        public virtual void start()
		{
            int periodMillis = pingManager.getPingPeriod();
            if (eventLoopGroup != null)
            {
                eventLoopGroup.addScheduleEvent(
                    this,
                    periodMillis,
                    periodMillis
                );
            }
            else
            {
                lock (this)
                {
                    this.schedule = newSchedule();
                    this.schedule.schedule(sendPingRequest, periodMillis, periodMillis);
                }
            }
		}

        private EzyScheduleAtFixedRate newSchedule() {
            EzyScheduleAtFixedRate answer = new EzyScheduleAtFixedRate("ezyfox-ping-schedule");
            return answer;
        }

		public virtual void stop()
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
                client.getSocket().disconnect((int)EzyDisconnectReason.SERVER_NOT_RESPONDING);
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

        public virtual void setSocketEventQueue(EzySocketEventQueue socketEventQueue)
        {
            this.socketEventQueue = socketEventQueue;
        }
	}
}
