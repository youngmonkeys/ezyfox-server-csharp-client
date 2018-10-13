using System;
using System.Threading;
using com.tvd12.ezyfoxserver.client.util;
using com.tvd12.ezyfoxserver.client.evt;
using com.tvd12.ezyfoxserver.client.request;
using static com.tvd12.ezyfoxserver.client.evt.EzyEventType;

namespace com.tvd12.ezyfoxserver.client
{

	class MainClass
	{
		public static void Main(string[] args)
		{
			EzyClient client = new EzyClient();
			client.addEventHandler(CONNECTION_SUCCESS, new EzyConnectionSuccessEventHandler());
			client.addEventHandler(HANDSHAKE, new ExHandshakeEventHandler());
			client.addEventHandler(LOGIN_SUCCESS, new ExLoginSuccessEventHandler());
			client.connect("localhost", 3005);
			while (true)
			{
				Thread.Sleep(3);
				client.processEvents();
			}

		}
	}

	class ExLoginSuccessEventHandler : EzyLoginSuccessEventHandler
	{
		
	}

	class ExHandshakeEventHandler : EzyHandshakeEventHandler
	{
		protected override request.EzyLoginRequestParams defaultLoginRequestParams()
		{
			var parameters = base.defaultLoginRequestParams();
			parameters.setZoneName("freechat");
			parameters.setUsername("dungtv");
			parameters.setPassword("123456");
			parameters.setData(EzyEntityArrays.newArray());
			return parameters;
		}	
	}
}
