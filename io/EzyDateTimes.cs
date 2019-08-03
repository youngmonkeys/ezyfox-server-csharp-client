using System;
namespace com.tvd12.ezyfoxserver.client.io
{
    public sealed class EzyDateTimes
    {
        public const String FORMAT_YY_MM = "yyyy-MM-dd";
        public const String FORMAT_HH_MM_SS_ZZZ = "HH:mm:ss.fff";
        public const String FORMAT_YY_MM_DD_HH_MM_SS_ZZZ = "yyyy-MM-dd HH:mm:ss.fff";

        private EzyDateTimes()
        {
        }

        public static long getOffsetMillis(DateTime from, DateTime to) {
            TimeSpan offset = to - from;
            long answer = (long)offset.TotalMilliseconds;
            return answer;
        }

        public static String format(DateTime dateTime, String format)
        {
            return String.Format("{0:" + format + "}", dateTime);
        }
    }
}
