using System;
using System.Text;

namespace com.tvd12.ezyfoxserver.client.security
{
	public class EzyKeyPair
	{
		private readonly String privateKey;
		private readonly String publicKey;

		public EzyKeyPair(String privateKey, String publicKey)
		{
			this.privateKey = privateKey;
			this.publicKey = publicKey;
		}

		public String getPrivateKey()
		{
			return privateKey;
		}

		public String getPublicKey()
		{
			return publicKey;
		}

		public override string ToString()
		{
			return new StringBuilder()
				.Append("(")
					.Append("public key: ")
						.Append(publicKey)
					.Append("private key: ")
						.Append(privateKey)
				.Append(")")
				.ToString();
		}
	}
}
