using System;

namespace com.tvd12.ezyfoxserver.client.security
{
	public interface EzyClientIdFetcher
	{
		String getClientId();
	}

	public class EzyUnknownClientIdFetcher : EzyClientIdFetcher
	{
		public String getClientId()
		{
			return "unknown";
		}
	}
}
