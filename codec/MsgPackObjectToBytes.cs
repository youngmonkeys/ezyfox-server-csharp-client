using System;

namespace com.tvd12.ezyfoxserver.client.codec
{
	public class MsgPackObjectToBytes
	{
		private readonly EzyMessageSerializer serializer;

		public MsgPackObjectToBytes(EzyMessageSerializer serializer)
		{
			this.serializer = serializer;
		}

		public byte[] convert(Object value)
		{
			try
			{
				return serializer.serialize(value);
			}
			catch (Exception e)
			{
				throw new ArgumentException("serialize value: " + value + " error", e);
			}
		}
	}
}
