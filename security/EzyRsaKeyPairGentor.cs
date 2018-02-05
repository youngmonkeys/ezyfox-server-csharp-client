using System;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;

namespace com.tvd12.ezyfoxserver.client.security
{
	public class EzyRsaKeyPairGentor : EzyKeyPairGentor
	{
		public EzyKeyPair generate(int keySize)
		{
			RsaKeyPairGenerator generator = new RsaKeyPairGenerator();
			generator.Init(new KeyGenerationParameters(new SecureRandom(), keySize));
			var pair = generator.GenerateKeyPair();
			var privateKey = PrivateKeyInfoFactory.CreatePrivateKeyInfo(pair.Private);
			byte[] privateKeyBytes = privateKey.ToAsn1Object().GetDerEncoded();
			string privateKeyString = Convert.ToBase64String(privateKeyBytes);

			var publicKey = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(pair.Public);
			byte[] publicKeyBytes = publicKey.ToAsn1Object().GetDerEncoded();
			string publicKeyString = Convert.ToBase64String(publicKeyBytes);
			EzyKeyPair keyPair = new EzyKeyPair(privateKeyString, publicKeyString);
			return keyPair;
		}
	}
}
