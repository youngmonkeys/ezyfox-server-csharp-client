using System;

namespace com.tvd12.ezyfoxserver.client.unity
{
	[Serializable]
	public class GenericReference<T>
	{
		public bool useConstant = true;
		public T constantValue;
		public GenericVariable<T> variable;

		public GenericReference() { }

		public GenericReference(T value)
		{
			useConstant = true;
			constantValue = value;
		}

		public T Value => useConstant ? constantValue : variable.Value;

		public static implicit operator T(GenericReference<T> reference)
		{
			return reference.Value;
		}
	}
}
