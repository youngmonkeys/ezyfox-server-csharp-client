using System;
using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public class EzySocketResponseApi : EzyAbstractResponseApi
	{
		protected readonly EzySocketDataEncoder encoder;

		public EzySocketResponseApi(
			EzySocketDataEncoder encoder,
			EzyPacketQueue packetQueue
		): base(packetQueue)
		{
			this.encoder = encoder;
		}

		protected override Object encodeData(EzyArray data)
		{
			return encoder.encode(data);
		}

        protected override byte[] dataToMessageContent(EzyArray data)
		{
			return encoder.toMessageContent(data);
		}

        protected override byte[] encryptMessageContent(
            byte[] messageContent,
            byte[] encryptionKey
        )
		{
			return encoder.encryptMessageContent(messageContent, encryptionKey);
		}
    }
}
