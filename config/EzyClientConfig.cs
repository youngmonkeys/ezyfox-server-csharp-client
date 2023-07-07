using System;
using com.tvd12.ezyfoxserver.client.builder;

namespace com.tvd12.ezyfoxserver.client.config
{
	public class EzyClientConfig
	{
        protected readonly String zoneName;
        protected readonly String clientName;
        protected readonly bool enableSSL;
        protected readonly bool enableDebug;
        protected readonly EzyReconnectConfig reconnect;
		protected readonly EzyPingConfig ping;

		protected EzyClientConfig(Builder builder)
		{
			this.zoneName = builder._zoneName;
            this.clientName = builder._clientName;
			this.enableSSL = builder._enableSSL;
			this.enableDebug = builder._enableDebug;
			this.ping = builder._pingConfigBuilder.build();
			this.reconnect = builder._reconnectConfigBuilder.build();
		}

		public String getZoneName()
		{
			return zoneName;
		}

        public String getClientName()
        {
            if (clientName == null)
                return zoneName;
            return clientName;
        }

		public bool isEnableSSL()
		{
			return enableSSL;
		}

		public bool isEnableDebug()
		{
			return enableDebug;
		}

		public EzyPingConfig getPing()
        {
			return ping;
        }

		public EzyReconnectConfig getReconnect()
		{
			return reconnect;
		}

		public static Builder builder()
		{
			return new Builder();
		}

		public class Builder : EzyBuilder<EzyClientConfig>
		{
            public String _zoneName;
            public String _clientName;
            public bool _enableSSL;
            public bool _enableDebug;
            public readonly EzyPingConfig.Builder _pingConfigBuilder;
			public readonly EzyReconnectConfig.Builder _reconnectConfigBuilder;

			public Builder()
			{
				this._pingConfigBuilder = new EzyPingConfig.Builder(this);
				this._reconnectConfigBuilder = new EzyReconnectConfig.Builder(this);
			}

			public Builder zoneName(String zoneName)
			{
				this._zoneName = zoneName;
				return this;
			}

            public Builder clientName(String clientName)
            {
                this._clientName = clientName;
                return this;
            }

            public Builder enableSSL()
            {
                return enableSSL(true);
            }

            public Builder enableSSL(bool enableSSL)
            {
                this._enableSSL = enableSSL;
                return this;
            }

            public Builder enableDebug()
            {
                return enableDebug(true);
            }

            public Builder enableDebug(bool enableDebug)
            {
                this._enableDebug = enableDebug;
                return this;
            }

            public EzyPingConfig.Builder pingConfigBuilder()
			{
				return _pingConfigBuilder;
			}

			public EzyReconnectConfig.Builder reconnectConfigBuilder()
			{
				return _reconnectConfigBuilder;
			}

			public EzyClientConfig build()
			{
				return new EzyClientConfig(this);
			}
		}
	}
}
