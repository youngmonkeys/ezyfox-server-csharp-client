using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.util;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.codec;
using com.tvd12.ezyfoxserver.client.concurrent;
using System.Threading.Tasks;

namespace com.tvd12.ezyfoxserver.client.socket
{
    public class EzyUdpSocketReader : EzySocketAdapter
    {
        protected byte[] sessionKey;
        protected EzySocketDataDecoder decoder;
        protected UdpClient datagramChannel;
        protected IPEndPoint serverEndPoint;
        protected Task<UdpReceiveResult> readTask;
        protected readonly int readBufferSize;
        protected readonly EzyQueue<EzyArray> dataQueue;

        public EzyUdpSocketReader() : base()
        {
            this.dataQueue = new EzySynchronizedQueue<EzyArray>();
            this.serverEndPoint = new IPEndPoint(IPAddress.Any, 0);
            this.readBufferSize = EzySocketConstants.MAX_READ_BUFFER_SIZE;
        }

        protected override void update()
        {
            while (true)
            {
                try
                {
                    if (!active)
                    {
                        return;
                    }
                    byte[] binary = readSocketData();
                    logger.info("udp received " + binary.Length + " bytes");
                    int bytesToRead = binary.Length;
                    if (bytesToRead <= 0)
                    {
                        return;
                    }
                    handleReceivedBytes(binary);

                    addSocketReadStats(binary.Length);
                }
                catch (SocketException e) {
                    logger.warn("I/O error at socket-reader: " + e.Message);
                    return;
                }
                catch (Exception e)
                {
                    logger.warn("I/O error at socket-reader", e);
                    return;
                }
            }
        }

        public override bool call()
        {
            try
            {
                if (!active)
                {
                    return false;
                }
                byte[] binary = readSocketDataAsync();
                if (binary == null)
                {
                    return true;
                }
                logger.debug("udp received " + binary.Length + " bytes");
                int bytesToRead = binary.Length;
                if (bytesToRead <= 0)
                {
                    return false;
                }
                handleReceivedBytes(binary);

                addSocketReadStats(binary.Length);
                return true;
            }
            catch (Exception e)
            {
                logger.warn("I/O error at socket-reader", e);
                return false;
            }
        }

        protected byte[] readSocketData() 
        {
            return datagramChannel.Receive(ref serverEndPoint);
        }

        protected byte[] readSocketDataAsync()
        {
            if (readTask == null)
            {
                readTask = datagramChannel.ReceiveAsync();
            }
            byte[] bytes = null;
            if (readTask.IsCompleted)
            {
                UdpReceiveResult result = readTask.Result;
                bytes = result.Buffer;
                readTask = null;
            }

            return bytes;
        }

        protected void handleReceivedBytes(byte[] bytes)
        {
            EzyMessage message = EzyMessageReaders.bytesToMessage(bytes);
            if (message == null)
                return;
            onMesssageReceived(message);
        }

        protected override void clear()
        {
            if (dataQueue != null)
                dataQueue.clear();
        }

        public void popMessages(IList<EzyArray> buffer)
        {
            dataQueue.pollAll(buffer);
        }

        private void onMesssageReceived(EzyMessage message)
        {
            try
            {
                Object data = decoder.decode(message, sessionKey);
                dataQueue.add((EzyArray)data);
                Console.WriteLine("udp received: " + data);
            }
            catch (Exception e)
            {
                logger.warn("decode error at socket-reader", e);
            }
        }

        public void setSessionKey(byte[] sessionKey)
        {
            this.sessionKey = sessionKey;
        }

        public void setDecoder(EzySocketDataDecoder decoder)
        {
            this.decoder = decoder;
        }

        public void setDatagramChannel(UdpClient datagramChannel)
        {
            this.datagramChannel = datagramChannel;
        }

        protected override String getThreadName()
        {
            return "udp-socket-reader";
        }
    }
}
