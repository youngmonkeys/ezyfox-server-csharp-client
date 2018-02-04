using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.request
{
	public class EzyArrayRequestSerializer : EzyRequestSerializer<EzyArray>
	{
		public EzyArray serialize(EzyRequest request)
		{
			var array = request.serialize();
			return array;
		}
	}
}
