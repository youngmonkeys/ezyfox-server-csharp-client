using System;
using System.Text;
using System.Threading;
using System.Collections.Generic;

using static com.tvd12.ezyfoxserver.client.reflect.EzyClasses;
using static com.tvd12.ezyfoxserver.client.logger.EzyLoggerLevel;

using static com.tvd12.ezyfoxserver.client.io.EzyDateTimes;

namespace com.tvd12.ezyfoxserver.client.logger
{
	public delegate EzyLogger EzyLoggerSupply(object name);

	public class EzyLoggerFactory
	{
		private static EzyLoggerSupply loggerSupply 
            = (name) => new EzySimpleLogger(name);

        private static readonly IDictionary<object, EzyLogger> loggers 
            = new Dictionary<object, EzyLogger>();

		public static EzyLogger getLogger()
		{
			return getLogger("DefaultLogger");
		}

        public static EzyLogger getLogger(object name)
		{
            EzyLogger logger = null;
            if(loggers.ContainsKey(name)) 
            {
                logger = loggers[name];
            }
            else 
            {
                lock (loggers)
                {
                    if(loggers.ContainsKey(name)) 
                    {
                        logger = loggers[name];   
                    }
                    else 
                    {
                        logger = loggerSupply(name);
                        loggers[name] = logger;
                    }
                }
            }
            return logger;
		}

        public static EzyLogger getLogger<T>() {
            return getLogger(typeof(T));
        }

		public static void setLoggerSupply(EzyLoggerSupply supply)
		{
			loggerSupply = supply;
		}
	}

	public class EzySimpleLogger : EzyLogger
	{
        protected object name;

        public EzySimpleLogger(object name)
		{
            this.name = (name is Type) ? getClassName((Type)name, 1) : name;
		}

		public void trace(String format, params Object[] args)
		{
            if(args.Length == 0)
                Console.WriteLine(standardizedMessage(TRACE, format));
            else
                Console.WriteLine(standardizedMessage(TRACE, format), args);
		}

		public void trace(String message, Exception e)
		{
            Console.WriteLine(standardizedMessage(TRACE, message) + "\n" + e);
		}

		public void debug(String format, params Object[] args)
		{
            if (args.Length == 0)
                Console.WriteLine(standardizedMessage(DEBUG, format));
            else
                Console.WriteLine(standardizedMessage(DEBUG, format), args);
		}

		public void debug(String message, Exception e)
		{
            Console.WriteLine(standardizedMessage(DEBUG, message) + "\n" + e);
		}

		public void info(String format, params Object[] args)
		{
            if (args.Length == 0)
                Console.WriteLine(standardizedMessage(INFO, format));
            else
                Console.WriteLine(standardizedMessage(INFO, format), args);
		}

		public void info(String message, Exception e)
		{
            Console.WriteLine(standardizedMessage(INFO, message) + "\n" + e);
		}

		public void warn(String format, params Object[] args)
		{
            if (args.Length == 0)
                Console.WriteLine(standardizedMessage(WARN, format));
            else
                Console.WriteLine(standardizedMessage(WARN, format), args);
		}

		public void warn(String message, Exception e)
		{
            Console.WriteLine(standardizedMessage(WARN, message) + "\n" + e);
		}

		public void error(String format, params Object[] args)
		{
            if (args.Length == 0)
                Console.WriteLine(standardizedMessage(ERROR, format));
            else
                Console.WriteLine(standardizedMessage(ERROR, format), args);
		}

		public void error(String message, Exception e)
		{
            Console.WriteLine(standardizedMessage(ERROR, message) + "\n" + e);
		}

        protected String standardizedMessage(String level, String message) {
            DateTime now = DateTime.Now;
            StringBuilder builder = new StringBuilder()
                .Append(format(now, FORMAT_YY_MM))
                .Append(" | ")
                .Append(format(now, FORMAT_HH_MM_SS_ZZZ))
                .Append(" | ")
                .Append(level)
                .Append(" | ")
                .Append(Thread.CurrentThread.Name)
                .Append(" | ")
                .Append(name)
                .Append(" | ")
                .Append(message);
            return builder.ToString();
        }
	}

}
