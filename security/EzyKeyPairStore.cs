using System;
using System.IO;
using com.tvd12.ezyfoxserver.client.util;

namespace com.tvd12.ezyfoxserver.client.security
{
	public interface EzyKeyPairStore
	{
		void store(EzyKeyPair keypair);
	}

	public class EzySystemKeyPairStore : EzyLoggable, EzyKeyPairStore
	{
		private const String FOLDER = ".runtime";
		private const String PUBLIC_KEY_PATH = FOLDER + "/public_key.txt";
		private const String PRIVATE_KEY_PATH = FOLDER + "/private_key.txt";

		public void store(EzyKeyPair keypair)
		{
			Directory.CreateDirectory(FOLDER);
			StreamWriter publicKeyWriter = new StreamWriter(PUBLIC_KEY_PATH, true);
			publicKeyWriter.WriteLine(keypair.getPublicKey());
			publicKeyWriter.Close();
			StreamWriter privateKeyWriter = new StreamWriter(PRIVATE_KEY_PATH, true);
			privateKeyWriter.WriteLine(keypair.getPrivateKey());
			privateKeyWriter.Close();
		}
	}
}
