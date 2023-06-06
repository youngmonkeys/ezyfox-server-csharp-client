using System;
using UnityEngine;

namespace com.tvd12.ezyfoxserver.client.unity
{
	[Serializable]
	public class EzyScriptableVariable<T> : ScriptableObject
	{
#if UNITY_EDITOR && !UNITY_WEBGL
		[Multiline]
		[field: SerializeField]
		private string developerDescription = "";
#endif

		[field: SerializeField]
		public T Value { get; set; }

		private void OnEnable()
		{
			hideFlags = HideFlags.DontUnloadUnusedAsset;
		}

		public void SetValue(EzyScriptableVariable<T> variable)
		{
			Value = variable.Value;
		}
	}
}
