using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public class EzySimplePackage : EzyPackage
	{
		protected EzyArray data;
        protected bool encrypted;
        protected byte[] encryptionKey;
		protected EzyTransportType transportType;

		public EzySimplePackage(
			EzyArray data,
            bool encrypted,
            byte[] encryptionKey
        ) : this(data, encrypted, encryptionKey, EzyTransportType.TCP)
		{
		}

		public EzySimplePackage(
			EzyArray data,
			bool encrypted,
			byte[] encryptionKey,
			EzyTransportType transportType
		)
		{
			this.data = data;
			this.encrypted = encrypted;
			this.encryptionKey = encryptionKey;
			this.transportType = transportType;
		}

		public EzyArray getData()
		{
			return data;
		}

		public bool isEncrypted()
		{
			return encrypted;
		}

		public byte[] getEncryptionKey()
		{
			return encryptionKey;
		}

		public EzyTransportType getTransportType()
		{
			return transportType;
		}

		public void release()
		{
			this.data = null;
			this.encryptionKey = null;
		}

	}
}
