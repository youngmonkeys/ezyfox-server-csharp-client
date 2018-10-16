namespace com.tvd12.ezyfoxserver.client.socket
{
    public class EzySocketDataEventLoopHandler : EzySocketEventLoopOneHandler
    {
        protected override string getThreadName()
        {
            return "data-event-handler";
        }
    }
}
