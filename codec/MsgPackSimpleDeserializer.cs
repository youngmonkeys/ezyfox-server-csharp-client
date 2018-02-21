using System;
using System.IO;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.io;
using com.tvd12.ezyfoxserver.client.util;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.builder;
using com.tvd12.ezyfoxserver.client.function;

namespace com.tvd12.ezyfoxserver.client.codec
{
	public class MsgPackSimpleDeserializer : EzyEntityBuilders, EzyMessageDeserializer
	{
		private Dictionary<int, EzyParser<EzyByteBuffer, Object>> parsers;
		private MsgPackTypeParser typeParser = new MsgPackTypeParser();
		private MapSizeDeserializer mapSizeDeserializer = new MapSizeDeserializer();
		private ArraySizeDeserializer arraySizeDeserializer = new ArraySizeDeserializer();
		private StringSizeDeserializer stringSizeDeserializer = new StringSizeDeserializer();

		public MsgPackSimpleDeserializer()
		{
			this.parsers = defaultParsers();
		}

		public T deserialize<T>(byte[] data)
		{
			return deserialize<T>(new MemoryStream(data));
		}

		public T deserialize<T>(MemoryStream stream)
		{
			Object answer = deserialize(EzyByteBuffer.wrap(stream));
			return (T)answer;
		}

		public Object deserialize(EzyByteBuffer buffer)
		{
			return deserialize(buffer, buffer.get() & 0xff);
		}

		protected Object deserialize(EzyByteBuffer buffer, int header)
		{
			updateBufferPosition(buffer);
			int type = getDataType(header);
			var parser = parsers[type];
			return parser(buffer);
		}

		protected Dictionary<int, EzyParser<EzyByteBuffer, Object>> defaultParsers()
		{
			var parsers = new Dictionary<int, EzyParser<EzyByteBuffer, Object>>();
			addParsers(parsers);
			return parsers;
		}

		protected void addParsers(Dictionary<int, EzyParser<EzyByteBuffer, Object>> parsers)
		{
			parsers.Add(MsgPackType.POSITIVE_FIXINT, buffer => parsePositiveFixInt(buffer)); //0K
			parsers.Add(MsgPackType.NEGATIVE_FIXINT, buffer => parseNegativeFixInt(buffer)); //OK
			parsers.Add(MsgPackType.UINT8, buffer => parseUInt8(buffer)); //OK
			parsers.Add(MsgPackType.UINT16, buffer => parseUInt16(buffer)); //OK
			parsers.Add(MsgPackType.UINT32, buffer => parseUInt32(buffer)); //OK
			parsers.Add(MsgPackType.UINT64, buffer => parseUInt64(buffer)); //OK
			parsers.Add(MsgPackType.INT8, buffer => parseInt8(buffer)); //OK
			parsers.Add(MsgPackType.INT16, buffer => parseInt16(buffer)); //OK
			parsers.Add(MsgPackType.INT32, buffer => parseInt32(buffer)); //OK
			parsers.Add(MsgPackType.INT64, buffer => parseInt64(buffer)); //OK
			parsers.Add(MsgPackType.FIXMAP, buffer => parseFixMap(buffer)); //OK
			parsers.Add(MsgPackType.MAP16, buffer => parseMap16(buffer)); //OK
			parsers.Add(MsgPackType.MAP32, buffer => parseMap32(buffer)); //OK
			parsers.Add(MsgPackType.FIXARRAY, buffer => parseFixArray(buffer)); //OK
			parsers.Add(MsgPackType.ARRAY16, buffer => parseArray16(buffer)); //OK
			parsers.Add(MsgPackType.ARRAY32, buffer => parseArray32(buffer)); //OK
			parsers.Add(MsgPackType.FIXSTR, buffer => parseFixStr(buffer));
			parsers.Add(MsgPackType.STR8, buffer => parseStr8(buffer));
			parsers.Add(MsgPackType.STR16, buffer => parseStr16(buffer));
			parsers.Add(MsgPackType.STR32, buffer => parseStr32(buffer));
			parsers.Add(MsgPackType.NIL, buffer => parseNil(buffer));
			parsers.Add(MsgPackType.FALSE, buffer => parseFalse(buffer));
			parsers.Add(MsgPackType.TRUE, buffer => parseTrue(buffer));
			parsers.Add(MsgPackType.BIN8, buffer => parseBin8(buffer));
			parsers.Add(MsgPackType.BIN16, buffer => parseBin16(buffer));
			parsers.Add(MsgPackType.BIN32, buffer => parseBin32(buffer));
			parsers.Add(MsgPackType.FLOAT32, buffer => parseFloat32(buffer));
			parsers.Add(MsgPackType.FLOAT64, buffer => parseFloat64(buffer));
		}

		protected Object parseFloat32(EzyByteBuffer buffer)
		{
			buffer.get();
			return buffer.getFloat();
		}

		protected Object parseFloat64(EzyByteBuffer buffer)
		{
			buffer.get();
			return buffer.getDouble();
		}

		protected Object parseBin32(EzyByteBuffer buffer)
		{
			return parseBin(buffer, getBinLength(buffer, 4));
		}

		protected Object parseBin16(EzyByteBuffer buffer)
		{
			return parseBin(buffer, getBinLength(buffer, 2));
		}

		protected Object parseBin8(EzyByteBuffer buffer)
		{
			return parseBin(buffer, getBinLength(buffer, 1));
		}

		protected int getBinLength(EzyByteBuffer buffer, int size)
		{
			buffer.get();
			return buffer.getInt(size);
		}

		protected Object parseBin(EzyByteBuffer buffer, int length)
		{
			return buffer.getBytes(length);
		}

		protected Object parseTrue(EzyByteBuffer buffer)
		{
			return parseValue(buffer, true);
		}

		protected Object parseFalse(EzyByteBuffer buffer)
		{
			return parseValue(buffer, false);
		}

		protected Object parseNil(EzyByteBuffer buffer)
		{
			return parseValue(buffer, null);
		}

		protected Object parseValue(EzyByteBuffer buffer, Object value)
		{
			buffer.get();
			return value;
		}

		protected EzyObject parseFixMap(EzyByteBuffer buffer)
		{
			return parseMap(buffer, getMapSize(buffer, 1));
		}

		protected EzyObject parseMap16(EzyByteBuffer buffer)
		{
			return parseMap(buffer, getMapSize(buffer, 3));
		}

		protected EzyObject parseMap32(EzyByteBuffer buffer)
		{
			return parseMap(buffer, getMapSize(buffer, 5));
		}

		protected int getMapSize(EzyByteBuffer buffer, int nbytes)
		{
			return mapSizeDeserializer.deserialize(buffer, nbytes);
		}

		protected EzyObject parseMap(EzyByteBuffer buffer, int size)
		{
			EzyObjectBuilder builder = newObjectBuilder();
			for (int i = 0; i < size; i++)
				builder.append(deserialize(buffer), deserialize(buffer));
			return builder.build();
		}

		protected String parseStr32(EzyByteBuffer buffer)
		{
			return parseString(buffer, 5);
		}

		protected String parseStr16(EzyByteBuffer buffer)
		{
			return parseString(buffer, 3);
		}

		protected String parseStr8(EzyByteBuffer buffer)
		{
			return parseString(buffer, 2);
		}

		protected String parseFixStr(EzyByteBuffer buffer)
		{
			return parseString(buffer, 1);
		}

		protected String parseString(EzyByteBuffer buffer, int nbytes)
		{
			return buffer.getStringUtf(parseStringSize(buffer, nbytes));
		}

		protected int parseStringSize(EzyByteBuffer buffer, int nbytes)
		{
			return stringSizeDeserializer.deserialize(buffer, nbytes);
		}

		protected int parsePositiveFixInt(EzyByteBuffer buffer)
		{
			return (buffer.get() & 0x7F);
		}

		protected int parseNegativeFixInt(EzyByteBuffer buffer)
		{
			return buffer.get();
		}

		protected int parseUInt8(EzyByteBuffer buffer)
		{
			return parseUInt(buffer, 1);
		}

		protected int parseUInt16(EzyByteBuffer buffer)
		{
			return parseUInt(buffer, 2);
		}

		protected int parseUInt32(EzyByteBuffer buffer)
		{
			return parseUInt(buffer, 4);
		}

		protected long parseUInt64(EzyByteBuffer buffer)
		{
			return parseULong(buffer, 8);
		}

		protected int parseInt8(EzyByteBuffer buffer)
		{
			return parseInt(buffer, 1);
		}

		protected int parseInt16(EzyByteBuffer buffer)
		{
			return parseInt(buffer, 2);
		}

		protected int parseInt32(EzyByteBuffer buffer)
		{
			return parseInt(buffer, 4);
		}

		protected long parseInt64(EzyByteBuffer buffer)
		{
			return parseLong(buffer, 8);
		}

		protected int parseUInt(EzyByteBuffer buffer, int size)
		{
			return (int)parseULong(buffer, size);
		}

		protected long parseULong(EzyByteBuffer buffer, int size)
		{
			buffer.get();
			return buffer.getULong(size);
		}

		protected int parseInt(EzyByteBuffer buffer, int size)
		{
			return (int)parseLong(buffer, size);
		}

		protected long parseLong(EzyByteBuffer buffer, int size)
		{
			buffer.get();
			return buffer.getLong(size);
		}

		protected EzyArray parseFixArray(EzyByteBuffer buffer)
		{
			return parseArray(buffer, parseArraySize(buffer, 1));
		}

		protected EzyArray parseArray16(EzyByteBuffer buffer)
		{
			return parseArray(buffer, parseArraySize(buffer, 3));
		}

		protected EzyArray parseArray32(EzyByteBuffer buffer)
		{
			return parseArray(buffer, parseArraySize(buffer, 5));
		}

		protected int parseArraySize(EzyByteBuffer buffer, int nbytes)
		{
			return arraySizeDeserializer.deserialize(buffer, nbytes);
		}

		protected EzyArray parseArray(EzyByteBuffer buffer, int size)
		{
			EzyArrayBuilder builder = newArrayBuilder();
			for (int i = 0; i < size; i++)
				builder.append(deserialize(buffer));
			return builder.build();
		}

		protected int getDataType(int type)
		{
			return typeParser.parse(type);
		}

		protected void updateBufferPosition(EzyByteBuffer buffer)
		{
			updateBufferPosition(buffer, -1);
		}

		protected void updateBufferPosition(EzyByteBuffer buffer, int offset)
		{
			buffer.position(buffer.position() + offset);
		}

	}

	abstract class AbstractSizeDeserializer
	{

		public int deserialize(EzyByteBuffer buffer, int nbytes)
		{
			return nbytes == 1
					? getFix(buffer)
					: getOther(buffer, nbytes);
		}

		protected abstract int getFix(EzyByteBuffer buffer);

		protected int getOther(EzyByteBuffer buffer, int nbytes)
		{
			buffer.get();
			return buffer.getUInt(nbytes - 1);
		}
	}

	class StringSizeDeserializer : AbstractSizeDeserializer
	{

		protected override int getFix(EzyByteBuffer buffer)
		{
			return buffer.get() & 0x1F;
		}

	}

	class MapSizeDeserializer : AbstractSizeDeserializer
	{

		protected override int getFix(EzyByteBuffer buffer)
		{
			return buffer.get() & 0xF;
		}

	}

	class ArraySizeDeserializer : AbstractSizeDeserializer
	{

		protected override int getFix(EzyByteBuffer buffer)
		{
			return buffer.get() & 0xF;
		}

	}
}
