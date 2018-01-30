using System;
using System.Net.Sockets;
using com.tvd12.ezyfoxserver.client.io;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public class EzySocketReader
	{
		protected readonly EzyByteBuffer readBuffer;

		public EzySocketReader()
		{
			this.readBuffer = newReadBuffer();
		}

		public void handle(IAsyncResult result)
		{
			StateObject state = (StateObject)result.AsyncState;
			Socket socket = state.workSocket;
			int bytesToRead = socket.EndReceive(result);
			if (bytesToRead > 0)
			{
				byte[] socketBuffer = state.buffer;
				readBuffer.clear();
				readBuffer.put(socketBuffer, 0, bytesToRead);
			}
		}

		protected EzyByteBuffer newReadBuffer()
		{
			return new EzyByteBuffer();
		}
	}
}
