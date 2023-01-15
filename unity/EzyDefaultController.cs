using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.logger;
using com.tvd12.ezyfoxserver.client.support;
using UnityEngine;
using Object = System.Object;

namespace com.tvd12.ezyfoxserver.client.unity
{
	public class EzyDefaultController : MonoBehaviour
	{
		private readonly List<Tuple<String, Object>> handlers = new();
		protected EzyLogger logger;

		protected void Awake()
		{
			logger = EzySingletonSocketManager.getInstance()
				.getLogger(GetType());
		}

		protected void addHandler<T>(String cmd, EzyAppProxyDataHandler<T> handler)
		{
			handlers.Add(
				new Tuple<String, Object>(
					cmd,
					EzySingletonSocketManager.getInstance()
						.on(cmd, handler)
				)
			);
		}

		protected virtual void OnDestroy()
		{
			logger.debug("OnDestroy");
			foreach (Tuple<String, Object> tuple in handlers)
			{
				EzySingletonSocketManager.getInstance()
					.removeAppHandler(tuple.Item1, tuple.Item2);
			}
		}
	}
}
