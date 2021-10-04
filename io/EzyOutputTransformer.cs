using System;
using com.tvd12.ezyfoxserver.client.constant;

namespace com.tvd12.ezyfoxserver.client.io
{
    public class EzyOutputTransformer
    {
        public T transform<T>(Object value)
        {
            Object result = transformByType(value, typeof(T));
            return (T)result;
        }

        public Object transformByType(Object value, Type outtype)
        {
            if (value == null)
            {
                return value;
            }
            var intype = value.GetType();

            if (intype == EzyTypes.PRIMITVE_SBYTE)
            {
                sbyte inValue = (sbyte)value;
                if(outtype == EzyTypes.PRIMITVE_SBYTE)
                {
                    return value;
                }
                if(outtype == EzyTypes.PRIMITVE_BYTE)
                {
                    return (Object)Convert.ToByte(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_DOUBLE)
                {
                    return (Object)Convert.ToDouble(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_FLOAT)
                {
                    return (Object)Convert.ToSingle(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_INT)
                {
                    return (Object)Convert.ToInt32(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_LONG)
                {
                    return (Object)Convert.ToInt64(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_SHORT)
                {
                    return (Object)Convert.ToInt16(inValue);
                }
            }
            else if (intype == EzyTypes.PRIMITVE_BYTE)
            {
                byte inValue = (byte)value;
                if (outtype == EzyTypes.PRIMITVE_SBYTE)
                {
                    return (Object)Convert.ToSByte(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_BYTE)
                {
                    return value;
                }
                if (outtype == EzyTypes.PRIMITVE_DOUBLE)
                {
                    return (Object)Convert.ToDouble(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_FLOAT)
                {
                    return (Object)Convert.ToSingle(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_INT)
                {
                    return (Object)Convert.ToInt32(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_LONG)
                {
                    return (Object)Convert.ToInt64(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_SHORT)
                {
                    return (Object)Convert.ToInt16(inValue);
                }
            }
            else if (intype == EzyTypes.PRIMITVE_DOUBLE)
            {
                double inValue = (double)value;
                if (outtype == EzyTypes.PRIMITVE_SBYTE)
                {
                    return (Object)Convert.ToSByte(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_BYTE)
                {
                    return (Object)Convert.ToByte(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_DOUBLE)
                {
                    return value;
                }
                if (outtype == EzyTypes.PRIMITVE_FLOAT)
                {
                    return (Object)Convert.ToSingle(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_INT)
                {
                    return (Object)Convert.ToInt32(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_LONG)
                {
                    return (Object)Convert.ToInt64(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_SHORT)
                {
                    return (Object)Convert.ToInt16(inValue);
                }
            }
            else if (intype == EzyTypes.PRIMITVE_FLOAT)
            {
                float inValue = (float)value;
                if (outtype == EzyTypes.PRIMITVE_SBYTE)
                {
                    return (Object)Convert.ToSByte(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_BYTE)
                {
                    return (Object)Convert.ToByte(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_DOUBLE)
                {
                    return (Object)Convert.ToDouble(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_FLOAT)
                {
                    return value;
                }
                if (outtype == EzyTypes.PRIMITVE_INT)
                {
                    return (Object)Convert.ToInt32(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_LONG)
                {
                    return (Object)Convert.ToInt64(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_SHORT)
                {
                    return (Object)Convert.ToInt16(inValue);
                }
            }
            else if (intype == EzyTypes.PRIMITVE_INT)
            {
                int inValue = (int)value;
                if (outtype == EzyTypes.PRIMITVE_SBYTE)
                {
                    return (Object)Convert.ToSByte(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_BYTE)
                {
                    return (Object)Convert.ToByte(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_DOUBLE)
                {
                    return (Object)Convert.ToDouble(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_FLOAT)
                {
                    return (Object)Convert.ToSingle(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_INT)
                {
                    return value;
                }
                if (outtype == EzyTypes.PRIMITVE_LONG)
                {
                    return (Object)Convert.ToInt64(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_SHORT)
                {
                    return (Object)Convert.ToInt16(inValue);
                }
            }
            else if (intype == EzyTypes.PRIMITVE_INT)
            {
                int inValue = (int)value;
                if (outtype == EzyTypes.PRIMITVE_SBYTE)
                {
                    return (Object)Convert.ToSByte(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_BYTE)
                {
                    return (Object)Convert.ToByte(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_DOUBLE)
                {
                    return (Object)Convert.ToDouble(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_FLOAT)
                {
                    return (Object)Convert.ToSingle(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_INT)
                {
                    return value;
                }
                if (outtype == EzyTypes.PRIMITVE_LONG)
                {
                    return (Object)Convert.ToInt64(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_SHORT)
                {
                    return (Object)Convert.ToInt16(inValue);
                }
            }
            else if (intype == EzyTypes.PRIMITVE_LONG)
            {
                long inValue = (long)value;
                if (outtype == EzyTypes.PRIMITVE_SBYTE)
                {
                    return (Object)Convert.ToSByte(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_BYTE)
                {
                    return (Object)Convert.ToByte(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_DOUBLE)
                {
                    return (Object)Convert.ToDouble(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_FLOAT)
                {
                    return (Object)Convert.ToSingle(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_INT)
                {
                    return (Object)Convert.ToInt32(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_LONG)
                {
                    return value;
                }
                if (outtype == EzyTypes.PRIMITVE_SHORT)
                {
                    return (Object)Convert.ToInt16(inValue);
                }
            }
            else if (intype == EzyTypes.PRIMITVE_SHORT)
            {
                short inValue = (short)value;
                if (outtype == EzyTypes.PRIMITVE_SBYTE)
                {
                    return (Object)Convert.ToSByte(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_BYTE)
                {
                    return (Object)Convert.ToByte(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_DOUBLE)
                {
                    return (Object)Convert.ToDouble(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_FLOAT)
                {
                    return (Object)Convert.ToDouble(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_INT)
                {
                    return (Object)Convert.ToInt32(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_LONG)
                {
                    return (Object)Convert.ToInt64(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_SHORT)
                {
                    return value;
                }
            }

            return value;
        }
    }
}
