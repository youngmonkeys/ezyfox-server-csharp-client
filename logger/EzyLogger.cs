using System;

namespace com.tvd12.ezyfoxserver.client.logger
{

	public interface EzyLogger
	{
		void trace(String format, params Object[] args);

		void trace(String message, Exception e);

		void debug(String format, params Object[] args);

		void debug(String message, Exception e);

		void info(String format, params Object[] args);

		void info(String message, Exception e);

		void warn(String format, params Object[] args);

		void warn(String message, Exception e);

		void error(String format, params Object[] args);

		void error(String message, Exception e);
	}

    public sealed class EzyLoggerLevel
    {

        private EzyLoggerLevel() {
        }

        public static readonly String TRACE = "TRACE";
        public static readonly String DEBUG = "DEBUG";
        public static readonly String INFO = "INFO";
        public static readonly String WARN = "WARN";
        public static readonly String ERROR = "ERROR";

    }
}
