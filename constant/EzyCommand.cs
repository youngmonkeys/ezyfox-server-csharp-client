namespace com.tvd12.ezyfoxserver.client.constant
{
	public enum EzyCommand
	{
        ERROR = 10,
		HANDSHAKE = 11,
		PING = 12,
		PONG = 13,
		DISCONNECT = 14,
		LOGIN = 20,
		LOGIN_ERROR = 21,
		APP_ACCESS = 30,
		APP_REQUEST = 31,
		APP_EXIT = 33,
		APP_ACCESS_ERROR = 34,
        APP_REQUEST_ERROR = 35,
		PLUGIN_INFO = 40,
		PLUGIN_REQUEST_BY_NAME = 41,
		PLUGIN_REQUEST_BY_ID = 42
	}
}
