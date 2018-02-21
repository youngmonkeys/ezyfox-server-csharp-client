using System;
using com.tvd12.ezyfoxserver.client.handler;

namespace com.tvd12.ezyfoxserver.client.codec
{
	public class MsgPackObjectToByteEncoder : EzyObjectToByteEncoder
	{
		protected readonly EzyMessageToBytes messageToBytes;
		protected readonly EzyObjectToMessage objectToMessage;

		public MsgPackObjectToByteEncoder(
				EzyMessageToBytes messageToBytes,
				EzyObjectToMessage objectToMessage)
		{
			this.messageToBytes = messageToBytes;
			this.objectToMessage = objectToMessage;
		}

		public byte[] encode(Object msg)
		{
			return convertObjectToBytes(msg);
		}

		protected byte[] convertObjectToBytes(Object obj)
		{
			return convertMessageToBytes(convertObjectToMessage(obj));
		}

		protected EzyMessage convertObjectToMessage(Object obj)
		{
			return objectToMessage.convert(obj);
		}

		protected byte[] convertMessageToBytes(EzyMessage message)
		{
			return messageToBytes.convert(message);
		}
	}
}
