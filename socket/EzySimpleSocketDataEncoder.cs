using System;
using com.tvd12.ezyfoxserver.client.codec;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public class EzySimpleSocketDataEncoder : EzySocketDataEncoder
	{
		private EzyObjectToByteEncoder encoder;

		public EzySimpleSocketDataEncoder(Object encoder)
		{
			this.encoder = (EzyObjectToByteEncoder)encoder;
		}

		public byte[] encode(Object data)
		{
			return encoder.encode(data);
		}

        public byte[] toMessageContent(Object data)
		{
			return encoder.toMessageContent(data);
		}

        public byte[] encryptMessageContent(byte[] messageContent, byte[] encryptionKey)
		{
			return encoder.encryptMessageContent(messageContent, encryptionKey);
		}
    }

}
