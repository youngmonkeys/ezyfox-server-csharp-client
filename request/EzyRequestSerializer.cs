namespace com.tvd12.ezyfoxserver.client.request
{
	public interface EzyRequestSerializer<T>
	{
		T serialize(EzyRequest request);
	}
}
