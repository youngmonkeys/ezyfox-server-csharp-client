# ezyfox-server-csharp-client <img src="https://github.com/youngmonkeys/ezyfox-server/blob/master/logo.png" width="64" />
csharp, unity client for ezyfox server

# Synopsis

csharp, unity client for ezyfox server

# For Unity

Please move to the [unity-client branch](https://github.com/youngmonkeys/ezyfox-server-csharp-client/tree/unity-client).

# Documentation

[https://youngmonkeys.org/ezyfox-csharp-client-sdk/](https://youngmonkeys.org/ezyfox-csharp-client-sdk/)

# How to use?
* Since our project depends on `Newtonsoft Json` unity package, you first need to add it to your Unity project in either one of the following ways:
   - Select `Window` >> `Package Manager` >> :heavy_plus_sign: icon >> `Add package from git URL` >> Add `com.unity.nuget.newtonsoft-json` to URL field >> Add
   - Edit the `manifest.json` file in the `Packages` folder of your Unity project and add "com.unity.nuget.newtonsoft-json": "3.0.2"
* Clone/copy this repository into your unity project
* Your Unity project can now start using scripts from this repository

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

1. [hello-csharp](https://github.com/tvd12/ezyfox-server-example/tree/master/hello-csharp)
2. [space-shooter](https://youngmonkeys.org/asset/space-shooter/)
