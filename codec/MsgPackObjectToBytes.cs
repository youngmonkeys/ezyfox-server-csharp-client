using System;
using com.tvd12.ezyfoxserver.client.builder;

namespace com.tvd12.ezyfoxserver.client.codec
{
	public class MsgPackObjectToBytes : EzyObjectToBytes
	{
		private readonly EzyMessageSerializer serializer;

		public MsgPackObjectToBytes(Builder builder)
		{
			this.serializer = builder.serializer;
		}

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

		public static Builder builder()
		{
			return new Builder();
		}

		public class Builder : EzyBuilder<EzyObjectToBytes>
		{
			public EzyMessageSerializer serializer;

			public Builder setSerializer(EzyMessageSerializer serializer)
			{
				this.serializer = serializer;
				return this;
			}

			public EzyObjectToBytes build()
			{
				return new MsgPackObjectToBytes(this);
			}
		}
	}

}
