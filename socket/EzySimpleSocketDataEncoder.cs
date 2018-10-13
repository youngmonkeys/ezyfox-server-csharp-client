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
			byte[] bytes = encoder.encode(data);
			return bytes;
		}

	}

}
