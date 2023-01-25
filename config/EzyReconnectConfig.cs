using com.tvd12.ezyfoxserver.client.builder;
using Newtonsoft.Json;

namespace com.tvd12.ezyfoxserver.client.config
{
	public class EzyReconnectConfig
	{
		[JsonProperty]
		private readonly bool enable;
		
		[JsonProperty]
		private readonly int maxReconnectCount;
		
		[JsonProperty]
		private readonly int reconnectPeriod;

		protected EzyReconnectConfig(Builder builder)
		{
			this.enable = builder._enable;
			this.reconnectPeriod = builder._reconnectPeriod;
			this.maxReconnectCount = builder._maxReconnectCount;
		}

		public bool isEnable()
		{
			return this.enable;
		}

		public int getMaxReconnectCount()
		{
			return maxReconnectCount;
		}

		public int getReconnectPeriod()
		{
			return reconnectPeriod;
		}

		public class Builder : EzyBuilder<EzyReconnectConfig>
		{

			public bool _enable = true;
			public int _maxReconnectCount = 5;
			public int _reconnectPeriod = 3000;
			public EzyClientConfig.Builder _parent;

			public Builder(EzyClientConfig.Builder parent)
			{
				this._parent = parent;
			}

			public Builder enable(bool enable)
			{
				this._enable = enable;
				return this;
			}

			public Builder reconnectPeriod(int reconnectPeriod)
			{
				this._reconnectPeriod = reconnectPeriod;
				return this;
			}

			public Builder maxReconnectCount(int maxReconnectCount)
			{
				this._maxReconnectCount = maxReconnectCount;
				return this;
			}

			public EzyClientConfig.Builder done()
			{
				return this._parent;
			}

			public EzyReconnectConfig build()
			{
				return new EzyReconnectConfig(this);
			}
		}
	}
}
