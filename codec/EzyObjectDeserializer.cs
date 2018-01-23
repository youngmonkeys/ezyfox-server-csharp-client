using System.IO;

namespace com.tvd12.ezyfoxserver.client.codec
{
	public interface EzyObjectDeserializer
	{
		T deserialize<T>(byte[] data);

		T deserialize<T>(MemoryStream buffer);
	}
}
