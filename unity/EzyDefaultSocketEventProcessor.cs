using UnityEngine;

namespace com.tvd12.ezyfoxserver.client.unity
{
	public class EzyDefaultSocketEventProcessor : MonoBehaviour
	{
		private static EzyDefaultSocketEventProcessor INSTANCE;

		private void Awake()
		{
			// If go back to current scene, don't make duplication
			if (INSTANCE != null)
			{
				Destroy(gameObject);
			}
			else
			{
				INSTANCE = this;
				DontDestroyOnLoad(gameObject);
			}
		}

		void Update()
		{
			// Main thread pulls data from socket
			EzySingletonSocketManager.getInstance()
				.processEvents();
		}
	}
}
