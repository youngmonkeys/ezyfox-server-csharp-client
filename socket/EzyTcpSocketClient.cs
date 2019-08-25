using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using com.tvd12.ezyfoxserver.client.codec;
using com.tvd12.ezyfoxserver.client.config;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.evt;
using com.tvd12.ezyfoxserver.client.manager;
using com.tvd12.ezyfoxserver.client.net;
using com.tvd12.ezyfoxserver.client.util;
using static com.tvd12.ezyfoxserver.client.constant.EzySocketStatuses;

namespace com.tvd12.ezyfoxserver.client.socket
{
    public class EzyTcpSocketReader : EzySocketReader 
    {
        protected TcpClient socket;

        public void setSocket(TcpClient socket)
        {
            this.socket = socket;
        }

        protected override int readSocketData(byte[] readBytes)
        {
            int bytesToRead = socket.GetStream().Read(readBytes, 0, readBufferSize);
            return bytesToRead;
        }
    }

    public class EzyTcpSocketWriter : EzySocketWriter 
    {
        protected TcpClient socket;

        protected override void writeToSocket(EzyPacket packet)
        {
            byte[] buffer = (byte[])packet.getData();
            NetworkStream stream = socket.GetStream();
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
        }

        public void setSocket(TcpClient socket)
        {
            this.socket = socket;
        }
    }

    public class EzyTcpSocketClient : EzySocketClient
	{
		protected TcpClient socket;

        public EzyTcpSocketClient() : base() {
            this.socket = null;
        }

        protected override bool connectNow()
        {
            try
            {
                this.socket = new TcpClient(host, port);
                return true;
            }
            catch(Exception ex) 
            {
                if (ex is SocketException)
                {
                    SocketException s = (SocketException)ex;
                    switch ((SocketError)s.ErrorCode)
                    {
                        case SocketError.NetworkUnreachable:
                            connectionFailedReason = EzyConnectionFailedReason.NETWORK_UNREACHABLE;
                            break;
                        case SocketError.TimedOut:
                            connectionFailedReason = EzyConnectionFailedReason.TIME_OUT;
                            break;
                        default:
                            connectionFailedReason = EzyConnectionFailedReason.CONNECTION_REFUSED;
                            break;
                    }
                }
                else if (ex is ArgumentException)
                {
                    connectionFailedReason = EzyConnectionFailedReason.UNKNOWN_HOST;
                }
                else
                {
                    connectionFailedReason = EzyConnectionFailedReason.UNKNOWN;
                }
                return false;
            }

        }

        protected override void createAdapters()
        {
            socketReader = new EzyTcpSocketReader();
            socketWriter = new EzyTcpSocketWriter();
        }

        protected override void startAdapters()
        {
            ((EzyTcpSocketReader)socketReader).setSocket(socket);
            socketReader.start();
            ((EzyTcpSocketWriter)socketWriter).setSocket(socket);
            socketWriter.start();
        }

        protected override void resetSocket()
        {
            this.socket = null;
        }

        protected override void closeSocket()
        {
            if(socket != null)
                this.socket.Close();
        }
    }
}
