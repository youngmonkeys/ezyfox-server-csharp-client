using System;

namespace com.tvd12.ezyfoxserver.client.codec
{
	public class MsgPackCodecCreator : EzyCodecCreator
	{
		protected readonly EzyMessageToBytes messageToBytes 
			= EzySimpleMessageToBytes.builder().build();
		protected readonly EzyObjectToMessage objectToMessage 
			= MsgPackObjectToMessage.builder().build();
		protected readonly EzyMessageDeserializer deserializer 
			= new MsgPackSimpleDeserializer();

		public Object newDecoder(int maxRequestSize)
		{
			return new MsgPackByteToObjectDecoder(deserializer, maxRequestSize);
		}

		public Object newEncoder()
		{
			return new MsgPackObjectToByteEncoder(messageToBytes, objectToMessage);
		}

	}
}
