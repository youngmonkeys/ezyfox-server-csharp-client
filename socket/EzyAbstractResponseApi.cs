using System;
using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public abstract class EzyAbstractResponseApi : EzyResponseApi
	{
		private readonly EzyPacketQueue packetQueue;

		public EzyAbstractResponseApi(EzyPacketQueue packetQueue)
		{
			this.packetQueue = packetQueue;
		}

		public void response(EzyPackage pack)
		{
			Object bytes;
			EzyArray data = pack.getData();
			if (pack.isEncrypted())
			{
				bytes = encryptMessageContent(
					dataToMessageContent(data),
					pack.getEncryptionKey()
				);
			}
			else
			{
				bytes = encodeData(data);
			}

			EzyPacket packet = createPacket(bytes, pack);
			packetQueue.add(packet);
		}

		private EzyPacket createPacket(Object bytes, EzyPackage pack)
		{
			EzySimplePacket packet = new EzySimplePacket();
			packet.setTransportType(pack.getTransportType());
			packet.setData(bytes);
			return packet;
		}

		protected abstract Object encodeData(EzyArray data);

		protected abstract byte[] dataToMessageContent(EzyArray data);

		protected abstract byte[] encryptMessageContent(
			byte[] messageContent,
			byte[] encryptionKey
		);
    }
}
