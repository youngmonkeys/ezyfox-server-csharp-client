using System;
using System.Collections.Generic;
using System.Threading;
using com.tvd12.ezyfoxserver.client.codec;
using com.tvd12.ezyfoxserver.client.config;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.evt;
using com.tvd12.ezyfoxserver.client.handler;
using com.tvd12.ezyfoxserver.client.manager;
using com.tvd12.ezyfoxserver.client.util;
using static com.tvd12.ezyfoxserver.client.constant.EzySocketStatuses;

namespace com.tvd12.ezyfoxserver.client.socket
{
    public abstract class EzySocketClient : EzyLoggable , EzyISocketClient, EzySocketDelegate
    {
        protected String host;
        protected int port;
        protected int reconnectCount;
        protected DateTime connectTime;
        protected int disconnectReason;
        protected long sessionId;
        protected String sessionToken;
        protected EzyReconnectConfig reconnectConfig;
        protected EzyHandlerManager handlerManager;
        protected ISet<Object> unloggableCommands;
        protected EzyPingManager pingManager;
        protected EzyPingSchedule pingSchedule;
        protected EzyEventHandlers eventHandlers;
        protected EzyDataHandlers dataHandlers;
        protected EzySocketReader socketReader;
        protected EzySocketWriter socketWriter;
        protected EzyConnectionFailedReason connectionFailedReason;
        protected readonly EzyCodecFactory codecFactory;
        protected readonly EzyPacketQueue packetQueue;
        protected readonly EzySocketEventQueue socketEventQueue;
        protected readonly EzyResponseApi responseApi;
        protected readonly IList<EzyEvent> localEventQueue;
        protected readonly IList<EzyArray> localMessageQueue;
        protected readonly IList<EzySocketStatus> localSocketStatuses;
        protected readonly EzyValueStack<EzySocketStatus> socketStatuses;

        public EzySocketClient()
        {
            this.codecFactory = new EzySimpleCodecFactory();
            this.packetQueue = new EzyBlockingPacketQueue();
            this.socketEventQueue = new EzySocketEventQueue();
            this.responseApi = newResponseApi();
            this.localEventQueue = new List<EzyEvent>();
            this.localMessageQueue = new List<EzyArray>();
            this.localSocketStatuses = new List<EzySocketStatus>();
            this.socketStatuses = new EzyValueStack<EzySocketStatus>(EzySocketStatus.NOT_CONNECT);
        }

        private EzyResponseApi newResponseApi()
        {
            Object encoder = codecFactory.newEncoder(EzyConnectionType.SOCKET);
            EzySocketDataEncoder socketDataEncoder = new EzySimpleSocketDataEncoder(encoder);
            EzyResponseApi api = new EzySocketResponseApi(socketDataEncoder, packetQueue);
            return api;
        }

        public void connectTo(String host, int port)
        {
            EzySocketStatus status = socketStatuses.last();
            if (!isSocketConnectable(status))
            {
                logger.warn("socket is connecting...");
                return;
            }
            this.socketStatuses.push(EzySocketStatus.CONNECTING);
            this.host = host;
            this.port = port;
            this.reconnectCount = 0;
            this.connect0(0);
        }

        public bool reconnect()
        {
            EzySocketStatus status = socketStatuses.last();
            if (!isSocketReconnectable(status))
            {
                logger.warn("socket is not in a reconnectable status");
                return false;
            }
            int maxReconnectCount = reconnectConfig.getMaxReconnectCount();
            if (reconnectCount >= maxReconnectCount)
                return false;
            socketStatuses.push(EzySocketStatus.RECONNECTING);
            int reconnectSleepTime = reconnectConfig.getReconnectPeriod();
            connect0(reconnectSleepTime);
            reconnectCount++;
            logger.info("try reconnect to server: " + reconnectCount + ", wating time: " + reconnectSleepTime);
            EzyEvent tryConnectEvent = new EzyTryConnectEvent(reconnectCount);
            socketEventQueue.addEvent(tryConnectEvent);
            return true;
        }

        protected void connect0(int sleepTime)
        {
            clearAdapters();
            createAdapters();
            updateAdapters();
            closeSocket();
            packetQueue.clear();
            socketEventQueue.clear();
            socketStatuses.clear();
            disconnectReason = (int)EzyDisconnectReason.UNKNOWN;
            connectionFailedReason = EzyConnectionFailedReason.UNKNOWN;
            Thread newThread = new Thread(() => connect1(sleepTime));
            newThread.Name = "ezyfox-connection";
            newThread.Start();
        }

        protected void connect1(int sleepTime)
        {
            DateTime currentTime = DateTime.Now;
            long dt = (long)(currentTime - connectTime).TotalMilliseconds;
            long realSleepTime = sleepTime;
            if (sleepTime <= 0)
            {
                if (dt < 2000) //delay 2000ms
                    realSleepTime = 2000 - dt;
            }
            if (realSleepTime >= 0)
                Thread.Sleep((int)realSleepTime);
            socketStatuses.push(EzySocketStatus.CONNECTING);
            bool success = this.connectNow();
            connectTime = DateTime.Now;

            if (success)
            {
                this.reconnectCount = 0;
                this.startAdapters();
                this.socketStatuses.push(EzySocketStatus.CONNECTED);
            }
            else
            {
                this.resetSocket();
                this.socketStatuses.push(EzySocketStatus.CONNECT_FAILED);
            }
        }

        protected abstract bool connectNow();

        protected abstract void createAdapters();

        protected void updateAdapters() 
        {
            Object decoder = codecFactory.newDecoder(EzyConnectionType.SOCKET);
            EzySocketDataDecoder socketDataDecoder = new EzySimpleSocketDataDecoder(decoder);
            socketReader.setDecoder(socketDataDecoder);
            socketWriter.setPacketQueue(packetQueue);
        }

        protected abstract void startAdapters();

        protected void clearAdapters()
        {
            clearAdapter(socketReader);
            clearAdapter(socketWriter);
            socketReader = null;
            socketWriter = null;
        }

        protected void clearAdapter(EzySocketAdapter adapter)
        {
            if (adapter != null)
                adapter.stop();
        }

        protected virtual void clearComponents(int disconnectReason) { }

        protected abstract void resetSocket();

        protected abstract void closeSocket();

        public virtual void onDisconnected(int reason)
        {
            pingSchedule.stop();
            packetQueue.clear();
            packetQueue.wakeup();
            socketEventQueue.clear();
            closeSocket();
            clearAdapters();
            clearComponents(reason);
            socketStatuses.push(EzySocketStatus.DISCONNECTED);
        }

        public void disconnect(int reason)
        {
            if (socketStatuses.last() != EzySocketStatus.CONNECTED)
                return;
            onDisconnected(disconnectReason = reason);
        }

        public void sendMessage(EzyArray message)
        {
            EzyPackage pack = new EzySimplePackage(message);
            responseApi.response(pack);
        }

        public void processEventMessages()
        {
            processStatuses();
            processEvents();
            processReceivedMessages();
        }

        protected void processStatuses()
        {
            socketStatuses.popAll(localSocketStatuses);
            for (int i = 0; i < localSocketStatuses.Count; ++i)
            {
                EzySocketStatus status = localSocketStatuses[i];
                if (status == EzySocketStatus.CONNECTED)
                {
                    EzyEvent evt = new EzyConnectionSuccessEvent();
                    socketEventQueue.addEvent(evt);
                }
                else if (status == EzySocketStatus.CONNECT_FAILED)
                {
                    EzyEvent evt = new EzyConnectionFailureEvent(connectionFailedReason);
                    socketEventQueue.addEvent(evt);
                    break;
                }
                else if (status == EzySocketStatus.DISCONNECTED)
                {
                    EzyEvent evt = new EzyDisconnectionEvent(disconnectReason);
                    socketEventQueue.addEvent(evt);
                    break;
                }
            }
            localSocketStatuses.Clear();
        }

        protected void processEvents()
        {
            socketEventQueue.popAll(localEventQueue);
            for (int i = 0; i < localEventQueue.Count; ++i)
            {
                EzyEvent evt = localEventQueue[i];
                eventHandlers.handle(evt);
            }
            localEventQueue.Clear();
        }

        protected void processReceivedMessages()
        {
            EzySocketStatus status = socketStatuses.last();
            if (status == EzySocketStatus.CONNECTED)
            {
                if (socketReader.isActive())
                {
                    processReceivedMessages0();
                }
            }
            EzySocketStatus statusLast = socketStatuses.last();
            if (isSocketDisconnectable(statusLast))
            {
                if (socketReader.isStopped())
                {
                    onDisconnected(disconnectReason);
                }
                else if (socketWriter.isStopped())
                {
                    onDisconnected(disconnectReason);
                }
            }
        }

        protected void processReceivedMessages0()
        {
            pingManager.setLostPingCount(0);
            popReadMessages();
            for (int i = 0; i < localMessageQueue.Count; ++i)
            {
                processReceivedMessage(localMessageQueue[i]);
            }
            localMessageQueue.Clear();
        }

        protected virtual void popReadMessages()
        {
            socketReader.popMessages(localMessageQueue);
        }

        protected void processReceivedMessage(EzyArray message)
        {
            int cmdId = message.get<int>(0);
            EzyArray data = message.get<EzyArray>(1, null);
            EzyCommand cmd = (EzyCommand)cmdId;
            printReceivedData(cmd, data);
            if (cmd == EzyCommand.DISCONNECT)
            {
                int reasonId = data.get<int>(0);
                disconnectReason = reasonId;
                socketStatuses.push(EzySocketStatus.DISCONNECTING);
            }
            else
            {
                dataHandlers.handle(cmd, data);
            }
        }

        protected void printReceivedData(EzyCommand cmd, EzyArray data)
        {
            if (!unloggableCommands.Contains(cmd))
                logger.debug("received command: " + cmd + " and data: " + data);
        }

        public string getHost()
        {
            return this.host;
        }

        public int getPort()
        {
            return this.port;
        }

        public void setSessionId(long sessionId)
        {
            this.sessionId = sessionId;
        }

        public void setSessionToken(String sessionToken)
        {
            this.sessionToken = sessionToken;
        }

        public void setPingManager(EzyPingManager pingManager) 
        {
            this.pingManager = pingManager;    
        }

        public void setPingSchedule(EzyPingSchedule pingSchedule)
        {
            this.pingSchedule = pingSchedule;
            this.pingSchedule.setSocketEventQueue(socketEventQueue);
        }

        public void setHandlerManager(EzyHandlerManager handlerManager)
        {
            this.handlerManager = handlerManager;
            this.dataHandlers = handlerManager.getDataHandlers();
            this.eventHandlers = handlerManager.getEventHandlers();
        }

        public void setReconnectConfig(EzyReconnectConfig reconnectConfig)
        {
            this.reconnectConfig = reconnectConfig;
        }

        public void setUnloggableCommands(ISet<Object> unloggableCommands)
        {
            this.unloggableCommands = unloggableCommands;
        }
    }

}
