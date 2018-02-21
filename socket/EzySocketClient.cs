using System;
using com.tvd12.ezyfoxserver.client.util;
using com.tvd12.ezyfoxserver.client.codec;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.concurrent;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public abstract class EzySocketClient : EzyLoggable
	{
		protected readonly EzyCodecCreator codecCreator;
		protected readonly EzySocketReader socketReader;
		protected readonly EzySocketWriter socketWriter;
		protected readonly EzyBlockingQueue<EzyArray> ticketsQueue;
		protected readonly EzyQueue<EzySocketDataEvent> dataEventQueue;
		protected readonly EzyQueue<EzySocketStatusEvent> statusEventQueue;

		protected EzySocketDataEventHandler dataEventHandler;
		protected EzySocketStatusEventHandler statusEventHandler;

		public EzySocketClient()
		{
			this.ticketsQueue = newTicketsQueue();
			this.codecCreator = newCodecCreator();
			this.socketReader = newSocketReader();
			this.socketWriter = newSocketWriter();
			this.dataEventQueue = new EzyQueue<EzySocketDataEvent>();
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

		public abstract void connect(String host, int port);

		public virtual void processEvents()
		{
			processStatusEvents();
			processDataEvents();
		}

		public virtual void sendMessage(EzyArray msg)
		{
			ticketsQueue.offer(msg);
		}

		protected virtual void processDataEvents()
		{
			while (dataEventQueue.size() > 0)
			{
				processDataEvent(dataEventQueue.poll());
			}
		}

		protected virtual void processStatusEvents()
		{
			while (statusEventQueue.size() > 0)
			{
				processStatusEvent(statusEventQueue.poll());
			}
		}

		protected virtual void processDataEvent(EzySocketDataEvent dataEvent)
		{
			dataEventHandler.handle(dataEvent);

		}

		protected virtual void processStatusEvent(EzySocketStatusEvent statusEvent)
		{
			statusEventHandler.handle(statusEvent);
		}

		protected virtual EzyBlockingQueue<EzyArray> newTicketsQueue()
		{
			return new EzyBlockingQueue<EzyArray>();
		}

		public void setDataEventHandler(EzySocketDataEventHandler dataEventHandler)
		{
			this.dataEventHandler = dataEventHandler;
		}

		public void setStatusEventHanlder(EzySocketStatusEventHandler statusEventHandler)
		{
			this.statusEventHandler = statusEventHandler;
		}

	}
}
