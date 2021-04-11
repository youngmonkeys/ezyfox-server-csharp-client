using System;

namespace com.tvd12.ezyfoxserver.client.io
{
	public sealed class EzyBytes
	{
		private EzyBytes()
		{
		}

		public static byte[] getBytes(int first, byte value)
		{
			return new byte[] { (byte)first, value };
		}

		public static byte[] getBytes(int first, int value)
		{
			return merge((byte)first, getBytes(value));
		}

		public static byte[] getBytes(int first, long value)
		{
			return merge((byte)first, getBytes(value));
		}

		public static byte[] getBytes(int first, short value)
		{
			return merge((byte)first, getBytes(value));
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
			byte[] bytes = BitConverter.GetBytes(value);
			swapBytes(bytes);
			return bytes;
		}

		public static byte[] getBytes(float value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			swapBytes(bytes);
			return bytes;
		}

		public static byte[] getBytes(long value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			swapBytes(bytes);
			return bytes;
		}

		public static byte[] getBytes(int value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			swapBytes(bytes);
			return bytes;
		}

		public static byte[] getBytes(short value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			swapBytes(bytes);
			return bytes;
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

		public static void swapBytes(byte[] bytes)
		{
			swapBytes(bytes, bytes.Length);
		}

		public static void swapBytes(byte[] bytes, int size)
		{
			for (int i = 0, k = size - 1; i < k; ++i, --k)
			{
				byte c = bytes[i];
				bytes[i] = bytes[k];
				bytes[k] = c;
			}
		}

        public static byte[] copyBytes(byte[] source, int size) 
        {
            return copyBytes(source, 0, size);
        }

        public static byte[] copyBytes(byte[] source, int offset, int size)
        {
            byte[] bytes = new byte[size];
            for (int i = 0; i < bytes.Length; ++i)
                bytes[i] = source[i + offset];
            return bytes;
        }
	}
}
