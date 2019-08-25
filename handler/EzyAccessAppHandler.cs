using System;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.manager;
using com.tvd12.ezyfoxserver.client.logger;

namespace com.tvd12.ezyfoxserver.client.handler
{
    public class EzyAccessAppHandler : EzyAbstractDataHandler
    {

        public override void handle(EzyArray data)
        {
            EzyZone zone = client.getZone();
            EzyAppManager appManager = zone.getAppManager();
            EzyApp app = newApp(zone, data);
            appManager.addApp(app);
            postHandle(app, data);
            logger.info("access app: " + app.getName() + " successfully");
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
