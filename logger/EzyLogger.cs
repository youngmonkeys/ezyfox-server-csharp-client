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

    public enum EzyLoggerLevel
    {

        TRACE = 1,
        DEBUG = 2,
        INFO = 3,
        WARN = 4,
        ERROR = 5

    }
}
