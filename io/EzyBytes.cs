using System;

namespace com.tvd12.ezyfoxserver.client.io
{
	public sealed class EzyBytes
	{
		private EzyBytes()
		{
		}

		public static byte[] getBytes(int first, int value, int size)
		{
			return getBytes(first, (long)value, size);
		}

		public static byte[] getBytes(int first, long value, int size)
		{
			return getBytes((byte)first, value, size);
		}

		public static byte[] getBytes(byte first, long value, int size)
		{
			return merge(first, getBytes(value, size));
		}

		public static byte[] getBytes(byte first, double value)
		{
			return merge(first, getBytes(value));
		}

		public static byte[] getBytes(byte first, float value)
		{
			return merge(first, getBytes(value));
		}

		public static byte[] getBytes(double value)
		{
			return BitConverter.GetBytes(value);
		}

		public static byte[] getBytes(float value)
		{
			return BitConverter.GetBytes(value);
		}

		public static byte[] getBytes(long value)
		{
			return getBytes(value, 8);
		}

		public static byte[] getBytes(int value)
		{
			return getBytes(value, 4);
		}

		public static byte[] getBytes(short value)
		{
			return getBytes(value, 2);
		}

		public static byte[] merge(byte first, byte[] other)
		{
			byte[] bytes = new byte[other.Length + 1];
			bytes[0] = first;
			other.CopyTo(bytes, 1);
			return bytes;
		}

		public static byte[] merge(byte[][] bytess)
		{
			int position = 0;
			byte[] answer = new byte[totalBytes(bytess)];
			foreach (byte[] bytes in bytess)
			{
				bytes.CopyTo(answer, position);
				position += bytes.Length;
			}
			return answer;
		}

		public static int totalBytes(byte[][] bytess)
		{
			int size = 0;
			foreach (byte[] bytes in bytess)
				size += bytes.Length;
			return size;
		}

		public static byte[] getBytes(long value, int size)
		{
			byte[] bytes = new byte[size];
			for (int i = 0; i < size; i++)
				bytes[i] = (byte)((value >> ((size - i - 1) * 8) & 0xff));
			return bytes;
		}
	}
}
