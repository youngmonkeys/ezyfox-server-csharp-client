using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.binding;
using com.tvd12.ezyfoxserver.client.concurrent;

namespace com.tvd12.ezyfoxserver.client.support
{
    public class EzySocketProxyManager
    {
        private EzyBinding binding;
        private String defaultZoneName;
        private EzySocketProxy defaultSocketProxy;
        private readonly AtomicBoolean inited = new AtomicBoolean();
        private readonly IDictionary<String, EzySocketProxy> socketProxyByZoneName;

        private static readonly EzySocketProxyManager INSTANCE = new EzySocketProxyManager();

        private EzySocketProxyManager()
        {
            socketProxyByZoneName = new Dictionary<String, EzySocketProxy>();
        }

        public static EzySocketProxyManager getInstance()
        {
            return INSTANCE;
        }

        public void init()
        {
            init(new EzyBindingBuilder().build());
        }

        public void init(EzyBinding binding)
        {
            if (inited.compareAndSet(false, true))
            {
                this.binding = binding;
            }
        }

        public void setDefaultZoneName(String defaultZoneName)
        {
            this.defaultZoneName = defaultZoneName;
        }

        public EzySocketProxy getSocketProxy(String zoneName)
        {
            if (!inited.get())
            {
                throw new Exception("Must call init function ahead");
            }
            if (socketProxyByZoneName.ContainsKey(zoneName))
            {
                return socketProxyByZoneName[zoneName];
            }
            lock (this)
            {
                if (socketProxyByZoneName.ContainsKey(zoneName))
                {
                    return socketProxyByZoneName[zoneName];
                }
                else
                {
                    EzySocketProxy socketProxy = new EzySocketProxy(
                        zoneName,
                        binding
                    );
                    socketProxyByZoneName[zoneName] = socketProxy;
                    if (defaultSocketProxy == null)
                    {
                        defaultSocketProxy = socketProxy;
                    }
                    return socketProxy;
                }
            }
        }

        public EzySocketProxy getDefaultSocketProxy()
        {
            if (!inited.get())
            {
                throw new Exception("Must call init function ahead");
            }
            if (defaultZoneName == null)
            {
                throw new Exception("Must set default zone name first");
            }
            return defaultSocketProxy != null
                ? defaultSocketProxy
                : getSocketProxy(defaultZoneName);
        }

        public void removeSocketProxy(String zoneName)
        {
            if (!inited.get())
            {
                throw new Exception("Must call init function ahead");
            }
            socketProxyByZoneName.Remove(zoneName);
        }
    }
}
