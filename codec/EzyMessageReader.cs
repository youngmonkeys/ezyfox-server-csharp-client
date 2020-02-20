using System;
using com.tvd12.ezyfoxserver.client.exception;

namespace com.tvd12.ezyfoxserver.client.codec
{
	public abstract class EzyMessageReader<B>
	{
		private int size;
		private byte[] content;
		private EzyMessageHeader header;

		public EzyMessageReader()
		{
			clear();
		}

		protected abstract int remaining(B buffer);
		protected abstract byte readByte(B buffer);
		protected abstract int readMessgeSize(B buffer);
		protected abstract void readMessageContent(B buffer, byte[] content);

		public bool readHeader(B buffer)
		{
			int remain = remaining(buffer);
			if (remain < getHeaderLength())
				return false;
			byte headerByte = readByte(buffer);
			readHeader(headerByte);
			return true;
		}

		public bool readSize(B buffer, int maxSize)
		{
			int remain = remaining(buffer);
			if (remain < getSizeLength())
				return false;
			this.size = readMessgeSize(buffer);
			if (size > maxSize)
				throw new EzyMaxRequestSizeException(size, maxSize);
			return true;
		}

		public bool readContent(B buffer)
		{
			int remain = remaining(buffer);
			if (remain < size)
				return false;
			this.content = new byte[size];
			readMessageContent(buffer, content);
			return true;
		}

		public void clear()
		{
			this.size = 0;
			this.content = new byte[0];
		}

		public EzyMessage get()
		{
            return new EzySimpleMessage(header, content, size);
		}

		private void readHeader(byte headerByte)
		{
            this.header = EzyMessageHeaderReader.read(headerByte);
		}

		protected int getSizeLength()
		{
			return header.isBigSize() ? 4 : 2;
		}

		protected int getHeaderLength()
		{
			return 1;
		}
	}
}
