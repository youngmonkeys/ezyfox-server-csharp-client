using System;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.factory;

namespace com.tvd12.ezyfoxserver.client.request
{
	public class EzyHandshakeRequest : EzyRequest
	{
		protected readonly String clientId;
		protected readonly String clientKey;
		protected readonly String clientType;
		protected readonly String clientVersion;
		protected readonly bool enableEncryption;
		protected readonly String token;

		public EzyHandshakeRequest(String clientId,
								   String clientKey,
								   String clientType,
								   String clientVersion,
								   bool enableEncryption, String token)
		{
			this.clientId = clientId;
			this.clientKey = clientKey;
			this.clientType = clientType;
			this.clientVersion = clientVersion;
			this.token = token;
			this.enableEncryption = enableEncryption;
		}

		public Object getCommand()
		{
			return EzyCommand.HANDSHAKE;
		}

		public EzyData serialize()
		{
			EzyData data = EzyEntityFactory.newArrayBuilder()
					.append(clientId)
					.append(clientKey)
					.append(clientType)
					.append(clientVersion)
					.append(enableEncryption)
					.append(token).build();
			return data;
		}
	}
}
