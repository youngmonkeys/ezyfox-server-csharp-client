using System;
using System.Threading;
using com.tvd12.ezyfoxserver.client.util;
using com.tvd12.ezyfoxserver.client.evt;
using com.tvd12.ezyfoxserver.client.request;
using static com.tvd12.ezyfoxserver.client.evt.EzyEventType;
using com.tvd12.ezyfoxserver.client.config;
using com.tvd12.ezyfoxserver.client.command;
using com.tvd12.ezyfoxserver.client.handler;
using com.tvd12.ezyfoxserver.client.constant;

namespace com.tvd12.ezyfoxserver.client
{

	class MainClass
	{
		public static void Main(string[] args)
		{
			EzyClientConfig clientConfig = EzyClientConfig
				.builder()
				.zoneName("freechat")
				.build();
			EzyClient client = EzyClients.getInstance().newDefaultClient(clientConfig);
			EzySetup setup = client.get<EzySetup>();
			setup.addEventHandler(EzyEventType.CONNECTION_SUCCESS, new EzyConnectionSuccessHandler());
			setup.addEventHandler(EzyEventType.CONNECTION_FAILURE, new EzyConnectionFailureHandler());
			setup.addDataHandler(EzyCommand.HANDSHAKE, new ExHandshakeEventHandler());
			client.connect("127.0.0.1", 3005);

			while (true)
			{
				Thread.Sleep(3);
				client.processEvents();
			}

		}
	}

	class ExHandshakeEventHandler : EzyHandshakeHandler
	{
		protected override EzyRequest getLoginRequest()
		{
			return new EzyLoginRequest("freechat", "dungtv", "123456");
		}
	}
}
