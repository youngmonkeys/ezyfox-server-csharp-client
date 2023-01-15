using UnityEngine;

namespace com.tvd12.ezyfoxserver.client.unity
{
	public class GenericVariable<T> : ScriptableObject
	{
#if UNITY_EDITOR && !UNITY_WEBGL
		[Multiline]
		public string DeveloperDescription = "";
#endif

		[SerializeField]
		private T value;

		public T Value
		{
			get => value;
			set => this.value = value;
		}

		public void setValue(GenericVariable<T> variable)
		{
			Value = variable.value;
		}
	}
}
