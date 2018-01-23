using System;

namespace com.tvd12.ezyfoxserver.client.codec
{
	public abstract class EzyAbstractToBytesSerializer : EzyAbstractSerializer<byte[]>
	{
		public override byte[] serialize(Object value)
		{
			return value == null
					? parseNil()
					: parseNotNull(value);
		} 
	}
}
