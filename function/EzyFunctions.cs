using System;

namespace com.tvd12.ezyfoxserver.client.function
{
	public delegate O EzyTransform<I,O>(I input);

	public delegate Object EzyToObject<I>(I input);

	public delegate O EzyParser<I,O>(I input);
}