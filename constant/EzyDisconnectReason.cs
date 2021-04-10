using System;
using System.Collections.Generic;

namespace com.tvd12.ezyfoxserver.client.constant
{
	public enum EzyDisconnectReason
	{
        CLOSE = -1,
        UNKNOWN = 0,
		IDLE = 1,
		NOT_LOGGED_IN = 2,
		ANOTHER_SESSION_LOGIN = 3,
		ADMIN_BAN = 4,
		ADMIN_KICK = 5,
		MAX_REQUEST_PER_SECOND = 6,
		MAX_REQUEST_SIZE = 7,
		SERVER_ERROR = 8,
		SERVER_NOT_RESPONDING = 400,
        UNAUTHORIZED = 401
    }

    public sealed class EzyDisconnectReasons
    {
        private static readonly IDictionary<int, String> REASON_NAMES = newReasonNames();

        private EzyDisconnectReasons() 
        {
        }

        public static String getDisconnectReasonName(int reason)
        {
            if (REASON_NAMES.ContainsKey(reason))
                return REASON_NAMES[reason];
            return reason.ToString();
        }

        private static IDictionary<int, String> newReasonNames()
        {
            IDictionary<int, String> dict = new Dictionary<int, String>();
            dict[(int)EzyDisconnectReason.CLOSE] = "CLOSE";
            dict[(int)EzyDisconnectReason.UNKNOWN] = "UNKNOWN";
            dict[(int)EzyDisconnectReason.IDLE] = "IDLE";
            dict[(int)EzyDisconnectReason.NOT_LOGGED_IN] = "NOT_LOGGED_IN";
            dict[(int)EzyDisconnectReason.ANOTHER_SESSION_LOGIN] = "ANOTHER_SESSION_LOGIN";
            dict[(int)EzyDisconnectReason.ADMIN_BAN] = "ADMIN_BAN";
            dict[(int)EzyDisconnectReason.ADMIN_KICK] = "ADMIN_KICK";
            dict[(int)EzyDisconnectReason.MAX_REQUEST_PER_SECOND] = "MAX_REQUEST_PER_SECOND";
            dict[(int)EzyDisconnectReason.MAX_REQUEST_SIZE] = "MAX_REQUEST_SIZE";
            dict[(int)EzyDisconnectReason.SERVER_ERROR] = "SERVER_ERROR";
            dict[(int)EzyDisconnectReason.SERVER_NOT_RESPONDING] = "SERVER_NOT_RESPONDING";
            dict[(int)EzyDisconnectReason.UNAUTHORIZED] = "UNAUTHORIZED";
            return dict;
        }
    }
}
