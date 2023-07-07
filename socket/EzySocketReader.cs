using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.util;
using com.tvd12.ezyfoxserver.client.concurrent;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.io;
using com.tvd12.ezyfoxserver.client.codec;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.callback;


namespace com.tvd12.ezyfoxserver.client.socket
{
    public abstract class EzySocketReader : EzySocketAdapter
	{
        protected byte[] sessionKey;
        protected EzySocketDataDecoder decoder;
        protected readonly int readBufferSize;
        protected readonly EzyQueue<EzyArray> dataQueue;
        protected readonly EzyCallback<EzyMessage> decodeBytesCallback;

        public EzySocketReader()
		{
            this.dataQueue = new EzySynchronizedQueue<EzyArray>();
            this.readBufferSize = EzySocketConstants.MAX_READ_BUFFER_SIZE;
            this.decodeBytesCallback = message => onMesssageReceived(message);
		}

        protected override void update()
		{
            byte[] readBytes = new byte[readBufferSize];
            while(true) 
            {
                if (!active)
                    return;
                int bytesToRead = readSocketData(readBytes);
                if (bytesToRead <= 0)
                    return;
                byte[] binary = EzyBytes.copyBytes(readBytes, bytesToRead);
                decoder.decode(binary, decodeBytesCallback);

                networkStatistics.getSocketStats().getNetworkStats().addReadBytes(binary.Length);
                networkStatistics.getSocketStats().getNetworkStats().addReadPackets(1);
            }
			
		}

        protected abstract int readSocketData(byte[] readBytes);

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
            Object data = decoder.decode(message, sessionKey);
            dataQueue.add((EzyArray)data);
        }

        public void setSessionKey(byte[] sessionKey)
        {
            this.sessionKey = sessionKey;
        }

        public void setDecoder(EzySocketDataDecoder decoder)
        {
            this.decoder = decoder;
        }

        protected override string getThreadName()
        {
            return "ezyfox-socket-reader";
        }
	}
}
