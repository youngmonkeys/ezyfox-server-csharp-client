namespace com.tvd12.ezyfoxserver.client.unity
{
	public partial class EzySingletonSocketManager
	{
		private static AbstractEzySocketManager INSTANCE;

		public static AbstractEzySocketManager getInstance()
		{
			if (INSTANCE == null)
			{
#if UNITY_WEBGL && !UNITY_EDITOR
				INSTANCE = new EzyWebGLSocketManager();
#else
				INSTANCE = new EzyDefaultSocketManager();
#endif
			}
			return INSTANCE;
		}
	}
}
