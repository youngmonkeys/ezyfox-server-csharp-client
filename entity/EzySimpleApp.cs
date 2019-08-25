using System;
using com.tvd12.ezyfoxserver.client.builder;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.factory;
using com.tvd12.ezyfoxserver.client.handler;
using com.tvd12.ezyfoxserver.client.request;

namespace com.tvd12.ezyfoxserver.client.entity
{
	public class EzySimpleApp : EzyEntity, EzyApp
	{
		protected readonly EzyClient client;
		protected readonly EzyZone zone;
		protected readonly int id;
		protected readonly String name;
		protected readonly EzyAppDataHandlers dataHandlers;

		public EzySimpleApp(EzyZone zone, int id, String name)
		{
			this.client = zone.getClient();
			this.zone = zone;
			this.id = id;
			this.name = name;
			this.dataHandlers = client.getHandlerManager().getAppDataHandlers(name);
		}

		public void send(EzyRequest request)
		{
			String cmd = (String)request.getCommand();
			EzyData data = request.serialize();
			send(cmd, data);
		}

		public void send(String cmd, EzyData data)
		{
			EzyArrayBuilder commandData = EzyEntityFactory.newArrayBuilder()
					.append(cmd)
					.append(data);
            EzyArray requestData = EzyEntityFactory.newArrayBuilder()
					.append(id)
                    .append(commandData.build())
					.build();
			client.send(EzyCommand.APP_REQUEST, requestData);
		}

		public int getId()
		{
			return id;
		}

		public String getName()
		{
			return name;
		}

		public EzyClient getClient()
		{
			return client;
		}

		public EzyZone getZone()
		{
			return zone;
		}

		public EzyAppDataHandler getDataHandler(Object cmd)
		{
			EzyAppDataHandler handler = dataHandlers.getHandler(cmd);
			return handler;
		}
	}
}
