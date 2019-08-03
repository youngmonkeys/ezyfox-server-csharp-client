# ezyfox-server-csharp-client <img src="https://github.com/youngmonkeys/ezyfox-server/blob/master/logo.png" width="48" height="48" />
csharp, unity client for ezyfox server

# Synopsis

csharp, unity client for ezyfox server

# Code Example

**1. Import**

```csharp
using com.tvd12.ezyfoxserver.client.io;
using com.tvd12.ezyfoxserver.client.evt;
using com.tvd12.ezyfoxserver.client.request;
using com.tvd12.ezyfoxserver.client.config;
using com.tvd12.ezyfoxserver.client.command;
using com.tvd12.ezyfoxserver.client.handler;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.factory;
```



**2. Create a TCP Client**

```csharp
EzyClientConfig clientConfig = EzyClientConfig
    .builder()
    .clientName("first")
    .zoneName("example")
    .build();
EzyClients clients = EzyClients.getInstance();
EzyClient client = clients.newDefaultClient(clientConfig);
```

**3. Setup client**

```csharp
EzySetup setup = client.get<EzySetup>();
setup.addEventHandler(EzyEventType.CONNECTION_SUCCESS, new EzyConnectionSuccessHandler());
setup.addEventHandler(EzyEventType.CONNECTION_FAILURE, new EzyConnectionFailureHandler());
setup.addDataHandler(EzyCommand.HANDSHAKE, new ExHandshakeEventHandler());
setup.addDataHandler(EzyCommand.LOGIN, new ExLoginSuccessHandler());
setup.addDataHandler(EzyCommand.APP_ACCESS, new ExAccessAppHandler());
```

**4. Setup app**

```csharp
EzyAppSetup appSetup = setup.setupApp("hello-world");
appSetup.addDataHandler("broadcastMessage", new MessageResponseHandler());
```

**5. Connect to server**

```csharp
client.connect("localhost", 3005);
```

**3. Handle socket's events on main thread**

For one client:

```csharp
while (true)
{
	Thread.Sleep(3);
	client.processEvents();
}
```

For multiple clients:

```csharp
IList<EzyClient> cachedClients = new List<EzyClient>();
while(true) 
{
    Thread.Sleep(3);
    clients.getClients(cachedClients);
    foreach (EzyClient one in cachedClients)
        one.processEvents();
}
```