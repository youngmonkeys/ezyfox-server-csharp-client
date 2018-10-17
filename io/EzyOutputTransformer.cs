using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.function;

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
            if (intype == EzyTypes.PRIMITVE_INT)
            {
                Int32 int32value = (Int32)value;
                if (outtype == EzyTypes.PRIMITVE_LONG)
                {
                    return (T)(Object)(Int64)int32value;
                }
            }
            else if (intype == EzyTypes.PRIMITVE_DOUBLE)
            {
                double doubleValue = (double)value;
                if (outtype == EzyTypes.PRIMITVE_FLOAT)
                {
                    return (T)(Object)(float)doubleValue;
                }
            }
            return (T)value;
        }
    }
}
