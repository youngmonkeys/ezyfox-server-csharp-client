using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.io;
using static com.tvd12.ezyfoxserver.client.codec.EzyDecodeState;

namespace com.tvd12.ezyfoxserver.client.codec
{
	public class MsgPackByteToObjectDecoder : EzyByteToObjectDecoder
	{

		protected Handlers handlers;
		protected EzyMessageDeserializer deserializer;

		public MsgPackByteToObjectDecoder(
				EzyMessageDeserializer deserializer, int maxSize)
		{
			this.deserializer = deserializer;
			this.handlers = Handlers.builder()
					.setMaxSize(maxSize)
					.build();
		}

		public Object decode(EzyMessage message)
		{
			return deserializer.deserialize<Object>(message.getContent());
		}

		public void decode(EzyByteBuffer bytes, Queue<EzyMessage> queue)
		{
			handlers.handle(bytes, queue);
		}

		public void reset()
		{
			handlers.reset();
		}

	}

	public abstract class AbstractHandler : EzyDecodeHandler
	{

		protected EzyDecodeHandler nextDecodeHandler;
		protected EzyByteBufferMessageReader messageReader;

		public EzyDecodeHandler nextHandler()
		{
			return nextDecodeHandler;
		}

		public void setNextHandler(EzyDecodeHandler next)
		{
			this.nextDecodeHandler = next;
		}

		public void setMessageReader(EzyByteBufferMessageReader messageReader)
		{
			this.messageReader = messageReader;
		}

		public abstract int nextState();

		public abstract bool handle(EzyByteBuffer input, Queue<EzyMessage> output);
	}

	public class PrepareMessage : AbstractHandler
	{

		public override int nextState()
		{
			return READ_MESSAGE_HEADER;
		}

		public override bool handle(EzyByteBuffer input, Queue<EzyMessage> queue)
		{
			messageReader.clear();
			return true;
		}
	}

	public class ReadMessageHeader : AbstractHandler
	{

		public override int nextState()
		{
			return READ_MESSAGE_SIZE;
		}

		public override bool handle(EzyByteBuffer input, Queue<EzyMessage> queue)
		{
			return messageReader.readHeader(input);
		}

	}

	public class ReadMessageSize : AbstractHandler
	{

		protected readonly int maxSize;

		public ReadMessageSize(int maxSize)
		{
			this.maxSize = maxSize;
		}

		public override int nextState()
		{
			return READ_MESSAGE_CONTENT;
		}

		public override bool handle(EzyByteBuffer input, Queue<EzyMessage> queue)
		{
			return messageReader.readSize(input, maxSize);
		}
	}

	public class ReadMessageContent : AbstractHandler
	{

		public override int nextState()
		{
			return PREPARE_MESSAGE;
		}

		public override bool handle(EzyByteBuffer input, Queue<EzyMessage> queue)
		{
			if (!messageReader.readContent(input))
				return false;
			queue.Enqueue(messageReader.get());
			return true;
		}

	}

	public class Handlers : EzyDecodeHandlers
	{

		public Handlers(AbstractBuilder builder) : base(builder)
		{
		}

		public static Builder builder()
		{
			return new Builder();
		}

		public class Builder : EzyDecodeHandlers.AbstractBuilder
		{
			protected int maxSize;
			protected EzyByteBufferMessageReader messageReader = new EzyByteBufferMessageReader();

			public Builder setMaxSize(int maxSize)
			{
				this.maxSize = maxSize;
				return this;
			}

			public Handlers build()
			{
				return new Handlers(this);
			}

			protected override void addHandlers(
				IDictionary<int, EzyDecodeHandler> answer)
			{
				EzyDecodeHandler readMessgeHeader = new ReadMessageHeader();
				EzyDecodeHandler prepareMessage = new PrepareMessage();
				EzyDecodeHandler readMessageSize = new ReadMessageSize(maxSize);
				EzyDecodeHandler readMessageContent = new ReadMessageContent();
				answer[PREPARE_MESSAGE] = newHandler(prepareMessage, readMessgeHeader);
				answer[READ_MESSAGE_HEADER] = newHandler(readMessgeHeader, readMessageSize);
				answer[READ_MESSAGE_SIZE] = newHandler(readMessageSize, readMessageContent);
				answer[READ_MESSAGE_CONTENT] = newHandler(readMessageContent);
			}


			private EzyDecodeHandler newHandler(EzyDecodeHandler handler)
			{
				return newHandler(handler, null);
			}

			private EzyDecodeHandler newHandler(EzyDecodeHandler handler, EzyDecodeHandler next)
			{
				return newHandler((AbstractHandler)handler, next);
			}

			private EzyDecodeHandler newHandler(AbstractHandler handler, EzyDecodeHandler next)
			{
				handler.setNextHandler(next);
				handler.setMessageReader(messageReader);
				return handler;
			}
		}
	}

}
