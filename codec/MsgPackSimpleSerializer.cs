using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.io;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.function;
using static com.tvd12.ezyfoxserver.client.codec.MsgPackConstant;
using static com.tvd12.ezyfoxserver.client.constant.EzyTypes;

namespace com.tvd12.ezyfoxserver.client.codec
{
	public class MsgPackSimpleSerializer : EzyAbstractToBytesSerializer
	{

		private IntSerializer intSerializer = new IntSerializer();
		private FloatSerializer floatSerializer = new FloatSerializer();
		private DoubleSerializer doubleSerializer = new DoubleSerializer();
		private BinSizeSerializer binSizeSerializer = new BinSizeSerializer();
		private MapSizeSerializer mapSizeSerializer = new MapSizeSerializer();
		private ArraySizeSerializer arraySizeSerializer = new ArraySizeSerializer();
		private StringSizeSerializer stringSizeSerializer = new StringSizeSerializer();

		protected override void addParsers(Dictionary<Type, EzyParser<Object, Object>> parsers)
		{
			parsers.Add(PRIMITVE_BOOLEAN, input => parseBoolean(input));
			parsers.Add(PRIMITVE_BYTE, input => parseByte(input));
			parsers.Add(PRIMITVE_CHAR, input => parseChar(input));
			parsers.Add(PRIMITVE_DOUBLE, input => parseDouble(input));
			parsers.Add(PRIMITVE_FLOAT, input => parseFloat(input));
			parsers.Add(PRIMITVE_INT, input => parseInt(input));
			parsers.Add(PRIMITVE_LONG, input => parseInt(input));
			parsers.Add(PRIMITVE_SHORT, input => parseShort(input));
			parsers.Add(PRIMITVE_STRING, input => parseString(input));

			parsers.Add(PRIMITVE_BOOLEAN_ARRAY, input => parsePrimitiveBooleans(input));
			parsers.Add(PRIMITVE_BYTE_ARRAY, input => parseBin(input));
			parsers.Add(PRIMITVE_CHAR_ARRAY, input => parsePrimitiveChars(input));
			parsers.Add(PRIMITVE_DOUBLE_ARRAY, input => parsePrimitiveDoubles(input));
			parsers.Add(PRIMITVE_FLOAT_ARRAY, input => parsePrimitiveFloats(input));
			parsers.Add(PRIMITVE_INT_ARRAY, input => parsePrimitiveInts(input));
			parsers.Add(PRIMITVE_LONG_ARRAY, input => parsePrimitiveLongs(input));
			parsers.Add(PRIMITVE_SHORT_ARRAY, input => parsePrimitiveShorts(input));
			parsers.Add(PRIMITVE_STRING_ARRAY, input => parseStrings(input));

			parsers.Add(EZY_OBJECT_TYPE, input => parseObject(input));
			parsers.Add(EZY_ARRAY_TYPE, input => parseArray(input));
		}

		//
		protected byte[] parsePrimitiveBooleans(Object array)
		{
			return parseBooleans((bool[])array);
		}

		protected byte[] parsePrimitiveChars(Object array)
		{
			return parseChars((char[])array);
		}

		protected byte[] parsePrimitiveDoubles(Object array)
		{
			return parseDoubles((double[])array);
		}

		protected byte[] parsePrimitiveFloats(Object array)
		{
			return parseFloats((float[])array);
		}

		protected byte[] parsePrimitiveInts(Object array)
		{
			return parseInts((int[])array);
		}

		protected byte[] parsePrimitiveLongs(Object array)
		{
			return parseLongs((long[])array);
		}

		protected byte[] parsePrimitiveShorts(Object array)
		{
			return parseShorts((short[])array);
		}

		protected byte[] parseStrings(Object array)
		{
			return parseStrings((String[])array);
		}
		//

		protected byte[] parseBooleans(bool[] array)
		{
			return parseArray<bool>(array);
		}

		protected byte[] parseChars(char[] array)
		{
			return parseBin(array);
		}

		protected byte[] parseDoubles(double[] array)
		{
			return parseArray<double>(array);
		}

		protected byte[] parseFloats(float[] array)
		{
			return parseArray<float>(array);
		}

		protected byte[] parseInts(int[] array)
		{
			return parseArray<int>(array);
		}

		protected byte[] parseLongs(long[] array)
		{
			return parseArray<long>(array);
		}

		protected byte[] parseShorts(short[] array)
		{
			return parseArray<short>(array);
		}

		protected byte[] parseStrings(String[] array)
		{
			return parseArray<String>(array);
		}

		//=============
		protected byte[] parseWrapperBooleans(Object array)
		{
			return parseBooleans((Boolean[])array);
		}

		protected byte[] parseWrapperBytes(Object array)
		{
			return parseBin(array);
		}

		protected byte[] parseWrapperChars(Object array)
		{
			return parseChars((Char[])array);
		}

		protected byte[] parseWrapperDoubles(Object array)
		{
			return parseDoubles((Double[])array);
		}

		protected byte[] parseWrapperInts(Object array)
		{
			return parseInts((int[])array);
		}

		protected byte[] parseWrapperLongs(Object array)
		{
			return parseLongs((long[])array);
		}

		protected byte[] parseWrapperShorts(Object array)
		{
			return parseShorts((short[])array);
		}

		//============
		protected byte[] parseBoolean(Object value)
		{
			return parseBoolean((Boolean)value);
		}

		protected byte[] parseBoolean(Boolean value)
		{
			return value ? parseTrue() : parseFalse();
		}

		protected byte[] parseFalse()
		{
			return new byte[] { cast(0xc2) };
		}

		protected byte[] parseTrue()
		{
			return new byte[] { cast(0xc3) };
		}

		protected byte[] parseByte(Object value)
		{
			return parseByte(Convert.ToByte(value));
		}

		protected byte[] parseByte(Byte value)
		{
			return parseInt(value);
		}

		protected byte[] parseChar(Object value)
		{
			return parseChar((Char)value);
		}

		protected byte[] parseChar(Char value)
		{
			return parseByte(value);
		}

		protected byte[] parseDouble(Object value)
		{
			return parseDouble((Double)value);
		}

		protected byte[] parseDouble(Double value)
		{
			return doubleSerializer.serialize(value);
		}

		protected byte[] parseFloat(Object value)
		{
			return parseFloat((float)value);
		}

		protected byte[] parseFloat(float value)
		{
			return floatSerializer.serialize(value);
		}

		protected byte[] parseInt(Object value)
		{
			return intSerializer.serialize(Convert.ToInt64(value));
		}

		protected byte[] parseShort(Object value)
		{
			return parseShort((short)value);
		}

		protected byte[] parseShort(short value)
		{
			return parseInt(value);
		}

		protected byte[] parseString(Object value)
		{
			return parseString((String)value);
		}


		protected byte[] parseObject(Object obj)
		{
			return parseObject(((EzyObject)obj));
		}

		protected byte[] parseObject(EzyObject obj)
		{
			int index = 1;
			int size = obj.size();
			byte[][] bytess = new byte[size * 2 + 1][];
			bytess[0] = parseMapSize(size);
			foreach (Object key in obj.keys())
			{
				bytess[index++] = serialize(key);
				bytess[index++] = serialize(obj.get<Object>(key));
			}
			return EzyBytes.merge(bytess);
		}

		protected byte[] parseArray(Object array)
		{
			return parseArray((EzyArray)array);
		}


		protected byte[] parseArray(EzyArray array)
		{
			int index = 1;
			int size = array.size();
			byte[][] bytess = new byte[size + 1][];
			bytess[0] = parseArraySize(size);
			for (int i = 0; i < size; ++i)
			{
				bytess[index++] = serialize(array.get<Object>(i));
			}
			return EzyBytes.merge(bytess);
		}

		protected byte[] parseArray<T>(T[] array)
		{
			int index = 1;
			int size = array.Length;
			byte[][] bytess = new byte[size + 1][];
			bytess[0] = parseArraySize(size);
			for (int i = 0; i < size; ++i)
			{
				bytess[index++] = serialize(array[i]);
			}
			return EzyBytes.merge(bytess);
		}

		protected override byte[] parseNil()
		{
			return new byte[] { cast(0xc0) };
		}

		protected byte[] parseBin(Object bin)
		{
			return parseBin((byte[])bin);
		}

		protected byte[] parseBin(byte[] bin)
		{
			byte[][] bytess = new byte[2][];
			bytess[0] = parseBinSize(bin.Length);
			bytess[1] = bin;
			return EzyBytes.merge(bytess);
		}

		protected byte[] parseBinSize(int size)
		{
			return binSizeSerializer.serialize(size);
		}

		protected byte[] parseString(String str)
		{
			byte[][] bytess = new byte[2][];
			bytess[1] = EzyStrings.getUtfBytes(str);
			bytess[0] = parseStringSize(bytess[1].Length);
			return EzyBytes.merge(bytess);
		}

		protected byte[] parseStringSize(int size)
		{
			return stringSizeSerializer.serialize(size);
		}

		protected byte[] parseArraySize(int size)
		{
			return arraySizeSerializer.serialize(size);
		}

		protected byte[] parseMapSize(int size)
		{
			return mapSizeSerializer.serialize(size);
		}

		protected byte cast(int value)
		{
			return (byte)value;
		}

		protected byte cast(long value)
		{
			return (byte)value;
		}
			
	}

	class BinSizeSerializer : EzyCastToByte
	{
		public byte[] serialize(int size)
		{
			if (size <= MAX_BIN8_SIZE)
				return parse8(size);
			if (size <= MAX_BIN16_SIZE)
				return parse16(size);
			return parse32(size);
		}

		private byte[] parse8(int size)
		{
			return new byte[] {
					cast(0xc4), cast(size)
				};
		}

		private byte[] parse16(int size)
		{
			return EzyBytes.getBytes(0xc5, size, 2);
		}

		private byte[] parse32(int size)
		{
			return EzyBytes.getBytes(0xc6, size, 4);
		}
	}

	class StringSizeSerializer : EzyCastToByte
	{
		public byte[] serialize(int size)
		{
			if (size <= MAX_FIXSTR_SIZE)
				return parseFix(size);
			if (size <= MAX_STR8_SIZE)
				return parse8(size);
			if (size <= MAX_STR16_SIZE)
				return parse16(size);
			return parse32(size);
		}

		private byte[] parseFix(int size)
		{
			return new byte[] {
					cast(0xa0 | size)
				};
		}

		private byte[] parse8(int size)
		{
			return EzyBytes.getBytes(0xd9, size, 1);
		}

		private byte[] parse16(int size)
		{
			return EzyBytes.getBytes(0xda, size, 2);
		}

		private byte[] parse32(int size)
		{
			return EzyBytes.getBytes(0xdb, size, 4);
		}
	}

	class ArraySizeSerializer : EzyCastToByte
	{
		public byte[] serialize(int size)
		{
			if (size <= MAX_FIXARRAY_SIZE)
				return parseFix(size);
			if (size <= MAX_ARRAY16_SIZE)
				return parse16(size);
			return parse32(size);
		}

		private byte[] parseFix(int size)
		{
			return new byte[] {
					cast(0x90 | size)
				};
		}

		private byte[] parse16(int size)
		{
			return EzyBytes.getBytes(0xdc, size, 2);
		}

		private byte[] parse32(int size)
		{
			return EzyBytes.getBytes(0xdd, size, 4);
		}
	}

	class MapSizeSerializer : EzyCastToByte
	{
		public byte[] serialize(int size)
		{
			if (size <= MAX_FIXMAP_SIZE)
				return parseFix(size);
			if (size <= MAX_MAP16_SIZE)
				return parse16(size);
			return parse32(size);
		}

		private byte[] parseFix(int size)
		{
			return new byte[] {
					cast(0x80 | size)
				};
		}

		private byte[] parse16(int size)
		{
			return EzyBytes.getBytes(0xde, size, 2);
		}

		private byte[] parse32(int size)
		{
			return EzyBytes.getBytes(0xdf, size, 4);
		}
	}

	class IntSerializer : EzyCastToByte
	{
		public byte[] serialize(long value)
		{
			return value >= 0
					? parsePositive(value)
					: parseNegative(value);
		}

		private byte[] parsePositive(long value)
		{
			if (value <= MAX_POSITIVE_FIXINT)
				return parsePositiveFix(value);
			if (value < MAX_UINT8)
				return parseU8(value);
			if (value < MAX_BIN16_SIZE)
				return parseU16(value);
			if (value < MAX_BIN32_SIZE)
				return parseU32(value);
			return parseU64(value);
		}

		private byte[] parsePositiveFix(long value)
		{
			return new byte[] { cast(0x0 | value) };
		}

		private byte[] parseU8(long value)
		{
			return EzyBytes.getBytes(0xcc, value, 1);
		}

		private byte[] parseU16(long value)
		{
			return EzyBytes.getBytes(0xcd, value, 2);
		}

		private byte[] parseU32(long value)
		{
			return EzyBytes.getBytes(0xce, value, 4);
		}

		private byte[] parseU64(long value)
		{
			return EzyBytes.getBytes(0xcf, value, 8);
		}

		private byte[] parseNegative(long value)
		{
			if (value >= MIN_NEGATIVE_FIXINT)
				return parseNegativeFix(value);
			if (value >= MIN_INT8)
				return parse8(value);
			if (value >= MIN_INT16)
				return parse16(value);
			if (value >= MIN_INT32)
				return parse32(value);
			return parse64(value);
		}

		private byte[] parseNegativeFix(long value)
		{
			return new byte[] { cast(0xe0 | value) };
		}

		private byte[] parse8(long value)
		{
			return EzyBytes.getBytes(0xd0, value, 1);
		}

		private byte[] parse16(long value)
		{
			return EzyBytes.getBytes(0xd1, value, 2);
		}

		private byte[] parse32(long value)
		{
			return EzyBytes.getBytes(0xd2, value, 4);
		}

		private byte[] parse64(long value)
		{
			return EzyBytes.getBytes(0xd3, value, 8);
		}
	}

	class DoubleSerializer : EzyCastToByte
	{
		public byte[] serialize(double value)
		{
			return EzyBytes.getBytes(cast(0xcb), value);
		}
			
	}

	class FloatSerializer : EzyCastToByte
	{

		public byte[] serialize(float value)
		{
			return EzyBytes.getBytes(cast(0xca), value);
		}
			
	}

}
