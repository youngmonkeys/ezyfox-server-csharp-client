using com.tvd12.ezyfoxserver.client.handler;

namespace com.tvd12.ezyfoxserver.client.codec
{
	public class MsgPackCodecCreator
	{
		protected readonly EzyMessageToBytes messageToBytes 
			= EzySimpleMessageToBytes.builder().build();
		protected readonly EzyObjectToMessage objectToMessage 
			= MsgPackObjectToMessage.builder().build();
		protected readonly EzyMessageDeserializer deserializer 
			= new MsgPackSimpleDeserializer();

		public EzyNioByteToObjectDecoder newDecoder(int maxRequestSize)
		{
			return new MsgPackByteToObjectDecoder(deserializer, maxRequestSize);
		}

		public EzyNioObjectToByteEncoder newEncoder()
		{
			return new MsgPackObjectToByteEncoder(messageToBytes, objectToMessage);
		}

	}
}
