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
        protected readonly byte[] buffer;
        protected readonly int bufferSize;
        protected readonly EzyQueue<EzyArray> dataQueue;
        protected readonly EzyCallback<EzyMessage> decodeBytesCallback;

        public EzySocketReader()
		{
            this.dataQueue = new EzySynchronizedQueue<EzyArray>();
            this.bufferSize = EzySocketConstants.MAX_READ_BUFFER_SIZE;
            this.buffer = new byte[bufferSize];
            this.decodeBytesCallback = message => onMesssageReceived(message);
		}

        protected override void update()
		{
            while(true) 
            {
                if (!active)
                {
                    return;
                }
                int readBytes = readSocketData(buffer);
                if (readBytes <= 0)
                {
                    return;
                }
                byte[] binary = EzyBytes.copyBytes(buffer, readBytes);
                decoder.decode(binary, decodeBytesCallback);

                addSocketReadStats(readBytes);
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
                int readBytes = readSocketDataAsync(buffer);
                if (readBytes < 0)
                {
                    return false;
                }
                byte[] binary = EzyBytes.copyBytes(buffer, readBytes);
                decoder.decode(binary, decodeBytesCallback);

                addSocketReadStats(readBytes);
                return true;
            }
            catch (Exception e)
            {
                logger.info("problems in socket-reader even loop", e);
                return false;
            }
        }

        protected abstract int readSocketData(byte[] readBytes);

        protected abstract int readSocketDataAsync(byte[] readBytes);

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
