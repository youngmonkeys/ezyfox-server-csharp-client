using System;
using com.tvd12.ezyfoxserver.client.builder;

namespace com.tvd12.ezyfoxserver.client.config
{
	public class EzyClientConfig
	{
        protected readonly String zoneName;
        protected readonly String clientName;
        protected readonly EzyReconnectConfig reconnect;

		protected EzyClientConfig(Builder builder)
		{
			this.zoneName = builder._zoneName;
            this.clientName = builder._clientName;
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
            public readonly EzyReconnectConfig.Builder _reconnectConfigBuilder;

			public Builder()
			{
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
