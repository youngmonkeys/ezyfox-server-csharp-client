using System;
using System.Security.Cryptography;

namespace com.tvd12.ezyfoxserver.client.security
{
	public class EzyRsaKeyPairGentor : EzyKeyPairGentor
	{
		public EzyKeyPair generate(int keySize)
		{
			var provider = new RSACryptoServiceProvider(keySize);
			var privateKey = provider.ExportParameters(true);
			var publicKey = provider.ExportParameters(false);
			var privateKeyString = Convert.ToBase64String(privateKey.Modulus);
			var publicKeyString = Convert.ToBase64String(publicKey.Modulus);
			EzyKeyPair keyPair = new EzyKeyPair(privateKeyString, publicKeyString);
			return keyPair;
		}
	}
}
