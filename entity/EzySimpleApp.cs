using System;
using System.Text;
using com.tvd12.ezyfoxserver.client.builder;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.factory;
using com.tvd12.ezyfoxserver.client.handler;
using com.tvd12.ezyfoxserver.client.request;

namespace com.tvd12.ezyfoxserver.client.entity
{
	public class EzySimpleApp : EzyEntity, EzyApp
	{
		protected readonly int id;
		protected readonly String name;
        protected readonly EzyZone zone;
        protected readonly EzyClient client;
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
			send(request, false);
		}


        public void send(EzyRequest request, bool encrypted)
		{
			String cmd = (String)request.getCommand();
			EzyData data = request.serialize();
			send(cmd, data, encrypted);
		}

        public void send(String cmd)
		{
			send(cmd, false);
		}

        public void send(String cmd, bool encrypted)
        {
            send(cmd, EzyEntityFactory.EMPTY_OBJECT, encrypted);
        }

        public void send(String cmd, EzyData data)
		{
			send(cmd, data, false);
		}


        public void send(String cmd, EzyData data, bool encrypted)
		{
			EzyArrayBuilder commandData = EzyEntityFactory.newArrayBuilder()
					.append(cmd)
					.append(data);
            EzyArray requestData = EzyEntityFactory.newArrayBuilder()
					.append(id)
                    .append(commandData.build())
					.build();
			client.send(EzyCommand.APP_REQUEST, requestData, encrypted);
		}

        public void udpSend(EzyRequest request)
		{
			udpSend(request, false);
		}

        public void udpSend(EzyRequest request, bool encryted)
        {
            String cmd = (String)request.getCommand();
            EzyData data = request.serialize();
            udpSend(cmd, data, encryted);
        }

        public void udpSend(String cmd)
		{
			udpSend(cmd, false);
		}

        public void udpSend(String cmd, bool encryted)
        {
            udpSend(cmd, EzyEntityFactory.EMPTY_OBJECT);
        }

        public void udpSend(String cmd, EzyData data)
		{
			udpSend(cmd, data, false);
		}

        public void udpSend(String cmd, EzyData data, bool encryted)
        {
            EzyArrayBuilder commandData = EzyEntityFactory.newArrayBuilder()
                    .append(cmd)
                    .append(data);
            EzyArray requestData = EzyEntityFactory.newArrayBuilder()
                    .append(id)
                    .append(commandData.build())
                    .build();
            client.udpSend(EzyCommand.APP_REQUEST, requestData, encryted);
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

        public override string ToString()
        {
            return new StringBuilder()
                .Append("App(")
                .Append("id: ").Append(id).Append(", ")
                .Append("name: ").Append(name)
                .Append(")")
                .ToString();
        }
    }
}
