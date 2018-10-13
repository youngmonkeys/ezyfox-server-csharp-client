using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.manager
{
	public class EzySimpleAppManager : EzyAppManager
	{
		protected readonly String zoneName;
		protected readonly IList<EzyApp> appList;
		protected readonly IDictionary<int, EzyApp> appsById;
		protected readonly IDictionary<String, EzyApp> appsByName;

		public EzySimpleAppManager(String zoneName)
		{
			this.zoneName = zoneName;
			this.appList = new List<EzyApp>();
			this.appsById = new Dictionary<int, EzyApp>();
			this.appsByName = new Dictionary<String, EzyApp>();
		}

		public void addApp(EzyApp app)
		{
			this.appList.Add(app);
			this.appsById[app.getId()] = app;
			this.appsByName[app.getName()] = app;
		}

		public EzyApp getApp()
		{
			if (appList.Count == 0)
				throw new ArgumentException("has no app in zone: " + zoneName);
			EzyApp app = appList[0];
			return app;
		}

		public IList<EzyApp> getAppList()
		{
			IList<EzyApp> list = new List<EzyApp>(appList);
			return list;
		}

		public EzyApp getAppById(int appId)
		{
			EzyApp app = null;
			if (appsById.ContainsKey(appId))
				app = appsById[appId];
			return app;
		}

		public EzyApp getAppByName(String appName)
		{
			EzyApp app = null;
			if (appsByName.ContainsKey(appName))
				app = appsByName[appName];
			return app;
		}

		public void clear()
		{
			appList.Clear();
			appsById.Clear();
			appsByName.Clear();
		}
	}
}
