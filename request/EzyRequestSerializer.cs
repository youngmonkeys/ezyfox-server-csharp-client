using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.factory;
using com.tvd12.ezyfoxserver.client.constant;

namespace com.tvd12.ezyfoxserver.client.request
{
	public interface EzyRequestSerializer
	{
        EzyArray serialize(EzyCommand cmd, EzyArray data);
	}

    public class EzySimpleRequestSerializer : EzyRequestSerializer 
    {
        public virtual EzyArray serialize(EzyCommand cmd, EzyArray data) 
        {
            EzyArray array = EzyEntityFactory.newArrayBuilder()
                                             .append((int)cmd)
                                             .append(data)
                                             .build();
            return array;
        }
    }
}
