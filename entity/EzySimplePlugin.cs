using System;
using System.Text;
using com.tvd12.ezyfoxserver.client.builder;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.factory;
using com.tvd12.ezyfoxserver.client.handler;
using com.tvd12.ezyfoxserver.client.request;

namespace com.tvd12.ezyfoxserver.client.entity
{
	public class EzySimplePlugin : EzyEntity, EzyPlugin
	{
		protected readonly int id;
		protected readonly String name;
        protected readonly EzyZone zone;
        protected readonly EzyClient client;
		protected readonly EzyPluginDataHandlers dataHandlers;

		public EzySimplePlugin(EzyZone zone, int id, String name)
		{
			this.client = zone.getClient();
			this.zone = zone;
			this.id = id;
			this.name = name;
			this.dataHandlers = client.getHandlerManager().getPluginDataHandlers(name);
		}

		public void send(EzyRequest request)
		{
			String cmd = (String)request.getCommand();
			EzyData data = request.serialize();
			send(cmd, data);
		}

        public void send(String cmd)
        {
            send(cmd, EzyEntityFactory.EMPTY_OBJECT);
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
			client.send(EzyCommand.PLUGIN_REQUEST, requestData);
		}

        public void udpSend(EzyRequest request)
        {
            String cmd = (String)request.getCommand();
            EzyData data = request.serialize();
            udpSend(cmd, data);
        }

        public void udpSend(String cmd)
        {
            udpSend(cmd, EzyEntityFactory.EMPTY_OBJECT);
        }

        public void udpSend(String cmd, EzyData data)
        {
            EzyArrayBuilder commandData = EzyEntityFactory.newArrayBuilder()
                    .append(cmd)
                    .append(data);
            EzyArray requestData = EzyEntityFactory.newArrayBuilder()
                    .append(id)
                    .append(commandData.build())
                    .build();
            client.udpSend(EzyCommand.PLUGIN_REQUEST, requestData);
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

		public EzyPluginDataHandler getDataHandler(Object cmd)
		{
			EzyPluginDataHandler handler = dataHandlers.getHandler(cmd);
			return handler;
		}

        public override string ToString()
        {
            return new StringBuilder()
                .Append("Plugin(")
                .Append("id: ").Append(id).Append(", ")
                .Append("name: ").Append(name)
                .Append(")")
                .ToString();
        }
    }
}
