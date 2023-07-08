using System.Text;

namespace com.tvd12.ezyfoxserver.client.security
{
	public class EzyKeyPair
	{
		private readonly byte[] privateKey;
		private readonly byte[] publicKey;

		public EzyKeyPair(byte[] privateKey, byte[] publicKey)
		{
			this.privateKey = privateKey;
			this.publicKey = publicKey;
		}

		public byte[] getPrivateKey()
		{
			return privateKey;
		}

		public byte[] getPublicKey()
		{
			return publicKey;
		}

		public override string ToString()
		{
			return new StringBuilder()
				.Append("(")
					.Append("public key: ")
						.Append(Encoding.UTF8.GetString(publicKey))
					.Append("private key: ")
						.Append(Encoding.UTF8.GetString(privateKey))
				.Append(")")
				.ToString();
		}
	}
}
