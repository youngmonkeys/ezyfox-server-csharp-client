using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.util;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.codec;
using com.tvd12.ezyfoxserver.client.concurrent;

namespace com.tvd12.ezyfoxserver.client.socket
{
    public class EzyUdpSocketReader : EzySocketAdapter
    {
        protected readonly int readBufferSize;
        protected EzyQueue<EzyArray> dataQueue;
        protected EzySocketDataDecoder decoder;
        protected UdpClient datagramChannel;
        protected IPEndPoint serverEndPoint;

        public EzyUdpSocketReader() : base()
        {
            this.readBufferSize = EzySocketConstants.MAX_READ_BUFFER_SIZE;
        }

        protected override void run()
        {
            this.dataQueue = new EzySynchronizedQueue<EzyArray>();
            this.serverEndPoint = new IPEndPoint(IPAddress.Any, 0);
            base.run();
        }

        protected override void update()
        {
            while (true)
            {
                try
                {
                    if (!active)
                        return;
                    byte[] binary = readSocketData();
                    logger.info("udp received " + binary.Length + " bytes");
                    int bytesToRead = binary.Length;
                    if (bytesToRead <= 0)
                        return;
                    handleReceivedBytes(binary);

                    networkStatistics.getSocketStats().getNetworkStats().addReadBytes(binary.Length);
                    networkStatistics.getSocketStats().getNetworkStats().addReadPackets(1);
                }
                catch (SocketException e) {
                    logger.warn("I/O error at socket-reader: " + e.Message);
                    networkStatistics.getSocketStats().getNetworkStats().addReadPackets(1);
                    return;
                }
                catch (Exception e)
                {
                    logger.warn("I/O error at socket-reader", e);
                    networkStatistics.getSocketStats().getNetworkStats().addReadPackets(1);
                    return;
                }
            }
        }

        protected byte[] readSocketData() 
        {
            byte[] bytes = datagramChannel.Receive(ref serverEndPoint);
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
                Object data = decoder.decode(message);
                dataQueue.add((EzyArray)data);
                Console.WriteLine("udp received: " + data);
            }
            catch (Exception e)
            {
                logger.warn("decode error at socket-reader", e);
            }
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
