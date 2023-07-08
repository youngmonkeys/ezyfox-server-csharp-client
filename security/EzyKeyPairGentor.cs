using System.Security.Cryptography;
using com.tvd12.ezyfoxserver.client.builder;
using com.tvd12.ezyfoxserver.client.config;

namespace com.tvd12.ezyfoxserver.client.security
{
	public class EzyKeyPairGentor
	{
		private readonly int keySize;

		protected EzyKeyPairGentor(Builder builder)
		{
			this.keySize = builder._keySize;	
		}

        public EzyKeyPair generate()
		{
            using (RSA rsa = RSA.Create())
            {
                RSAParameters publicKey = rsa.ExportParameters(false);
                RSAParameters privateKey = rsa.ExportParameters(true);

                byte[] publicKeyBytes = exportPublicKeyToBytes(publicKey);
                byte[] privateKeyBytes = exportPrivateKeyToBytes(privateKey);
                return new EzyKeyPair(privateKeyBytes, publicKeyBytes);
            }
        }

        private byte[] exportPublicKeyToBytes(RSAParameters publicKey)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportParameters(publicKey);
                byte[] keyBytes = rsa.ExportSubjectPublicKeyInfo();
                return keyBytes;
            }
        }

        private byte[] exportPrivateKeyToBytes(RSAParameters privateKey)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportParameters(privateKey);
                byte[] keyBytes = rsa.ExportPkcs8PrivateKey();
                return keyBytes;
            }
        }

        public static Builder builder()
        {
            return new Builder();
        }

        public class Builder : EzyBuilder<EzyKeyPairGentor>
		{
			public int _keySize = 2048;

			public Builder keySize(int keySize)
			{
				this._keySize = keySize;
				return this;
			}

			public EzyKeyPairGentor build()
			{
                return new EzyKeyPairGentor(this);
			}
        }
    }
}
