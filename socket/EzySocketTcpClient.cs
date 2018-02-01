using System;
using System.Net.Sockets;
using com.tvd12.ezyfoxserver.client.codec;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public class EzySocketTcpClient : EzySocketClient
	{
		private TcpClient socket;

		protected override EzyCodecCreator newCodecCreator()
		{
			return new MsgPackCodecCreator();
		}

		protected override EzySocketReader newSocketReader()
		{
			return new EzySocketTcpReader();
		}

		protected override EzySocketWriter newSocketWriter()
		{
			return new EzySocketTcpWriter();
		}

		public override void connect()
		{
            socket = new TcpClient();
			((EzySocketTcpReader)socketReader).setSocket(socket);
			((EzySocketTcpWriter)socketWriter).setSocket(socket);
			socket.BeginConnect(host, port, new AsyncCallback(handleConnectionResult), socket);
		}

		private void handleConnectionResult(IAsyncResult result)
		{
			handleConnectionSuccess(result);
		}

		private void handleConnectionSuccess(IAsyncResult result)
		{
			socketReader.start();
			socketWriter.start();
			statusEventQueue.add(new EzySocketConnectedEvent());
		}
	}
}
