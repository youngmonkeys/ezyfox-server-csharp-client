using System;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.io;
using com.tvd12.ezyfoxserver.client.util;
using com.tvd12.ezyfoxserver.client.codec;
using com.tvd12.ezyfoxserver.client.handler;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public class EzySocketTcpReader : EzySocketReader
	{
		protected TcpClient socket;
		protected readonly byte[] readBytes;
		protected EzyByteToObjectDecoder decoder;
		protected readonly EzyByteBuffer readBuffer;
		protected readonly Queue<EzyMessage> messageQueue;
		protected const int READ_BUFFER_SIZE = 102400;

		public EzySocketTcpReader() : base()
		{
			this.readBuffer = newReadBuffer();
			this.readBytes = new byte[getReadBufferSize()];
			this.messageQueue = new Queue<EzyMessage>();
		}

		protected override void readSocketData()
		{
			int bytesToRead = socket.GetStream().Read(readBytes, 0, getReadBufferSize());
			if (bytesToRead <= 0)
			{
				return;
			}
			readBuffer.clear();
			readBuffer.put(readBytes, 0, bytesToRead);
			decoder.decode(readBuffer, messageQueue);
			if (messageQueue.Count > 0)
			{
				EzyMessage message = messageQueue.Dequeue();
				Object socketData = decoder.decode(message);
				getLogger().debug("receiver data: " + socketData);
				dataEventQueue.add(new EzySocketDataEvent(socketData));
			}
		}

		protected EzyByteBuffer newReadBuffer()
		{
			return EzyByteBuffer.allocate(getReadBufferSize());
		}

		protected int getReadBufferSize()
		{
			return READ_BUFFER_SIZE;
		}

		public void setSocket(TcpClient socket)
		{
			this.socket = socket;
		}

		public override void setDecoder(Object decoder)
		{
			this.decoder = (EzyByteToObjectDecoder)decoder;
		}
	}
}
