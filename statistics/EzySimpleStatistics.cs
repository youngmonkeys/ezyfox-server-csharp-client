namespace com.tvd12.ezyfoxserver.client.statistics
{
    public class EzySimpleStatistics : EzyStatistics
    {
        protected EzySocketStatistics socketStats;

        public EzySimpleStatistics()
        {
            socketStats = newSocketStatistics();
        }

        protected EzySocketStatistics newSocketStatistics()
        {
            return new EzySimpleSocketStatistics();
        }

        public EzySocketStatistics getSocketStats()
        {
            return this.socketStats;
        }
    }
}
