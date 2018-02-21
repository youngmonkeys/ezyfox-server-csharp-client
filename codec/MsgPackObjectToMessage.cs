using System;

namespace com.tvd12.ezyfoxserver.client.codec
{
	public class MsgPackObjectToMessage : EzyObjectToMessage
	{
		private EzyObjectToBytes objectToBytes;

		public MsgPackObjectToMessage(MsgPackObjectToMessageBuilder builder)
		{
			this.objectToBytes = builder.newObjectToBytes();
		}

		public EzyMessage convert(Object value)
		{
			return convert(convertObject(value));
		}

		private byte[] convertObject(Object value)
		{
			return objectToBytes.convert(value);
		}

		private EzyMessage convert(byte[] content)
		{
			return EzyMessageBuilder.newInstance()
					.setSize(content.Length)
					.setContent(content)
					.setHeader(newHeader(content))
					.build();
		}

		private EzyMessageHeader newHeader(byte[] content)
		{
			return EzyMessageHeaderBuilder.newInstance()
					.setBigSize(isBigMessage(content))
					.setEncrypted(false)
					.setCompressed(false)
					.build();
		}

		private bool isBigMessage(byte[] content)
		{
			return content.Length > MsgPackConstant.MAX_SMALL_MESSAGE_SIZE;
		}

		public static MsgPackObjectToMessageBuilder builder()
		{
			return new MsgPackObjectToMessageBuilder();
		}
	}

	public class MsgPackObjectToMessageBuilder
	{

		public EzyObjectToMessage build()
		{
			return new MsgPackObjectToMessage(this);
		}

		public EzyObjectToBytes newObjectToBytes()
		{
			return MsgPackObjectToBytes.builder().setSerializer(newSerializer()).build();
		}

		protected EzyMessageSerializer newSerializer()
		{
			return new MsgPackSimpleSerializer();
		}
	}
}
