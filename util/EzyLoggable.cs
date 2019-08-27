using com.tvd12.ezyfoxserver.client.logger;

namespace com.tvd12.ezyfoxserver.client.util
{
	public class EzyLoggable
	{
		protected readonly EzyLogger logger;

		public EzyLoggable()
		{
			this.logger = EzyLoggerFactory.getLogger(GetType());
		}

		protected EzyLogger getLogger()
		{
			return logger;
		}
	}
}
