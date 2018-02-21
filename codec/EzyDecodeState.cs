namespace com.tvd12.ezyfoxserver.client.codec
{
	public sealed class EzyDecodeState
	{
		public const int PREPARE_MESSAGE = 0;
		public const int READ_MESSAGE_HEADER = 1;
		public const int READ_MESSAGE_SIZE = 2;
		public const int READ_MESSAGE_CONTENT = 3;

		private EzyDecodeState()
		{
		}
	}
}
