using System;
using com.tvd12.ezyfoxserver.client.builder;

namespace com.tvd12.ezyfoxserver.client.config
{
	public class EzyClientConfig
	{

		private readonly String zoneName;
		private readonly EzyReconnectConfig reconnect;

		protected EzyClientConfig(Builder builder)
		{
			this.zoneName = builder._zoneName;
			this.reconnect = builder._reconnectConfigBuilder.build();
		}

		public String getZoneName()
		{
			return zoneName;
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
