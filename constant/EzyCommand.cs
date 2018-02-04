namespace com.tvd12.ezyfoxserver.client.constant
{
	public sealed class EzyCommand
	{
		public const int ERROR = 10;
		public const int HANDSHAKE = 11;
		public const int PING = 12;
		public const int PONG = 13;
		public const int DISCONNECT = 14;
		public const int PLUGIN_REQUEST = 15;
		public const int LOGIN = 20;
		public const int LOGIN_ERROR = 21;
		public const int LOGOUT = 22;
		public const int APP_ACCESS = 30;
		public const int APPR_EQUEST = 31;
		public const int APP_JOINED = 32;
		public const int APPEXIT = 33;
		public const int APP_ACCESS_ERROR = 34;

		private EzyCommand()
		{
		}
	}
}
