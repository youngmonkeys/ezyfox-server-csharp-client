using System;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.util;
using com.tvd12.ezyfoxserver.client.io;
using com.tvd12.ezyfoxserver.client.net;
using com.tvd12.ezyfoxserver.client.codec;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.exception;
using static com.tvd12.ezyfoxserver.client.constant.EzySocketStatuses;
using com.tvd12.ezyfoxserver.client.statistics;

namespace com.tvd12.ezyfoxserver.client.socket
{
    public class EzyUdpSocketClient : EzyLoggable, EzyISocketClient
    {
        protected long sessionId;
        protected String sessionToken;
        protected byte[] sessionKey;
        protected InetSocketAddress serverAddress;
        protected UdpClient datagramChannel;
        protected EzyUdpSocketReader socketReader;
        protected EzyUdpSocketWriter socketWriter;
        protected readonly EzyPacketQueue packetQueue;
        protected readonly EzyResponseApi responseApi;
        protected readonly EzyCodecFactory codecFactory;
        protected readonly EzyValueStack<EzySocketStatus> socketStatuses;
        protected EzyStatistics networkStatistics;

        public EzyUdpSocketClient(EzyCodecFactory codecFactory) {
            this.codecFactory = codecFactory;
            this.packetQueue = new EzyBlockingPacketQueue();
            this.responseApi = newResponseApi();
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
                logger.warn("udp socket is connecting...");
                return;
            }
            serverAddress = new InetSocketAddress(host, port);
            connect0();
        }

        public bool reconnect()
        {
            EzySocketStatus status = socketStatuses.last();
            if (status != EzySocketStatus.CONNECT_FAILED)
            {
                return false;
            }
            logger.warn("udp socket is re-connecting...");
            connect0();
            return true;
        }

        public void setStatus(EzySocketStatus status)
        {
            socketStatuses.push(status);
        }

        protected void connect0()
        {
            try
            {
                clearAdapters();
                createAdapters();
                addNetworkStatisticsAdapers();
                updateAdapters();
                closeSocket();
                packetQueue.clear();
                socketStatuses.clear();
                datagramChannel = new UdpClient();
                datagramChannel.Connect(serverAddress.getHost(), serverAddress.getPort());
                startAdapters();
                socketStatuses.push(EzySocketStatus.CONNECTING);
                sendHandshakeRequest();
                Thread newThread = new Thread(() => { 
                    Thread.Sleep(3000);
                    EzySocketStatus status = socketStatuses.last();
                    if (status == EzySocketStatus.CONNECTING)
                        socketStatuses.push(EzySocketStatus.CONNECT_FAILED);
                    reconnect(); 
                });
                newThread.Name = "udp-reconnect";
                newThread.Start();
            }
            catch (Exception e) {
                throw new EzyUdpConnectionException("udp can't connect to: " + serverAddress, e);
            }
        }

        public void disconnect(int reason)
        {
            packetQueue.clear();
            packetQueue.wakeup();
            closeSocket();
            clearAdapters();
            socketStatuses.push(EzySocketStatus.DISCONNECTED);
        }

        public void sendMessage(EzyArray message, bool encrypted)
        {
            EzyPackage pack = new EzySimplePackage(
                message,
                encrypted,
                sessionKey,
                EzyTransportType.UDP
            );
            try
            {
                responseApi.response(pack);
            }
            catch (Exception e)
            {
                logger.warn("udp send message: " + message + " error", e);
            }
        }

        public void popReadMessages(IList<EzyArray> buffer)
        {
            EzySocketStatus status = socketStatuses.last();
            if (status == EzySocketStatus.CONNECTING || status == EzySocketStatus.CONNECTED)
                this.socketReader.popMessages(buffer);
        }

        protected void createAdapters()
        {
            this.socketReader = new EzyUdpSocketReader();
            this.socketWriter = new EzyUdpSocketWriter();
        }

        public void setNetworkStatistics(EzyStatistics networkStatistics)
        {
            this.networkStatistics = networkStatistics;
        }

        protected void addNetworkStatisticsAdapers()
        {
            socketReader.setNetworkStatistics(networkStatistics);
            socketWriter.setNetworkStatistics(networkStatistics);
        }

        protected void updateAdapters()
        {
            Object decoder = codecFactory.newDecoder(EzyConnectionType.SOCKET);
            EzySocketDataDecoder socketDataDecoder = new EzySimpleSocketDataDecoder(decoder);
            this.setSessionToken(sessionToken);
            this.socketReader.setDecoder(socketDataDecoder);
            this.socketWriter.setPacketQueue(packetQueue);
        }

        protected void startAdapters()
        {
            this.socketReader.setDatagramChannel(datagramChannel);
            this.socketReader.start();
            this.socketWriter.setDatagramChannel(datagramChannel);
            this.socketWriter.start();
        }

        protected void clearAdapters()
        {
            this.clearAdapter(socketReader);
            this.socketReader = null;
            this.clearAdapter(socketWriter);
            this.socketWriter = null;
        }

        protected void clearAdapter(EzySocketAdapter adapter)
        {
            if (adapter != null)
                adapter.stop();
        }

        protected void closeSocket()
        {
            try
            {
                if (datagramChannel != null)
                    datagramChannel.Close();
            }
            catch (Exception e)
            {
                logger.warn("close udp socket error", e);
            }
        }

        protected void sendHandshakeRequest()
        {
            int tokenSize = sessionToken.Length;
            int messageSize = 0;
            messageSize += 8; // sessionIdSize
            messageSize += 2; // tokenLengthSize
            messageSize += tokenSize; // messageSize
            EzyByteBuffer buffer = EzyByteBuffer.allocate(1 + 2 + messageSize);
            byte header = 0;
            header |= 1 << 5;
            buffer.put(header);
            buffer.putShort((short) messageSize);
            buffer.putLong(sessionId);
            buffer.putShort((short) tokenSize);
            buffer.put(EzyStrings.getBytes(sessionToken));
            buffer.flip();
            byte[] bytes = buffer.getRemainBytes();
            datagramChannel.Send(bytes, bytes.Length);
        }

        public void setSessionId(long sessionId)
        {
            this.sessionId = sessionId;
        }

        public void setSessionToken(String sessionToken)
        {
            this.sessionToken = sessionToken;
        }

        public void setSessionKey(byte[] sessionKey)
        {
            this.sessionKey = sessionKey;
        }
    }
}
