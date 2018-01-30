using System;

namespace com.tvd12.ezyfoxserver.client.codec
{
	public class EzyMessageHeaderBuilder
	{
		protected bool bigSize;
		protected bool encrypted;
		protected bool compressed;
		protected bool text;

		public static EzyMessageHeaderBuilder newInstance()
		{
			return new EzyMessageHeaderBuilder();
		}

		public static EzyMessageHeaderBuilder messageHeaderBuilder()
		{
			return new EzyMessageHeaderBuilder();
		}

		public EzyMessageHeaderBuilder setBigSize(bool bigSize)
		{
			this.bigSize = bigSize;
			return this;
		}

		public EzyMessageHeaderBuilder setEncrypted(bool encrypted)
		{
			this.encrypted = encrypted;
			return this;
		}

		public EzyMessageHeaderBuilder setCompressed(bool compressed)
		{
			this.compressed = compressed;
			return this;
		}

		public EzyMessageHeaderBuilder setText(bool text)
		{
			this.text = text;
			return this;
		}

		public EzyMessageHeader build()
		{
			EzySimpleMessageHeader header = new EzySimpleMessageHeader();
			header.setBigSize(bigSize);
			header.setEncrypted(encrypted);
			header.setCompressed(compressed);
			header.setText(text);
			return header;
		}
	}
}
