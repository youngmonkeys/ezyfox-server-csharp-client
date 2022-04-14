namespace com.tvd12.ezyfoxserver.client.statistics
{
    public class EzySimpleComponentStatistics : EzyComponentStatistics
    {
        protected EzyNetworkStats networkStats;

        public EzyNetworkStats getNetworkStats()
        {
            return this.networkStats;
        }

        protected EzyNetworkStats newNetworkStats()
        {
            return new EzySimpleNetworkStats();
        }

        public EzySimpleComponentStatistics()
        {
            this.networkStats = newNetworkStats();
        }
    }
}
