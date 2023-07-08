using System;
using com.tvd12.ezyfoxserver.client.security;

namespace com.tvd12.ezyfoxserver.client.codec
{
	public class MsgPackObjectToByteEncoder : EzyObjectToByteEncoder
	{
		protected readonly EzyAesCrypt cryptor;
		protected readonly EzyMessageToBytes messageToBytes;
		protected readonly EzyObjectToMessage objectToMessage;

		public MsgPackObjectToByteEncoder(
			EzyMessageToBytes messageToBytes,
			EzyObjectToMessage objectToMessage
		)
		{
			this.messageToBytes = messageToBytes;
			this.objectToMessage = objectToMessage;
			this.cryptor = EzyAesCrypt.getDefault();
		}

		public byte[] encode(Object msg)
		{
			return convertObjectToBytes(msg);
		}

        public byte[] toMessageContent(Object data)
        {
            return this.objectToMessage.convertToMessageContent(data);
        }

        public byte[] encryptMessageContent(byte[] messageContent, byte[] encryptionKey)
        {
            EzyMessage message;
			if (encryptionKey != null) {
				message = this.objectToMessage.packToMessage(this.doEncrypt(messageContent, encryptionKey), true);
			} else {
				message = this.objectToMessage.packToMessage(messageContent, false);
			}

			return this.convertMessageToBytes(message);
		}

		protected byte[] doEncrypt(byte[] messageContent, byte[] encryptionKey)
		{
			return cryptor.encrypt(messageContent, encryptionKey);
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
