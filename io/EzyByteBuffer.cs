using System;
using System.IO;
using System.Text;

namespace com.tvd12.ezyfoxserver.client.io
{
	public class EzyByteBuffer
	{
		protected readonly MemoryStream stream;

		public EzyByteBuffer() : this(new MemoryStream(32768))
		{
		}

		public EzyByteBuffer(MemoryStream stream)
		{
			this.stream = stream;
		}

		public static EzyByteBuffer wrap(MemoryStream stream)
		{
			return new EzyByteBuffer(stream);
		}

		public static EzyByteBuffer allocate(int capacity)
		{
			return new EzyByteBuffer(new MemoryStream(capacity));
		}

		public byte get()
		{
			return (byte)stream.ReadByte();
		}

		public void get(byte[] dst)
		{
			get(dst, 0, dst.Length);
		}

		public void get(byte[] dst, int offset, int length)
		{
			int end = offset + length;
			for (int i = offset; i < end; i++)
				dst[i] = get();
		}

		public byte[] getBytes(int size)
		{
			byte[] bytes = new byte[size];
			get(bytes, 0, size);
			return bytes;
		}

		public float getFloat()
		{
			return BitConverter.ToSingle(getBytes(4), 0);
		}

		public double getDouble()
		{
			return BitConverter.ToDouble(getBytes(8), 0);
		}

		public int getInt()
		{
			return getInt(4);
		}

		public int getInt(int byteSize)
		{
			return EzyInts.bin2int(byteSize);
		}

		public int getUInt(int byteSize)
		{
			return EzyInts.bin2uint(getBytes(byteSize));
		}

		public long getLong()
		{
			return getLong(8);
		}

		public long getLong(int byteSize)
		{
			return EzyLongs.bin2long(byteSize);
		}

		public long getULong(int byteSize)
		{
			return EzyLongs.bin2ulong(getBytes(byteSize));
		}

		public short getShort()
		{
			return (short)getInt(2);
		}

		public String getStringUtf(int length)
		{
			return Encoding.UTF8.GetString(getBytes(length));
		}

		public void put(byte b)
		{
			stream.WriteByte(b);
		}

		public void put(byte[] src)
		{
			put(src, 0, src.Length);
		}

		public void put(byte[] src, int offset, int length)
		{
			stream.Write(src, offset, length);
		}

		public void putInt(int value)
		{
			put(BitConverter.GetBytes(value));
		}

		public void putShort(short value)
		{
			put(BitConverter.GetBytes(value));
		}

		public void flip()
		{
		}

		public void clear()
		{
			stream.SetLength(0);
		}

		public int position()
		{
			return (int)stream.Position;
		}

		public void position(int pos)
		{
			stream.Position = pos;
		}

		public int remaining()
		{
			return (int)(stream.Length - stream.Position);
		}

		public override string ToString()
		{
			return stream.ToString();
		}
	}
}
