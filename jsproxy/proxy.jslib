var EzyFoxServerClientPlugin = {
    $ezy: {
        client: null,
        setup: null,
        setupApp: null,
        handshakeHandler: null,
        accessAppHandler: null,
        zoneName: null,
        appName: null,
        handlersByCommand: {}
    },

    init: function (zoneName, appName) {
        ezy.zoneName = UTF8ToString(zoneName);
        ezy.appName = UTF8ToString(appName);

        ezy.handshakeHandler = new EzyHandshakeHandler();

        var userLoginHandler = new EzyLoginSuccessHandler();
        userLoginHandler.handleLoginSuccess = () => {
            var appAccessRequest = [ezy.appName, []];
            ezy.client.send(EzyCommand.APP_ACCESS, appAccessRequest);
        }

        ezy.accessAppHandler = new EzyAppAccessHandler();

        var disconnectionHandler = new EzyDisconnectionHandler();
        disconnectionHandler.preHandle = function (event) {
        }

        var config = new EzyClientConfig;
        config.zoneName = ezy.zoneName;
        var clients = EzyClients.getInstance();
        ezy.client = clients.newDefaultClient(config);
        ezy.setup = ezy.client.setup;
        ezy.setup.addEventHandler(EzyEventType.DISCONNECTION, disconnectionHandler);
        ezy.setup.addDataHandler(EzyCommand.HANDSHAKE, ezy.handshakeHandler);
        ezy.setup.addDataHandler(EzyCommand.LOGIN, userLoginHandler);
        ezy.setup.addDataHandler(EzyCommand.APP_ACCESS, ezy.accessAppHandler);
        ezy.setupApp = ezy.setup.setupApp(ezy.appName);
    },

    clientConnect: function (host, username, password, onAppAccessedCallbackPtr) {
        var usernameString = UTF8ToString(username);
        var passwordString = UTF8ToString(password);
        var hostString = UTF8ToString(host);
        ezy.handshakeHandler.getLoginRequest = function (context) {
            return [ezy.zoneName, usernameString, passwordString, []];
        }
        ezy.accessAppHandler.postHandle = (app, data) => {
            dynCall_v(onAppAccessedCallbackPtr);
        }
        ezy.client.connect("ws://" + hostString + ":2208/ws");
    },

    addCommand: function (command, callbackPtr) {
        var commandString = UTF8ToString(command);
        var lenCommand = lengthBytesUTF8(commandString) + 1;
        var ptrCommand = _malloc(lenCommand);
        stringToUTF8(commandString, ptrCommand, lenCommand);

        var handler = (app, data) => {
            var dataJson = JSON.stringify(data);
            var len = lengthBytesUTF8(dataJson) + 1;
            var ptr = _malloc(len);
            stringToUTF8(dataJson, ptr, len);
            dynCall_vii(callbackPtr, ptrCommand, ptr);
        }
        ezy.setupApp.addDataHandler(commandString, handler);
    },

    sendCommand: function (command) {
        var commandString = UTF8ToString(command);
        ezy.client.getApp().send(commandString, {});
    },

    sendCommandData: function (command, dataJson) {
        var commandString = UTF8ToString(command);
        var dataJsonString = UTF8ToString(dataJson);
        var data = JSON.parse(dataJsonString);
        ezy.client.getApp().send(commandString, data);
    },
};

autoAddDeps(EzyFoxServerClientPlugin, '$ezy');
mergeInto(LibraryManager.library, EzyFoxServerClientPlugin);
