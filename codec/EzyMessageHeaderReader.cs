using System;

namespace com.tvd12.ezyfoxserver.client.codec
{
	public class EzyMessageHeaderReader
	{
		protected bool readBigSize(byte header)
		{
			return (header & 1 << 0) != 0;
		}

		protected bool readEncrypted(byte header)
		{
			return (header & (1 << 1)) != 0;
		}

		protected bool readCompressed(byte header)
		{
			return (header & (1 << 2)) != 0;
		}

		protected bool readText(byte header)
		{
			return (header & (1 << 3)) != 0;
		}

		public EzyMessageHeader read(byte header)
		{
			return EzyMessageHeaderBuilder.newInstance()
					.setBigSize(readBigSize(header))
					.setEncrypted(readEncrypted(header))
					.setCompressed(readCompressed(header))
					.setText(readText(header))
					.build();
		}
	}
}
