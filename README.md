# ezyfox-server-csharp-client <img src="https://github.com/youngmonkeys/ezyfox-server/blob/master/logo.png" width="48" height="48" />
csharp, unity client for ezyfox server

# Synopsis

csharp, unity client for ezyfox server

# Code Example

**1. Create a TCP Client**

```csharp
EzyClientConfig clientConfig = EzyClientConfig
				.builder()
				.zoneName("freechat")
				.build();
EzyClient client = EzyClients.getInstance().newDefaultClient(clientConfig);
EzySetup setup = client.get<EzySetup>();
setup.addEventHandler(EzyEventType.CONNECTION_SUCCESS, new EzyConnectionSuccessHandler());
setup.addEventHandler(EzyEventType.CONNECTION_FAILURE, new EzyConnectionFailureHandler());
setup.addDataHandler(EzyCommand.HANDSHAKE, new ExHandshakeEventHandler());
```

**2. Connect to server**

```csharp
client.connect("31.12.1.2", 3005);
```csharp

**3. Handle socket's events on main thread**

```csharp
while (true)
{
	Thread.Sleep(3);
	client.processEvents();
}
```