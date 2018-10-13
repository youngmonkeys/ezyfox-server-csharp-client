using System;
using System.Net.Sockets;
using System.Threading;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.io;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public class EzySocketReader : EzyAbstractSocketEventHandler
	{
		protected TcpClient socketChannel;
		protected readonly byte[] readBytes;
		protected readonly EzyByteBuffer readBuffer;
		protected readonly EzySocketDataHandler socketDataHandler;

		public EzySocketReader(EzySocketDataHandler socketDataHandler)
		{
			this.readBuffer = newReadBuffer();
			this.readBytes = new byte[getReadBufferSize()];
			this.socketDataHandler = socketDataHandler;
		}

		public override void handleEvent()
		{
			try
			{
				processSocketChannel();
				Thread.Sleep(3);
			}
			catch (Exception e)
			{
				Console.WriteLine("I/O error at socket-reader: " + e);
			}
		}

		private void processSocketChannel()
		{
			if (socketChannel == null)
				return;
			if (!socketChannel.Connected)
				return;
			int bytesToRead = socketChannel.GetStream().Read(readBytes, 0, getReadBufferSize());
			if (bytesToRead <= 0)
			{
				return;
			}
			readBuffer.clear();
			readBuffer.put(readBytes, 0, bytesToRead);
			readBuffer.flip();
			byte[] binary = readBuffer.getBytes(bytesToRead);
			socketDataHandler.fireBytesReceived(binary);
		}

		private void closeConnection()
		{
			socketChannel.Close();
			socketDataHandler.fireSocketDisconnected(EzyDisconnectReason.UNKNOWN);
		}

		public void setSocketChannel(TcpClient socketChannel)
		{
			this.socketChannel = socketChannel;
		}

		protected EzyByteBuffer newReadBuffer()
		{
			return EzyByteBuffer.allocate(getReadBufferSize());
		}

		protected int getReadBufferSize()
		{
			return EzySocketConstants.MAX_READ_BUFFER_SIZE;
		}

	}
}
