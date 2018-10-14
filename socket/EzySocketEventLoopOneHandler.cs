namespace com.tvd12.ezyfoxserver.client.socket
{
	public abstract class EzySocketEventLoopOneHandler : EzySocketEventLoopHandler
	{
		protected EzySocketEventHandler eventHandler;

		public void setEventHandler(EzySocketEventHandler eventHandler)
		{
			this.eventHandler = eventHandler;
		}

		protected override EzySimpleSocketEventLoop newEventLoop()
		{
			EzySimpleSocketEventLoop eventLoop = new EzySimpleSocketEventLoop();
			eventLoop.setAction(() => eventHandler.handleEvent());
			return eventLoop;
		}

		public override void destroy()
		{
			base.destroy();
			if (eventHandler != null)
				eventHandler.destroy();
			this.eventHandler = null;
		}
	}
}
