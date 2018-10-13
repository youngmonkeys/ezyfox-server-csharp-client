using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.io;
using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.wrapper
{
	public class EzyAppManager
	{
		protected IList<EzyApp> appList = new List<EzyApp>();
		protected IDictionary<int, EzyApp> appsById = new Dictionary<int, EzyApp>();
		protected IDictionary<String, EzyApp> appsByName = new Dictionary<String, EzyApp>();

		public void addApp(EzyApp app)
		{
			appList.Add(app);
			appsById.Add(app.getId(), app);
			appsByName.Add(app.getName(), app);
		}

		public void removeApp(EzyApp app)
		{
			appList.Remove(app);
			appsById.Remove(app.getId());
			appsByName.Remove(app.getName());
		}

		public void removeApp(int appId)
		{
			var app = getApp(appId);
			if (app != null)
			{
				removeApp(app);
			}
		}

		public void removeApp(String appName)
		{
			var app = getApp(appName);
			if (app != null)
			{
				removeApp(app);
			}
		}

		public EzyApp getApp(int appId)
		{
			return appsById[appId];
		}

		public EzyApp getApp(String appName)
		{
			return appsByName[appName];
		}

		public IList<EzyApp> getAppList()
		{
			return EzyLists.clone(appList);
		}

		public ISet<int> getAppIds()
		{
			return EzyMaps.keySet(appsById);
		}

		public ISet<String> getAppNames()
		{
			return EzyMaps.keySet(appsByName);
		}

	}
}
