using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.callback;
using com.tvd12.ezyfoxserver.client.codec;
using com.tvd12.ezyfoxserver.client.io;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public class EzySimpleSocketDataDecoder : EzySocketDataDecoder
	{

		protected EzyByteBuffer buffer;
		protected volatile bool active;
		private readonly Queue<EzyMessage> queue;
		private readonly EzyByteToObjectDecoder decoder;

		public EzySimpleSocketDataDecoder(Object decoder)
		{
			this.active = true;
			this.queue = new Queue<EzyMessage>();
			this.decoder = (EzyByteToObjectDecoder)decoder;
		}

		public Object decode(EzyMessage message, byte[] decryptionKey)
		{
			Object answer = decoder.decode(message, decryptionKey);
			return answer;
		}

		public void decode(
				byte[] bytes, EzyCallback<EzyMessage> callback)
		{
			predecode(bytes);
			decoder.decode(buffer, queue);
			handleQueue(callback);
			postdecode();
		}

		private void handleQueue(EzyCallback<EzyMessage> callback)
		{
			while (queue.Count > 0 && active)
			{
				do
				{
					EzyMessage message = queue.Dequeue();
					callback(message);
				}
				while (queue.Count > 0);

				if (buffer.hasRemaining())
				{
					decoder.decode(buffer, queue);
				}
			}
		}

		private void predecode(byte[] bytes)
		{
			if (buffer == null)
				buffer = newBuffer(bytes);
			else
				buffer = mergeBytes(bytes);
		}

		private void postdecode()
		{
			buffer = getRemainBytes(buffer);
		}

		private EzyByteBuffer newBuffer(byte[] bytes)
		{
			return EzyByteBuffer.wrap(bytes);
		}

		private EzyByteBuffer mergeBytes(byte[] bytes)
		{
			int capacity = buffer.remaining() + bytes.Length;
			EzyByteBuffer merge = EzyByteBuffer.allocate(capacity);
			merge.put(buffer);
			merge.put(bytes);
			merge.flip();
			return merge;
		}

		private EzyByteBuffer getRemainBytes(EzyByteBuffer old)
		{
			if (!old.hasRemaining())
				return null;
			byte[] bytes = new byte[old.remaining()];
			old.get(bytes);
			return EzyByteBuffer.wrap(bytes);
		}

	}
}
