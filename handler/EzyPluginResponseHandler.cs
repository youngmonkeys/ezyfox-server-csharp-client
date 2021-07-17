using System;
using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.handler
{
	public class EzyPluginResponseHandler : EzyAbstractDataHandler
	{
		public override void handle(EzyArray data)
		{
			int pluginId = data.get<int>(0);
			EzyArray commandData = data.get<EzyArray>(1);
			String cmd = commandData.get<String>(0);
			EzyData responseData = commandData.get<EzyData>(1, null);

			EzyPlugin plugin = client.getPluginById(pluginId);
            if (plugin == null)
            {
                logger.info("receive message when has not joined plugin yet");
                return;
            }
			EzyPluginDataHandler dataHandler = plugin.getDataHandler(cmd);
			if (dataHandler != null)
				dataHandler.handle(plugin, responseData);
			else
                logger.warn("plugin: " + plugin.getName() + " has no handler for command: " + cmd);
		}
	}
}
