using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.manager
{
	public class EzySimplePluginManager : EzyPluginManager
	{
		protected readonly String zoneName;
		protected readonly IList<EzyPlugin> pluginList;
		protected readonly IDictionary<int, EzyPlugin> pluginsById;
		protected readonly IDictionary<String, EzyPlugin> pluginsByName;

		public EzySimplePluginManager(String zoneName)
		{
			this.zoneName = zoneName;
			this.pluginList = new List<EzyPlugin>();
			this.pluginsById = new Dictionary<int, EzyPlugin>();
			this.pluginsByName = new Dictionary<String, EzyPlugin>();
		}

		public void addPlugin(EzyPlugin plugin)
		{
			this.pluginList.Add(plugin);
			this.pluginsById[plugin.getId()] = plugin;
			this.pluginsByName[plugin.getName()] = plugin;
		}

        public EzyPlugin removePlugin(int pluginId) {
            EzyPlugin plugin = null;
            if(pluginsById.ContainsKey(pluginId)) {
                plugin = pluginsById[pluginId];
                pluginsById.Remove(pluginId);
                pluginsByName.Remove(plugin.getName());
                pluginList.Remove(plugin);
            }
            return plugin;
        }

		public EzyPlugin getPlugin()
		{
			if (pluginList.Count == 0)
				throw new ArgumentException("has no plugin in zone: " + zoneName);
			EzyPlugin plugin = pluginList[0];
			return plugin;
		}

		public IList<EzyPlugin> getPluginList()
		{
			IList<EzyPlugin> list = new List<EzyPlugin>(pluginList);
			return list;
		}

		public EzyPlugin getPluginById(int pluginId)
		{
			EzyPlugin plugin = null;
			if (pluginsById.ContainsKey(pluginId))
				plugin = pluginsById[pluginId];
			return plugin;
		}

		public EzyPlugin getPluginByName(String pluginName)
		{
			EzyPlugin plugin = null;
			if (pluginsByName.ContainsKey(pluginName))
				plugin = pluginsByName[pluginName];
			return plugin;
		}

		public void clear()
		{
			pluginList.Clear();
			pluginsById.Clear();
			pluginsByName.Clear();
		}
	}
}
