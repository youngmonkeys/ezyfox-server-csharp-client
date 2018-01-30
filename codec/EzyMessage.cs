using System;

namespace com.tvd12.ezyfoxserver.client.codec
{
	public interface EzyMessage
	{
		int getSize();

		byte[] getContent();

		EzyMessageHeader getHeader();

		int getByteCount();

		int getSizeLength();

		bool hasBigSize();
	}
}
