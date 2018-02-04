using System;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.constant;

namespace com.tvd12.ezyfoxserver.client.request
{
	public class EzyHandshakeRequest : EzyFixedRequest
	{
		public EzyHandshakeRequest(EzyParams parameters) : base(EzyCommand.HANDSHAKE, parameters)
		{
		}
	}

	public class EzyHandshakeRequestParams : EzyEmptyParams
	{
		private String clientId;
		private String clientKey;
		private String reconnectToken;

		public override EzyArray serialize()
		{
			var array = newArrayBuilder()
				.append(clientId)
				.append(clientKey)
				.append(reconnectToken)
				.append(getClientType())
				.append(getClientVersion())
				.build();
			return array;
		}

		public String getClientType()
		{
			return "csharp";
		}

		public String getClientVersion()
		{
			return "1.0.0";
		}

		public String getClientId()
		{
			return clientId;
		}

		public String getClientKey()
		{
			return clientKey;
		}

		public String getReconnectToken()
		{
			return reconnectToken;
		}

		public void setClientId(String clientId)
		{
			this.clientId = clientId;
		}

		public void setClientKey(String clientKey)
		{
			this.clientKey = clientKey;
		}

		public void setReconnectToken(String reconnectToken)
		{
			this.reconnectToken = reconnectToken;
		}
	}
}
