using System;
using com.tvd12.ezyfoxserver.client.constant;

namespace com.tvd12.ezyfoxserver.client.codec
{
	public class EzySimpleCodecFactory : EzyCodecFactory
	{
		private readonly EzyCodecCreator socketCodecCreator;

		public EzySimpleCodecFactory()
		{
			this.socketCodecCreator = newSocketCodecCreator();
		}

		public Object newEncoder(EzyConnectionType connectionType)
		{
			Object encoder = socketCodecCreator.newEncoder();
			return encoder;
		}

		public Object newDecoder(EzyConnectionType connectionType)
		{
			Object decoder = socketCodecCreator.newDecoder(Int32.MaxValue);
			return decoder;
		}

		private EzyCodecCreator newSocketCodecCreator()
		{
			EzyCodecCreator answer = new MsgPackCodecCreator();
			return answer;
		}

	}

}
