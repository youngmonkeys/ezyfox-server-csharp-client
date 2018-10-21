using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using com.tvd12.ezyfoxserver.client.codec;
using com.tvd12.ezyfoxserver.client.config;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.evt;
using com.tvd12.ezyfoxserver.client.factory;
using com.tvd12.ezyfoxserver.client.manager;
using com.tvd12.ezyfoxserver.client.net;
using com.tvd12.ezyfoxserver.client.request;
using com.tvd12.ezyfoxserver.client.util;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public class EzyTcpSocketClient :
		EzyAbstractSocketClient,
		EzyDisconnectionDelegate
	{
		protected int reconnectCount;
		protected DateTime startConnectTime;
		protected TcpClient socketChannel;
		protected SocketAddress socketAddress;
		protected EzySocketThread socketThread;
		protected readonly EzyReconnectConfig reconnectConfig;
		protected readonly EzyMainThreadQueue mainThreadQueue;
		protected readonly EzyHandlerManager handlerManager;
		protected readonly ISet<Object> unloggableCommands;
		protected readonly EzyCodecFactory codecFactory;
		protected readonly EzyPacketQueue packetQueue;
		protected readonly EzySocketEventQueue eventQueue;
		protected readonly EzyResponseApi responseApi;
		protected readonly EzySocketDataHandler dataHandler;
		protected readonly EzyPingSchedule pingSchedule;
		protected readonly EzyPingManager pingManager;
		protected readonly EzySocketReader socketReader;
		protected readonly EzySocketWriter socketWriter;
		protected readonly EzySocketDataEventHandler socketDataEventHandler;
		protected readonly EzySocketReadingLoopHandler socketReadingLoopHandler;
		protected readonly EzySocketWritingLoopHandler socketWritingLoopHandler;
        protected readonly EzySocketDataEventLoopHandler socketDataEventLoopHandler;

		public EzyTcpSocketClient(EzyClientConfig clientConfig,
								  EzyMainThreadQueue mainThreadQueue,
								  EzyHandlerManager handlerManager,
								  EzyPingManager pingManager,
								  EzyPingSchedule pingSchedule,
								  ISet<Object> unloggableCommands)
		{
			this.reconnectConfig = clientConfig.getReconnect();
			this.mainThreadQueue = mainThreadQueue;
			this.handlerManager = handlerManager;
			this.pingManager = pingManager;
			this.pingSchedule = pingSchedule;
			this.unloggableCommands = unloggableCommands;
			this.codecFactory = new EzySimpleCodecFactory();
			this.packetQueue = new EzyBlockingPacketQueue();
			this.eventQueue = new EzyLinkedBlockingEventQueue();
			this.responseApi = newResponseApi();
			this.dataHandler = newSocketDataHandler();
			this.socketReader = new EzySocketReader(dataHandler);
			this.socketWriter = new EzySocketWriter(packetQueue, dataHandler);
			this.socketDataEventHandler = newSocketDataEventHandler();
			this.socketReadingLoopHandler = newSocketReadingLoopHandler();
			this.socketWritingLoopHandler = newSocketWritingLoopHandler();
            this.socketDataEventLoopHandler = newSocketDataEventLoopHandler();
			this.pingSchedule.setDataHandler(dataHandler);
			this.startComponents();
		}

		private void startComponents()
		{
			this.socketReadingLoopHandler.start();
			this.socketWritingLoopHandler.start();
            this.socketDataEventLoopHandler.start();
		}

		private EzyResponseApi newResponseApi()
		{
			Object encoder = codecFactory.newEncoder(EzyConnectionType.SOCKET);
			EzySocketDataEncoder socketDataEncoder = new EzySimpleSocketDataEncoder(encoder);
			EzyResponseApi api = new EzySocketResponseApi(socketDataEncoder, packetQueue);
			return api;
		}

		private EzySocketDataHandler newSocketDataHandler()
		{
			Object decoder = codecFactory.newDecoder(EzyConnectionType.SOCKET);
			EzySocketDataDecoder socketDataDecoder = new EzySimpleSocketDataDecoder(decoder);
			return new EzySocketDataHandler(socketDataDecoder, eventQueue, this);
		}

		private EzySocketDataEventHandler newSocketDataEventHandler()
		{
			return new EzySocketDataEventHandler(
					mainThreadQueue,
					dataHandler,
					pingManager,
					handlerManager,
					eventQueue, unloggableCommands);
		}

		private EzySocketReadingLoopHandler newSocketReadingLoopHandler()
		{
			EzySocketReadingLoopHandler handler = new EzySocketReadingLoopHandler();
			handler.setEventHandler(socketReader);
			return handler;
		}

		private EzySocketWritingLoopHandler newSocketWritingLoopHandler()
		{
			EzySocketWritingLoopHandler handler = new EzySocketWritingLoopHandler();
			handler.setEventHandler(socketWriter);
			return handler;
		}

        private EzySocketDataEventLoopHandler newSocketDataEventLoopHandler()
        {
            EzySocketDataEventLoopHandler handler = new EzySocketDataEventLoopHandler();
            handler.setEventHandler(socketDataEventHandler);
            return handler;
        }

		public override void connect(String host, int port)
		{
			socketAddress = new InetSocketAddress(host, port);
			connect();
		}

		public override void connect()
		{
			reconnectCount = 0;
			handleConnection(0);
		}

		public override bool reconnect()
		{
			int maxReconnectCount = reconnectConfig.getMaxReconnectCount();
			if (reconnectCount >= maxReconnectCount)
				return false;
			long reconnectSleepTime = getReconnectSleepTime();
			handleConnection(reconnectSleepTime);
			reconnectCount++;
            logger.info("try reconnect to server: " + reconnectCount + ", wating time: " + reconnectSleepTime);
			EzyEvent tryConnectEvent = new EzyTryConnectEvent(reconnectCount);
			EzySocketEvent tryConnectSocketEvent
					= new EzySimpleSocketEvent(EzySocketEventType.EVENT, tryConnectEvent);
			dataHandler.fireSocketEvent(tryConnectSocketEvent);
			return true;
		}

		private void handleConnection(long sleepTime)
		{
			if (socketThread != null)
				socketThread.cancel();
			disconnect();
			resetComponents();
			socketThread = new EzySocketThread(this);
			socketThread.start();
		}

		protected override void connect0()
		{
            logger.info("connecting to server");
			String host = socketAddress.getHost();
			int port = socketAddress.getPort();
			startConnectTime = DateTime.Now;
			socketChannel = new TcpClient();
			socketChannel.BeginConnect(host, port, new AsyncCallback(handleConnectionResult), socketChannel);
		}

		private void handleConnectionResult(IAsyncResult result)
		{
			EzyEvent evt = null;
			try
			{
				socketChannel.EndConnect(result);
                logger.info("connected to server");
				socketReader.setSocketChannel(socketChannel);
				dataHandler.setSocketChannel(socketChannel);
				dataHandler.setDisconnected(false);
				reconnectCount = 0;
				evt = new EzyConnectionSuccessEvent();
			}
			catch (Exception ex)
			{
				if (ex is SocketException)
				{
					SocketException s = (SocketException)ex;
					switch ((SocketError)s.ErrorCode)
					{
						case SocketError.NetworkUnreachable:
							evt = EzyConnectionFailureEvent.networkUnreachable();
							break;
						case SocketError.TimedOut:
							evt = EzyConnectionFailureEvent.timeout();
							break;
						default:
							evt = EzyConnectionFailureEvent.connectionRefused();
							break;
					}
				}
				else if (ex is ArgumentException)
				{
					evt = EzyConnectionFailureEvent.unknownHost();
				}
				else
				{
					evt = EzyConnectionFailureEvent.unknown();
				}
                logger.info("connect to server: " + socketAddress + " error", ex);
			}
			EzySocketEvent socketEvent = new EzySimpleSocketEvent(EzySocketEventType.EVENT, evt);
			dataHandler.fireSocketEvent(socketEvent);

		}

		private long getReconnectSleepTime()
		{
			DateTime now = DateTime.Now;
			long offset = (long)(now - startConnectTime).TotalMilliseconds;
			long reconnectPeriod = reconnectConfig.getReconnectPeriod();
			long sleepTime = reconnectPeriod - offset;
			return sleepTime;
		}

		public override void send(EzyRequest request)
		{
			Object cmd = request.getCommand();
			EzyData data = request.serialize();
			send(cmd, data);
		}

		public override void send(Object cmd, EzyData data)
		{
			EzyArray array = EzyEntityFactory.newArrayBuilder()
											 .append((int)cmd)
											 .append(data)
											 .build();
			if (!unloggableCommands.Contains(cmd))
                logger.info("send command: " + cmd + " and data: " + data);
			EzyPackage pack = new EzySimplePackage(array);
			try
			{
				responseApi.response(pack);
			}
			catch (Exception e)
			{
                logger.info("send cmd: " + cmd + " with data: " + data + " error", e);
			}
		}

		public override void disconnect()
		{
			if (socketChannel != null)
				disconnect0();
			socketChannel = null;
			handleDisconnected();
			dataHandler.setDisconnected(true);
		}

		private void disconnect0()
		{
			try
			{
				socketChannel.Close();
			}
			catch (Exception e)
			{
                logger.info("close socket error", e);
			}
		}

		public void onDisconnected(int reason)
		{
			handleDisconnected();
		}

		private void handleDisconnected()
		{
			packetQueue.clear();
			eventQueue.clear();
			pingSchedule.stop();
		}

		protected override void resetComponents()
		{
			packetQueue.clear();
			eventQueue.clear();
			dataHandler.reset();
			socketReader.reset();
			socketWriter.reset();
			socketReadingLoopHandler.reset();
			socketWritingLoopHandler.reset();
		}

        public class EzySocketThread : EzyLoggable
		{
			private readonly Thread thread;
            private volatile bool cancelled;
			private readonly EzyTcpSocketClient client;

			public EzySocketThread(EzyTcpSocketClient client) : this(0, client)
			{
			}

			public EzySocketThread(long sleepTime, EzyTcpSocketClient client)
			{
				this.client = client;
                this.cancelled = false;
				this.thread = new Thread(() => handleConnect(sleepTime));
				this.thread.Name = "socket-connection";
			}

			private void handleConnect(long sleepTime)
			{
				try
				{
                    logger.info("sleeping " + sleepTime + "ms before connect to server");
					sleepBeforeConnect(sleepTime);
                    if (!cancelled)
                        client.connect0();
				}
				catch (Exception e)
				{
                    logger.info("start connect to server error", e);
				}
			}

			private void sleepBeforeConnect(long sleepTime)
			{
				if (sleepTime > 0)
					Thread.Sleep((int)sleepTime);
			}

			public void start()
			{
				thread.Start();
			}

			public void cancel()
			{
                this.cancelled = true;
			}
		}
	}
}
