namespace com.tvd12.ezyfoxserver.client.codec
{
	public interface EzyMessageHeader
	{
		bool isBigSize(); //bit 1

		bool isEncrypted(); // bit 2

		bool isCompressed(); // bit 3

		bool isText(); // bit 4
	}

}
