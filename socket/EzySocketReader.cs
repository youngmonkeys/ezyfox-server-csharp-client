using System;
using System.Net.Sockets;
using System.Threading;
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
        protected EzyByteBuffer readBuffer;
        protected EzyQueue<EzyArray> dataQueue;
        protected EzySocketDataDecoder decoder;
        protected readonly int readBufferSize;
        protected readonly EzyCallback<EzyMessage> decodeBytesCallback;

        public EzySocketReader()
		{
            this.readBufferSize = EzySocketConstants.MAX_READ_BUFFER_SIZE;
            this.decodeBytesCallback = message => onMesssageReceived(message);
		}

        protected EzyByteBuffer newReadBuffer(int bufferSize)
        {
            return EzyByteBuffer.allocate(bufferSize);
        }

        protected override void run()
        {
            this.readBuffer = newReadBuffer(readBufferSize);
            this.dataQueue = new EzySynchronizedQueue<EzyArray>();
            base.run();
        }

        protected override void update()
		{
            byte[] readBytes = new byte[readBufferSize];
            while(true) 
            {
                Thread.Sleep(3);

                if (!active)
                    return;
                int bytesToRead = readSocketData(readBytes);
                if (bytesToRead <= 0)
                    return;
                readBuffer.clear();
                readBuffer.put(readBytes, 0, bytesToRead);
                readBuffer.flip();
                byte[] binary = readBuffer.getBytes(bytesToRead);
                decoder.decode(binary, decodeBytesCallback);
            }
			
		}

        protected abstract int readSocketData(byte[] readBytes);

        public void popMessages(IList<EzyArray> buffer)
        {
            dataQueue.pollAll(buffer);
        }

        private void onMesssageReceived(EzyMessage message)
        {
            Object data = decoder.decode(message);
            dataQueue.add((EzyArray)data);
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
