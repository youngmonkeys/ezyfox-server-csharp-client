var EzyFoxServerClientPlugin = {
    $ezy: {
        client: null,
        setup: null,
        setupApp: null,
        handshakeHandler: null,
        accessAppHandler: null,
        zoneName: null,
        appName: null,
        eventHandlerCallback: null,
        dataHandlerCallback: null,
        toUTF8: function (s) {
            var sLength = lengthBytesUTF8(s) + 1;
            var sPtr = _malloc(sLength);
            stringToUTF8(s, sPtr, sLength);
            return sPtr;
        },
        map: {
            'init': function (clientName, jsonConfig, callback) {
                var config = JSON.parse(jsonConfig);
                config.getClientName = function () {
                    return this.clientName;
                }
                var clients = EzyClients.getInstance();
                ezy.client = clients.newClient(config);
                ezy.setup = ezy.client.setup;

                for (var ezyEventType in EzyEventType) {
                    if (EzyEventType.hasOwnProperty(ezyEventType)) {
                        var eventHandler = {"ezyEventType": ezyEventType, "clientName": clientName};
                        eventHandler.handle = function (event) {
                            var jsonData = event ? JSON.stringify(event) : "{}";
                            console.log(this.clientName);
                            console.log(this.ezyEventType);
                            console.log(jsonData);
                            dynCall_viii(
                                ezy.eventHandlerCallback,
                                ezy.toUTF8(this.clientName),
                                ezy.toUTF8(this.ezyEventType),
                                ezy.toUTF8(jsonData)
                            );
                        }
                        ezy.setup.addEventHandler(ezyEventType, eventHandler);
                    }
                }

                for (var commandId in EzyCommands) {
                    if (EzyCommands.hasOwnProperty(commandId)) {
                        var dataHandler = {"cmd": EzyCommands[commandId], "clientName": clientName};
                        dataHandler.handle = function (data) {
                            var jsonData = data ? JSON.stringify(data) : "{}";
                            console.log(this.clientName);
                            console.log(this.cmd);
                            console.log(jsonData);
                            dynCall_viii(
                                ezy.dataHandlerCallback,
                                ezy.toUTF8(this.clientName),
                                this.cmd.id,
                                ezy.toUTF8(jsonData)
                            );
                        }
                        ezy.setup.addDataHandler(EzyCommands[commandId], dataHandler);
                    }
                }

                dynCall_vii(callback, ezy.toUTF8(clientName), ezy.toUTF8(jsonConfig));
            },
            'connect': function (clientName, jsonData, callback) {
                var data = JSON.parse(jsonData);
                ezy.client.connect(data.url);
            },
            'reconnect': function (clientName, callback) {
                console.log('reconnect: clientName = ' + clientName);
                ezy.client.reconnect();
            },
            'disconnect': function (clientName, jsonData, callback) {
                console.log('disconnect: clientName = ' + clientName + ', jsonData = ' + jsonData);
                var data = JSON.parse(jsonData);
                ezy.client.disconnect(data.reason);
            },
            'send': function (clientName, jsonData, callback) {
                var data = JSON.parse(jsonData);
                var cmd = EzyCommands[data.cmdId];
                var sendData = data.data;
                EzyClients.getInstance()
                    .getClient(clientName)
                    .send(cmd, sendData);
            },
        }
    },

    setEventHandlerCallback: function (callback) {
        ezy.eventHandlerCallback = callback;
    },
    
    setDataHandlerCallback: function (callback) {
        ezy.dataHandlerCallback = callback;
    },

    run4: function (clientName, functionName, jsonData, callback) {
        var clientNameString = UTF8ToString(clientName);
        var functionNameString = UTF8ToString(functionName);
        var jsonDataString = UTF8ToString(jsonData);
        console.log(
            'run4(clientName=' + clientNameString + ', functionName=' +
            functionNameString + ', jsonData=' + jsonDataString + ')'
        );
        ezy.map[functionNameString](clientNameString, jsonDataString, callback);
    },

    run3: function (clientName, functionName, callback) {
        var clientNameString = UTF8ToString(clientName);
        var functionNameString = UTF8ToString(functionName);
        console.log('run3(clientName=' + clientNameString + ', functionName=' + functionNameString + ')');
        ezy.map[functionNameString](clientNameString, callback);
    }
};

autoAddDeps(EzyFoxServerClientPlugin, '$ezy');
mergeInto(LibraryManager.library, EzyFoxServerClientPlugin);
