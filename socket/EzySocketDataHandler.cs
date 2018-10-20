using System;
using System.Net.Sockets;
using System.Threading;
using com.tvd12.ezyfoxserver.client.callback;
using com.tvd12.ezyfoxserver.client.codec;
using com.tvd12.ezyfoxserver.client.concurrent;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.evt;
using com.tvd12.ezyfoxserver.client.util;

namespace com.tvd12.ezyfoxserver.client.socket
{
    public class EzySocketDataHandler : EzyLoggable, EzyResettable
	{
		protected TcpClient socketChannel;
		protected volatile bool disconnected;
		protected readonly EzySocketDataDecoder decoder;
		protected readonly EzySocketEventQueue eventQueue;
		protected readonly EzyDecodeMessageThread decodeMessageThread;
		protected readonly EzyCallback<EzyMessage> decodeBytesCallback;
		protected readonly EzyDisconnectionDelegate disconnectionDelegate;

		public EzySocketDataHandler(EzySocketDataDecoder decoder,
									EzySocketEventQueue eventQueue,
									EzyDisconnectionDelegate disconnectionDelegate)
		{
			this.decoder = decoder;
			this.eventQueue = eventQueue;
			this.disconnectionDelegate = disconnectionDelegate;
			this.decodeBytesCallback = message => handleReceivedMesssage(message);
			this.decodeMessageThread = new EzyDecodeMessageThread(decoder,
																  data => handleReceivedData(data),
																  ex => fireExceptionCaught(ex));
            this.decodeMessageThread.start();
		}

		public void setDisconnected(bool disconnected)
		{
			this.disconnected = disconnected;
		}

		public void setSocketChannel(TcpClient socketChannel)
		{
			this.socketChannel = socketChannel;
		}

		public void fireBytesReceived(byte[] bytes)
		{
			try
			{
				decoder.decode(bytes, decodeBytesCallback);
			}
			catch (Exception throwable)
			{
				fireExceptionCaught(throwable);
			}
		}

		public void fireSocketDisconnected(EzyDisconnectReason reason)
		{
			if (disconnected)
				return;
			disconnected = true;
			disconnectionDelegate.onDisconnected(reason);
			EzyEvent evt = new EzyDisconnectionEvent(reason);
			EzySocketEvent socketEvent = new EzySimpleSocketEvent(
					EzySocketEventType.EVENT, evt);
			fireSocketEvent(socketEvent);
		}

		public void fireSocketEvent(EzySocketEvent socketEvent)
		{
			eventQueue.add(socketEvent);
		}

		private void fireExceptionCaught(Exception e)
		{
            logger.error("exeception caught", e);
		}

		private void handleReceivedMesssage(EzyMessage message)
		{
			decodeMessageThread.addMessage(message);
		}

		private void handleReceivedData(Object data)
		{
			EzyResponse reponse = newSocketResponse(data);
			EzySocketEvent evt = new EzySimpleSocketEvent(EzySocketEventType.RESPONSE, reponse);
			bool success = eventQueue.add(evt);
			if (!success)
			{
                logger.warn("response queue is full, drop incomming response");
			}
		}

		private EzyResponse newSocketResponse(Object data)
		{
			return new EzySimpleResponse((EzyArray)data);
		}

		public void firePacketSend(EzyPacket packet)
		{
			bool can = canWriteBytes();
			if (can)
				writePacketToSocket(packet);
			else
				packet.release();
		}

		private bool canWriteBytes()
		{
			if (socketChannel == null)
				return false;
			return socketChannel.Connected;
		}

		protected void writePacketToSocket(EzyPacket packet)
		{
			byte[] buffer = (byte[])packet.getData();
			NetworkStream stream = socketChannel.GetStream();
			stream.Write(buffer, 0, buffer.Length);
			stream.Flush();
		}

		public void reset()
		{
			decoder.reset();
			decodeMessageThread.reset();
		}
	}

	public class EzyDecodeMessageThread : EzyStartable, EzyStoppable, EzyResettable
	{
		private volatile bool active;
		private readonly Thread thread;
		private readonly EzySocketDataDecoder decoder;
		private readonly EzyCallback<Object> dataCallback;
		private readonly EzyCallback<Exception> exceptionCallback;
		private readonly EzyBlockingQueue<EzyMessage> messageQueue;

		public EzyDecodeMessageThread(EzySocketDataDecoder decoder,
									  EzyCallback<Object> dataCallback,
									  EzyCallback<Exception> exceptionCallback)
		{
			this.decoder = decoder;
			this.dataCallback = dataCallback;
			this.exceptionCallback = exceptionCallback;
			this.messageQueue = new EzyBlockingQueue<EzyMessage>();
			this.thread = new Thread(loop);
			this.thread.Name = "decode";
		}

		public void addMessage(EzyMessage message)
		{
			this.messageQueue.add(message);
		}

		public void start()
		{
			thread.Start();
		}

		private void loop()
		{
			this.active = true;
			while (active)
			{
				handle();
			}
		}

		private void handle()
		{
			try
			{
				EzyMessage message = messageQueue.take();
				Object data = decoder.decode(message);
				dataCallback(data);
			}
			catch (Exception ex)
			{
				exceptionCallback(ex);
			}
		}

		public void reset()
		{
			this.messageQueue.clear();
		}

		public void stop()
		{
			active = false;
		}
	}
}
