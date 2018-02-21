using com.tvd12.ezyfoxserver.client.io;

namespace com.tvd12.ezyfoxserver.client.codec
{
	public class EzyByteBufferMessageReader : EzyMessageReader<EzyByteBuffer>
	{
		protected override byte readByte(EzyByteBuffer buffer)
		{
			return buffer.get();
		}

		protected override int remaining(EzyByteBuffer buffer)
		{
			return buffer.remaining();
		}

		protected override int readMessgeSize(EzyByteBuffer buffer)
		{
			return buffer.getUInt(getSizeLength());
		}

		protected override void readMessageContent(EzyByteBuffer buffer, byte[] content)
		{
			buffer.get(content);
		}
	}
}
