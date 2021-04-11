# ezyfox-server-csharp-client <img src="https://github.com/youngmonkeys/ezyfox-server/blob/master/logo.png" width="48" height="48" />
csharp, unity client for ezyfox server

# Synopsis

csharp, unity client for ezyfox server

# Documentation
[https://youngmonkeys.org/ezyfox-csharp-client-sdk/](https://youngmonkeys.org/ezyfox-csharp-client-sdk/)

# Code Example

**1. Import**

```csharp
using com.tvd12.ezyfoxserver.client.config;
using com.tvd12.ezyfoxserver.client;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.factory;
using com.tvd12.ezyfoxserver.client.handler;
using com.tvd12.ezyfoxserver.client.evt;
using com.tvd12.ezyfoxserver.client.request;
```

**2. Create a TCP Client**

```csharp
var config = EzyClientConfig.builder()
    .clientName(ZONE_NAME)
    .build();
socketClient = new EzyUTClient(config);
EzyClients.getInstance().addClient(socketClient);
```

**3. Setup client**

```csharp
setup.addEventHandler(EzyEventType.CONNECTION_SUCCESS, new EzyConnectionSuccessHandler());
setup.addEventHandler(EzyEventType.CONNECTION_FAILURE, new EzyConnectionFailureHandler());
setup.addEventHandler(EzyEventType.DISCONNECTION, new DisconnectionHandler());
setup.addDataHandler(EzyCommand.HANDSHAKE, new HandshakeHandler());
setup.addDataHandler(EzyCommand.LOGIN, new LoginSuccessHandler());
setup.addDataHandler(EzyCommand.LOGIN_ERROR, new EzyLoginErrorHandler());
setup.addDataHandler(EzyCommand.APP_ACCESS, new AppAccessHandler());
setup.addDataHandler(EzyCommand.UDP_HANDSHAKE, new UdpHandshakeHandler());
```

**4. Setup app**

```csharp
var appSetup = setup.setupApp(APP_NAME);
appSetup.addDataHandler("reconnect", new ReconnectResponseHandler());
appSetup.addDataHandler("getGameId", new GetGameIdResponseHandler());
appSetup.addDataHandler("startGame", new StartGameResponseHandler());
```

**5. Connect to server**

```csharp
socketClient.connect("127.0.0.1", 3005);
```

**6. Handle socket's events on main thread**

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
    foreach (EzyClient client in cachedClients)
    {
        client.processEvents();
    }
}
```
# Used By
1. [space-shooter](https://youngmonkeys.org/asset/space-shooter/)