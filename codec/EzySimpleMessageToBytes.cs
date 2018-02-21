using com.tvd12.ezyfoxserver.client.io;
using com.tvd12.ezyfoxserver.client.builder;

namespace com.tvd12.ezyfoxserver.client.codec
{
	public class EzySimpleMessageToBytes : EzyMessageToBytes
	{

		public EzySimpleMessageToBytes(Builder builder)
		{
		}

		public byte[] convert(EzyMessage message)
		{
			EzyByteBuffer buffer = newByteBuffer(message);
			writeHeader(buffer, message);
			writeSize(buffer, message);
			writeContent(buffer, message);
			byte[] answer = new byte[buffer.position()];
			buffer.flip();
			buffer.get(answer);
			return answer;
		}

		private void writeHeader(EzyByteBuffer buffer, EzyMessage message)
		{
			writeHeader(buffer, message.getHeader());
		}

		private void writeHeader(EzyByteBuffer buffer, EzyMessageHeader header)
		{
			byte headerByte = 0;
			headerByte |= (byte)(header.isBigSize() ? 1 << 0 : 0);
			headerByte |= (byte)(header.isEncrypted() ? 1 << 1 : 0);
			headerByte |= (byte)(header.isCompressed() ? 1 << 2 : 0);
			headerByte |= (byte)(header.isText() ? 1 << 3 : 0);
			buffer.put(headerByte);
		}

		private void writeSize(EzyByteBuffer buffer, EzyMessage message)
		{
			if (message.hasBigSize())
				buffer.putInt(message.getSize());
			else
				buffer.putShort((short)message.getSize());
		}

		private void writeContent(EzyByteBuffer buffer, EzyMessage message)
		{
			buffer.put(message.getContent());
		}

		private EzyByteBuffer newByteBuffer(EzyMessage message)
		{
			int capacity = getCapacity(message);
			return EzyByteBuffer.allocate(capacity);
		}

		private int getCapacity(EzyMessage message)
		{
			return message.getByteCount();
		}

		public static Builder builder()
		{
			return new Builder();
		}

		public class Builder : EzyBuilder<EzyMessageToBytes>
		{

			public EzyMessageToBytes build()
			{
				return new EzySimpleMessageToBytes(this);
			}

		}
	}

}
