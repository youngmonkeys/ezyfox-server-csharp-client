using System;
using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public class EzySocketResponseApi : EzyAbstractResponseApi
	{
		protected readonly EzySocketDataEncoder encoder;

		public EzySocketResponseApi(EzySocketDataEncoder encoder,
									EzyPacketQueue packetQueue)
			: base(packetQueue)
		{
			this.encoder = encoder;
		}

		protected override Object encodeData(EzyArray data)
		{
			Object answer = encoder.encode(data);
			return answer;
		}
	}
}
