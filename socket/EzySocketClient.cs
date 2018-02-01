using System;
using com.tvd12.ezyfoxserver.client.util;
using com.tvd12.ezyfoxserver.client.codec;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.concurrent;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public abstract class EzySocketClient
	{
		protected String host;
		protected int port;
		protected readonly EzyCodecCreator codecCreator;
		protected readonly EzySocketReader socketReader;
		protected readonly EzySocketWriter socketWriter;
		protected readonly EzyBlockingQueue<EzyArray> ticketsQueue;
		protected readonly EzyQueue<EzySocketDataEvent> dataEventQueue;
		protected readonly EzyQueue<EzySocketStatusEvent> statusEventQueue;

		public EzySocketClient()
		{
			this.ticketsQueue = newTicketsQueue();
			this.codecCreator = newCodecCreator();
			this.socketReader = newSocketReader();
			this.socketWriter = newSocketWriter();
			this.statusEventQueue = new EzyQueue<EzySocketStatusEvent>();
			this.socketReader.setDataEventQueue(dataEventQueue);
			this.socketReader.setStatusEventQueue(statusEventQueue);
			this.socketReader.setDecoder(codecCreator.newDecoder(Int32.MaxValue));
            this.socketWriter.setTicketsQueue(ticketsQueue);
			this.socketWriter.setEncoder(codecCreator.newEncoder());
		}

		protected abstract EzyCodecCreator newCodecCreator();
		protected abstract EzySocketReader newSocketReader();
		protected abstract EzySocketWriter newSocketWriter();

		public abstract void connect();

		protected virtual EzyBlockingQueue<EzyArray> newTicketsQueue()
		{
			return new EzyBlockingQueue<EzyArray>();
		}
	}
}
