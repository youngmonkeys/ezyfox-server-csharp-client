using System;

namespace com.tvd12.ezyfoxserver.client.exception
{
    public class EzyUdpConnectionException : Exception
    {
        public EzyUdpConnectionException(String msg)
            : base(msg)
        {
        }

        public EzyUdpConnectionException(String msg, Exception e)
            : base(msg, e)
        {
        }
    }
}