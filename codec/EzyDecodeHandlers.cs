using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.io;
using com.tvd12.ezyfoxserver.client.util;

namespace com.tvd12.ezyfoxserver.client.codec
{
	public abstract class EzyDecodeHandlers : EzyResettable
	{
		protected int state;
		protected IDictionary<int, EzyDecodeHandler> handlers;

		protected EzyDecodeHandlers(AbstractBuilder builder)
		{
			this.state = firstState();
			this.handlers = builder.newHandlers();
		}

		public void handle(EzyByteBuffer input, Queue<EzyMessage> output)
		{
			EzyDecodeHandler handler = handlers[state];
			while (handler != null && handler.handle(input, output))
			{
				state = handler.nextState();
				handler = handler.nextHandler();
			}
		}

		protected int firstState()
		{
			return EzyDecodeState.PREPARE_MESSAGE;
		}

		public void reset()
		{
			this.state = firstState();
		}

		public abstract class AbstractBuilder
		{
			public IDictionary<int, EzyDecodeHandler> newHandlers()
			{
				var answer = new Dictionary<int, EzyDecodeHandler>();
				addHandlers(answer);
				return answer;
			}

			protected abstract void addHandlers(IDictionary<int, EzyDecodeHandler> answer);
		}
	}

}
