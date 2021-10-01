using System;
using com.tvd12.ezyfoxserver.client.constant;

namespace com.tvd12.ezyfoxserver.client.io
{
    public class EzyOutputTransformer
    {

        public T transform<T>(Object value)
        {
            if (value == null)
            {
                return (T)value;
            }
            var intype = value.GetType();
            var outtype = typeof(T);

            if (intype == EzyTypes.PRIMITVE_SBYTE)
            {
                sbyte inValue = (sbyte)value;
                if(outtype == EzyTypes.PRIMITVE_SBYTE)
                {
                    return (T)value;
                }
                if(outtype == EzyTypes.PRIMITVE_BYTE)
                {
                    return (T)(Object)Convert.ToByte(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_DOUBLE)
                {
                    return (T)(Object)Convert.ToDouble(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_FLOAT)
                {
                    return (T)(Object)Convert.ToSingle(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_INT)
                {
                    return (T)(Object)Convert.ToInt32(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_LONG)
                {
                    return (T)(Object)Convert.ToInt64(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_SHORT)
                {
                    return (T)(Object)Convert.ToInt16(inValue);
                }
            }
            else if (intype == EzyTypes.PRIMITVE_BYTE)
            {
                byte inValue = (byte)value;
                if (outtype == EzyTypes.PRIMITVE_SBYTE)
                {
                    return (T)(Object)Convert.ToSByte(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_BYTE)
                {
                    return (T)value;
                }
                if (outtype == EzyTypes.PRIMITVE_DOUBLE)
                {
                    return (T)(Object)Convert.ToDouble(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_FLOAT)
                {
                    return (T)(Object)Convert.ToSingle(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_INT)
                {
                    return (T)(Object)Convert.ToInt32(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_LONG)
                {
                    return (T)(Object)Convert.ToInt64(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_SHORT)
                {
                    return (T)(Object)Convert.ToInt16(inValue);
                }
            }
            else if (intype == EzyTypes.PRIMITVE_DOUBLE)
            {
                double inValue = (double)value;
                if (outtype == EzyTypes.PRIMITVE_SBYTE)
                {
                    return (T)(Object)Convert.ToSByte(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_BYTE)
                {
                    return (T)(Object)Convert.ToByte(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_DOUBLE)
                {
                    return (T)value;
                }
                if (outtype == EzyTypes.PRIMITVE_FLOAT)
                {
                    return (T)(Object)Convert.ToSingle(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_INT)
                {
                    return (T)(Object)Convert.ToInt32(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_LONG)
                {
                    return (T)(Object)Convert.ToInt64(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_SHORT)
                {
                    return (T)(Object)Convert.ToInt16(inValue);
                }
            }
            else if (intype == EzyTypes.PRIMITVE_FLOAT)
            {
                float inValue = (float)value;
                if (outtype == EzyTypes.PRIMITVE_SBYTE)
                {
                    return (T)(Object)Convert.ToSByte(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_BYTE)
                {
                    return (T)(Object)Convert.ToByte(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_DOUBLE)
                {
                    return (T)(Object)Convert.ToDouble(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_FLOAT)
                {
                    return (T)value;
                }
                if (outtype == EzyTypes.PRIMITVE_INT)
                {
                    return (T)(Object)Convert.ToInt32(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_LONG)
                {
                    return (T)(Object)Convert.ToInt64(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_SHORT)
                {
                    return (T)(Object)Convert.ToInt16(inValue);
                }
            }
            else if (intype == EzyTypes.PRIMITVE_INT)
            {
                int inValue = (int)value;
                if (outtype == EzyTypes.PRIMITVE_SBYTE)
                {
                    return (T)(Object)Convert.ToSByte(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_BYTE)
                {
                    return (T)(Object)Convert.ToByte(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_DOUBLE)
                {
                    return (T)(Object)Convert.ToDouble(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_FLOAT)
                {
                    return (T)(Object)Convert.ToSingle(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_INT)
                {
                    return (T)value;
                }
                if (outtype == EzyTypes.PRIMITVE_LONG)
                {
                    return (T)(Object)Convert.ToInt64(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_SHORT)
                {
                    return (T)(Object)Convert.ToInt16(inValue);
                }
            }
            else if (intype == EzyTypes.PRIMITVE_INT)
            {
                int inValue = (int)value;
                if (outtype == EzyTypes.PRIMITVE_SBYTE)
                {
                    return (T)(Object)Convert.ToSByte(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_BYTE)
                {
                    return (T)(Object)Convert.ToByte(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_DOUBLE)
                {
                    return (T)(Object)Convert.ToDouble(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_FLOAT)
                {
                    return (T)(Object)Convert.ToSingle(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_INT)
                {
                    return (T)value;
                }
                if (outtype == EzyTypes.PRIMITVE_LONG)
                {
                    return (T)(Object)Convert.ToInt64(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_SHORT)
                {
                    return (T)(Object)Convert.ToInt16(inValue);
                }
            }
            else if (intype == EzyTypes.PRIMITVE_LONG)
            {
                long inValue = (long)value;
                if (outtype == EzyTypes.PRIMITVE_SBYTE)
                {
                    return (T)(Object)Convert.ToSByte(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_BYTE)
                {
                    return (T)(Object)Convert.ToByte(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_DOUBLE)
                {
                    return (T)(Object)Convert.ToDouble(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_FLOAT)
                {
                    return (T)(Object)Convert.ToSingle(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_INT)
                {
                    return (T)(Object)Convert.ToInt32(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_LONG)
                {
                    return (T)value;
                }
                if (outtype == EzyTypes.PRIMITVE_SHORT)
                {
                    return (T)(Object)Convert.ToInt16(inValue);
                }
            }
            else if (intype == EzyTypes.PRIMITVE_SHORT)
            {
                short inValue = (short)value;
                if (outtype == EzyTypes.PRIMITVE_SBYTE)
                {
                    return (T)(Object)Convert.ToSByte(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_BYTE)
                {
                    return (T)(Object)Convert.ToByte(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_DOUBLE)
                {
                    return (T)(Object)Convert.ToDouble(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_FLOAT)
                {
                    return (T)(Object)Convert.ToDouble(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_INT)
                {
                    return (T)(Object)Convert.ToInt32(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_LONG)
                {
                    return (T)(Object)Convert.ToInt64(inValue);
                }
                if (outtype == EzyTypes.PRIMITVE_SHORT)
                {
                    return (T)value;
                }
            }

            return (T)value;
        }
    }
}
