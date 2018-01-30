using System;

namespace com.tvd12.ezyfoxserver.client.exception
{
	public class EzyMaxRequestSizeException : Exception
	{
		public EzyMaxRequestSizeException(String msg) 
			: base(msg)
		{
		}

		public EzyMaxRequestSizeException(int size, int maxSize) 
			: this("size = " + size + " when max size = " + maxSize)
		{
		}
	}
}
