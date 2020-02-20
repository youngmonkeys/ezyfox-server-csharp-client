using System;
using System.Text;

namespace com.tvd12.ezyfoxserver.client.codec
{
	public class EzySimpleMessageHeader : EzyMessageHeader
	{
		protected bool bigSize;
		protected bool encrypted;
		protected bool compressed;
		protected bool text;
        protected bool rawBytes;
        protected bool udpHandshake;

        public EzySimpleMessageHeader(bool bigSize,
                                      bool encrypted, 
                                      bool compressed, 
                                      bool text,
                                      bool rawBytes,
                                      bool udpHandshake)
        {
            this.bigSize = bigSize;
            this.encrypted = encrypted;
            this.compressed = compressed;
            this.text = text;
            this.rawBytes = rawBytes;
            this.udpHandshake = udpHandshake;
        }

		public void setBigSize(bool bigSize)
		{
			this.bigSize = bigSize;
		}

		public void setEncrypted(bool encrypted)
		{
			this.encrypted = encrypted;
		}

		public void setCompressed(bool compressed)
		{
			this.compressed = compressed;
		}

		public void setText(bool text)
		{
			this.text = text;
		}

        public void setRawBytes(bool rawBytes)
        {
            this.rawBytes = rawBytes;
        }

        public void setUdpHandshake(bool udpHandshake)
        {
            this.udpHandshake = udpHandshake;
        }

		public bool isBigSize()
		{
			return bigSize;
		}

		public bool isEncrypted()
		{
			return encrypted;
		}

		public bool isCompressed()
		{
			return compressed;
		}

		public bool isText()
		{
			return text;
		}

        public bool isRawBytes() 
        {
            return rawBytes;
        }

        public bool isUdpHandshake()
        {
            return udpHandshake;
        }

		public String toString()
		{
			return new StringBuilder()
					.Append("<")
					.Append("bigSize: ")
						.Append(bigSize)
						.Append(", ")
					.Append("encrypted: ")
						.Append(encrypted)
						.Append(", ")
					.Append("compressed: ")
						.Append(compressed)
						.Append(", ")
					.Append("text: ")
						.Append(text)
					.Append(">")
					.ToString();
		}
	}
}
