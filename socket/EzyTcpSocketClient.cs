using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using com.tvd12.ezyfoxserver.client.constant;

namespace com.tvd12.ezyfoxserver.client.socket
{
    public class EzyTcpSocketReader : EzySocketReader 
    {
        protected TcpClient socket;
        protected Task<int> readTask;

        public void setSocket(TcpClient socket)
        {
            this.socket = socket;
        }

        protected override int readSocketData(byte[] buffer)
        {
            try
            {
                return socket.GetStream().Read(buffer, 0, bufferSize);
            }
            catch(Exception ex) 
            {
                logger.warn("I/O error at socket-reader", ex);
                return -1;    
            }
        }

        protected override int readSocketDataAsync(byte[] buffer)
        {
            try
            {
                if (readTask == null)
                {
                    readTask = socket
                        .GetStream()
                        .ReadAsync(buffer, 0, bufferSize);
                }
                int readBytes = 0;
                if (readTask.IsCompleted)
                {
                    readBytes = readTask.Result;
                    readTask = null;
                }
                return readBytes;
            }
            catch (Exception ex)
            {
                logger.warn("I/O error at socket-reader", ex);
                return -1;
            }
        }
    }

    public class EzyTcpSocketWriter : EzySocketWriter 
    {
        protected TcpClient socket;

        protected override int writeToSocket(EzyPacket packet)
        {
            try
            {
                byte[] buffer = (byte[])packet.getData();
                NetworkStream stream = socket.GetStream();
                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();
                int writtenBytes = buffer.Length;
                return writtenBytes;
            }
            catch(Exception ex) 
            {
                logger.warn("I/O error at socket-writer", ex);
                return -1;
            }
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
