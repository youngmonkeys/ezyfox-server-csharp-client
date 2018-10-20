using System;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.handler;

namespace com.tvd12.ezyfoxserver.client.handler
{
	public class EzyAppResponseHandler : EzyAbstractDataHandler
	{
		public override void handle(EzyArray data)
		{
			int appId = data.get<int>(0);
			EzyArray commandData = data.get<EzyArray>(1);
			String cmd = commandData.get<String>(0);
			EzyData responseData = commandData.get<EzyData>(1, null);

			EzyApp app = client.getAppById(appId);
			EzyAppDataHandler dataHandler = app.getDataHandler(cmd);
			if (dataHandler != null)
				dataHandler.handle(app, responseData);
			else
                logger.warn("app: " + app.getName() + " has no handler for command: " + cmd);
		}
	}
}
