using System;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.manager;

namespace com.tvd12.ezyfoxserver.client.handler
{
    public class EzyPluginInfoHandler : EzyAbstractDataHandler
    {

        public override void handle(EzyArray data)
        {
            EzyZone zone = client.getZone();
            EzyPluginManager pluginManager = zone.getPluginManager();
            EzyPlugin plugin = newPlugin(zone, data);
            pluginManager.addPlugin(plugin);
            postHandle(plugin, data);
            logger.info("access plugin: " + plugin.getName() + " successfully");
        }

        protected virtual void postHandle(EzyPlugin plugin, EzyArray data)
        {
        }

        protected virtual EzyPlugin newPlugin(EzyZone zone, EzyArray data)
        {
            int pluginId = data.get<int>(0);
            String pluginName = data.get<String>(1);
            EzySimplePlugin plugin = new EzySimplePlugin(zone, pluginId, pluginName);
            return plugin;
        }
    }
}
