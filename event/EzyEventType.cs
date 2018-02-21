namespace com.tvd12.ezyfoxserver.client.evt
{
	public sealed class EzyEventType
	{
		public const int CONNECTION_SUCCESS = 0;
		public const int CONNECTION_FAILURE = 1;
		public const int HANDSHAKE = 2;
		public const int LOGIN_SUCCESS = 3;
		public const int ACCESS_APP_SUCCESS = 4;

		private EzyEventType()
		{
		}
	}
}
