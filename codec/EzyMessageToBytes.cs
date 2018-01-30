namespace com.tvd12.ezyfoxserver.client.codec
{
	public interface EzyMessageToBytes
	{
		byte[] convert(EzyMessage message);
	}
}
