using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.handler
{
    public class EzyLoginErrorHandler : EzyAbstractDataHandler
    {
        public override void handle(EzyArray data)
        {
            client.disconnect(401);
            handleLoginError(data);
        }

        protected virtual void handleLoginError(EzyArray data)
        {
        }
    }
}
