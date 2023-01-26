using com.tvd12.ezyfoxserver.client.builder;

namespace com.tvd12.ezyfoxserver.client.config
{
    public class EzyPingConfig
    {

        private readonly int pingPeriod;
        private readonly int maxLostPingCount;

        public EzyPingConfig(Builder builder)
        {
            this.pingPeriod = builder._pingPeriod;
            this.maxLostPingCount = builder._maxLostPingCount;
        }

        public int getPingPeriod()
        {
            return pingPeriod;
        }

        public int getMaxLostPingCount()
        {
            return maxLostPingCount;
        }

        public class Builder : EzyBuilder<EzyPingConfig>
        {

            public int _pingPeriod = 3000;
            public int _maxLostPingCount = 5;
            public EzyClientConfig.Builder _parent;

            public Builder(EzyClientConfig.Builder parent)
            {
                this._parent = parent;
            }

            public Builder pingPeriod(int pingPeriod)
            {
                this._pingPeriod = pingPeriod;
                return this;
            }

            public Builder maxLostPingCount(int maxLostPingCount)
            {
                this._maxLostPingCount = maxLostPingCount;
                return this;
            }

            public EzyClientConfig.Builder done()
            {
                return this._parent;
            }

            public EzyPingConfig build()
            {
                return new EzyPingConfig(this);
            }
        }
    }
}
