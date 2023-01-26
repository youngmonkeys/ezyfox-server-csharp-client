using System;
using UnityEngine;

namespace com.tvd12.ezyfoxserver.client.unity
{
	[Serializable]
	public class EzyScriptableVariable<T> : ScriptableObject
	{
#if UNITY_EDITOR && !UNITY_WEBGL
		[Multiline]
		public string DeveloperDescription = "";
#endif

		[field: SerializeField] public T Value { get; set; }

		public void setValue(EzyScriptableVariable<T> variable)
		{
			Value = variable.Value;
		}
	}
}
