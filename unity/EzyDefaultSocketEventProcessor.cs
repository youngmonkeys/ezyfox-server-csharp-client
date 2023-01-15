using UnityEngine;

namespace com.tvd12.ezyfoxserver.client.unity
{
	public class EzyDefaultSocketEventProcessor : MonoBehaviour
	{
		private static EzyDefaultSocketEventProcessor _instance;

		private void Awake()
		{
			// If go back to current scene, don't make duplication
			if (_instance != null)
			{
				Destroy(gameObject);
			}
			else
			{
				_instance = this;
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
