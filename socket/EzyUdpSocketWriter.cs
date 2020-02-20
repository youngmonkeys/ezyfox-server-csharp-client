using System;
using System.Net.Sockets;

namespace com.tvd12.ezyfoxserver.client.socket
{
    public class EzyUdpSocketWriter : EzySocketWriter
    {
        protected UdpClient datagramChannel;

        public void setDatagramChannel(UdpClient datagramChannel)
        {
            this.datagramChannel = datagramChannel;
        }

        protected override int writeToSocket(EzyPacket packet)
        {
            try {
                byte[] bytes = (byte[])packet.getData();
                int bytesToWrite = bytes.Length;
                int writtenByes = datagramChannel.Send(bytes, bytesToWrite);
                return writtenByes;
            }
            catch (Exception e) {
                logger.warn("I/O error at socket-writer", e);
                return -1;
            }
            finally {
                packet.release();
            }
        }
    
        protected override String getThreadName()
        {
            return "udp-socket-writer";
        }
    }
}
