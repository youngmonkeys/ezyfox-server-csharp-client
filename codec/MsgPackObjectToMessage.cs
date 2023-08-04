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
            return this.packToMessage(this.convertToMessageContent(value), false);
        }

        public byte[] convertToMessageContent(Object value)
        {
            return this.objectToBytes.convert(value);
        }

        public EzyMessage packToMessage(byte[] content, bool encrypted)
        {
            return new EzySimpleMessage(this.newHeader(content, encrypted), content, content.Length);
        }

        private EzyMessageHeader newHeader(byte[] content, bool encrypted)
		{
            return new EzySimpleMessageHeader(isBigMessage(content), encrypted, false, false, false, false);
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
