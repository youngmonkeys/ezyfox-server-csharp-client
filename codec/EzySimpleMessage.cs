using System;
using System.Text;

namespace com.tvd12.ezyfoxserver.client.codec
{
	public class EzySimpleMessage : EzyMessage
	{
		private int size;
		private byte[] content;
		private EzyMessageHeader header;
		private int byteCount;

		public void setSize(int size)
		{
			this.size = size;
		}

		public void setContent(byte[] content)
		{
			this.content = content;
		}

		public void setHeader(EzyMessageHeader header)
		{
			this.header = header;
		}

		public void countBytes()
		{
			this.byteCount = 1 + getSizeLength() + getContent().Length;
		}

		public EzyMessageHeader getHeader()
		{
			return header;
		}

		public byte[] getContent()
		{
			return content;
		}

		public int getSize()
		{
			return size;
		}

		public int getByteCount()
		{
			return byteCount;
		}

		public int getSizeLength()
		{
			return hasBigSize() ? 4 : 2;
		}

		public bool hasBigSize()
		{
			return getHeader().isBigSize();
		}

		public String toString()
		{
			return new StringBuilder()
						.Append("(")
					.Append("header: ")
						.Append(header)
						.Append(", ")
					.Append("size: ")
						.Append(size)
						.Append(", ")
					.Append("byteCount: ")
						.Append(byteCount)
						.Append(", ")
					.Append("content: ")
						.Append(String.Join(",", content))
					.Append(")")
					.ToString();
		}
	}
}
