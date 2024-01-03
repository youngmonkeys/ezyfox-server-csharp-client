using UnityEngine;

namespace com.tvd12.ezyfoxserver.client.unity
{
	public abstract class EzyAbstractEventProcessor : MonoBehaviour
	{
		private static EzyAbstractEventProcessor INSTANCE;
		
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
#if UNITY_WEBGL && !UNITY_EDITOR
#else
			EzyClients.getInstance()
				.getClient(GetZoneName())
				.processEvents();
#endif
		}

		protected abstract string GetZoneName();
	}
}
