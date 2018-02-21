using System;
using System.Net.Sockets;
using com.tvd12.ezyfoxserver.client.handler;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.concurrent;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public class EzySocketTcpWriter : EzySocketWriter
	{
		protected TcpClient socket;
		protected EzyObjectToByteEncoder encoder;

		protected override void writeBytes(Object bytes)
		{
			byte[] buffer = (byte[])bytes;
			NetworkStream stream = socket.GetStream();
			stream.Write(buffer, 0, buffer.Length);
			stream.Flush();
		}

		protected override Object encodeData(EzyArray data)
		{
			return encoder.encode(data);
		}

		public void setSocket(TcpClient socket)
		{
			this.socket = socket;
		}

		public override void setEncoder(Object encoder)
		{
			this.encoder = (EzyObjectToByteEncoder)encoder;
		}
	}
}
