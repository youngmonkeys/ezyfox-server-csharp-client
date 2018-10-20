using System;

namespace com.tvd12.ezyfoxserver.client.logger
{
	public delegate EzyLogger EzyLoggerSupply(Type type);

	public class EzyLoggerFactory
	{
		private static EzyLoggerSupply loggerSupply
				= (type) => new EzySimpleLogger(type);

		public static EzyLogger getLogger()
		{
			return getLogger(typeof(EzyLoggerFactory));
		}

		public static EzyLogger getLogger(Type type)
		{
			return loggerSupply(type);
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
		protected Type type;

		public EzySimpleLogger(Type type)
		{
			this.type = type;
		}

		public void trace(String format, params Object[] args)
		{
			Console.WriteLine("TRACE:: " + format, args);
		}

		public void trace(String message, Exception e)
		{
			Console.WriteLine("TRACE:: " + message + "\n" + e);
		}

		public void debug(String format, params Object[] args)
		{
			Console.WriteLine("DEBUG:: " + format, args);
		}

		public void debug(String message, Exception e)
		{
			Console.WriteLine("DEBUG:: " + message + "\n" + e);
		}

		public void info(String format, params Object[] args)
		{
			Console.WriteLine("INFO:: " + format, args);
		}

		public void info(String message, Exception e)
		{
			Console.WriteLine("INFO:: " + message + "\n" + e);
		}

		public void warn(String format, params Object[] args)
		{
			Console.WriteLine("WARN:: " + format, args);
		}

		public void warn(String message, Exception e)
		{
			Console.WriteLine("WARN:: " + message + "\n" + e);
		}

		public void error(String format, params Object[] args)
		{
			Console.WriteLine("ERROR:: " + format, args);
		}

		public void error(String message, Exception e)
		{
			Console.WriteLine("ERROR:: " + message + "\n" + e);
		}
	}

}
