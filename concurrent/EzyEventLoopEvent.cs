namespace com.tvd12.ezyfoxserver.client.concurrent
{
    public interface EzyEventLoopEvent
    {
        bool call();

        void onFinished() { }

        void onRemoved() { }
    }
}
