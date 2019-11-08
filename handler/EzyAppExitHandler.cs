using System;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.manager;

namespace com.tvd12.ezyfoxserver.client.handler
{
    public class EzyAppExitHandler : EzyAbstractDataHandler
    {

        public override void handle(EzyArray data)
        {
            EzyZone zone = client.getZone();
            EzyAppManager appManager = zone.getAppManager();
            int appId = data.get<int>(0);
            int reasonId = data.get<int>(1);
            EzyApp app = appManager.removeApp(appId);
            if(app != null) {
                postHandle(app, data);
                logger.info("user exit app: " + app + ", reason: " + reasonId);
            }
        }

        protected virtual void postHandle(EzyApp app, EzyArray data)
        {
        }

        protected virtual EzyApp newApp(EzyZone zone, EzyArray data)
        {
            int appId = data.get<int>(0);
            String appName = data.get<String>(1);
            EzySimpleApp app = new EzySimpleApp(zone, appId, appName);
            return app;
        }
    }
}
