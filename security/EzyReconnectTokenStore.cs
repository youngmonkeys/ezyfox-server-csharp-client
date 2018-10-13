using System;
using System.IO;

namespace com.tvd12.ezyfoxserver.client.security
{
	public interface EzyReconnectTokenStore
	{
		void store(String reconnectToken);
	}

	public class EzySystemReconnectTokenStore : EzyReconnectTokenStore
	{
		private const String FOLDER = ".runtime";
		private const String PATH = FOLDER + "/reconnect_token.txt";

		public void store(String reconnectToken)
		{
			Directory.CreateDirectory(FOLDER);
			StreamWriter writer = new StreamWriter(PATH, true);
			writer.WriteLine(reconnectToken);
			writer.Close();
		}
	}
}
