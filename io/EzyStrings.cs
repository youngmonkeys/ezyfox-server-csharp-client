using System;
using System.Text;
namespace com.tvd12.ezyfoxserver.client.io
{
	public sealed class EzyStrings
	{
		private EzyStrings()
		{
		}

		public static String newUtf(byte[] bytes)
		{
			return Encoding.UTF8.GetString(bytes);
		}


		public static byte[] getUtfBytes(String str)
		{
			return Encoding.UTF8.GetBytes(str);
		}

		public static String getString(String[] array, int index, String def)
		{
			return array.Length > index ? array[index] : def;
			}
		}
}
