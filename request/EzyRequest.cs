using System;
using com.tvd12.ezyfoxserver.client.util;
using com.tvd12.ezyfoxserver.client.io;
using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.request
{
	public interface EzyParams : EzyArrayDataSerializable
	{
	}

	public interface EzyRequest : EzyArrayDataSerializable
	{
		int getCommand();

		EzyParams getParameters();
	}

	public abstract class EzyAbstractRequest : EzyEntityBuilders, EzyRequest
	{
		public EzyArray serialize()
		{
			var parray = getParameters().serialize();
			var array = newArrayBuilder()
				.append(getCommand())
				.append(parray)
				.build();
			return array;
		}

		public abstract int getCommand();
		public abstract EzyParams getParameters();
	}

	public class EzyEmptyParams : EzyEntityBuilders, EzyParams {

		public virtual EzyArray serialize()
		{
			return newArrayBuilder().build();
		}
	}

	public class EzyFixedRequest : EzyAbstractRequest
	{
		private readonly int command;
		private readonly EzyParams parameters;

		public EzyFixedRequest(int command, EzyParams parameters)
		{
			this.command = command;
			this.parameters = parameters;
		}

		public override int getCommand()
		{
			return command;
		}

		public override EzyParams getParameters()
		{
			return parameters;
		}
	}
}
