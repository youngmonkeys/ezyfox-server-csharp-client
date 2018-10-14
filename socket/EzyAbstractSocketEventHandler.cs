namespace com.tvd12.ezyfoxserver.client.socket
{
	public abstract class EzyAbstractSocketEventHandler : EzySocketEventHandler
	{
		public abstract void handleEvent();

		public virtual void reset()
		{
		}

		public virtual void destroy()
		{
		}
	}
}
