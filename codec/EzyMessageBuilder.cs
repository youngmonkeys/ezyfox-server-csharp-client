using System;

namespace com.tvd12.ezyfoxserver.client.codec
{
	public class EzyMessageBuilder
	{
		private int size;
		private byte[] content;
		private EzyMessageHeader header;

		public static EzyMessageBuilder newInstance()
		{
			return new EzyMessageBuilder();
		}

		public EzyMessageBuilder setSize(int size)
		{
			this.size = size;
			return this;
		}

		public EzyMessageBuilder setContent(byte[] content)
		{
			this.content = content;
			return this;
		}

		public EzyMessageBuilder setHeader(EzyMessageHeader header)
		{
			this.header = header;
			return this;
		}

		public EzyMessageBuilder setHeader(EzyMessageHeaderBuilder buider)
		{
			return setHeader(buider.build());
		}

		public EzyMessage build()
		{
			EzySimpleMessage answer = new EzySimpleMessage();
			answer.setSize(size);
			answer.setHeader(header);
			answer.setContent(content);
			answer.countBytes();
			return answer;
		}
	}
}
