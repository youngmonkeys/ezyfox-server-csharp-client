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
                            EzyLogger.console(this.clientName);
                            EzyLogger.console(this.ezyEventType);
                            EzyLogger.console(jsonData);
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
                            EzyLogger.console(this.clientName);
                            EzyLogger.console(this.cmd);
                            EzyLogger.console(jsonData);
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
                EzyLogger.console('reconnect: clientName = ' + clientName);
                ezy.client.reconnect();
            },
            'disconnect': function (clientName, jsonData, callback) {
                EzyLogger.console('disconnect: clientName = ' + clientName + ', jsonData = ' + jsonData);
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
            'startPing': function (clientName, callback) {
                EzyLogger.console('start ping: clientName = ' + clientName);
                ezy.client.pingSchedule.start();
            },
            'stopPing': function (clientName, callback) {
                EzyLogger.console('start ping: clientName = ' + clientName);
                ezy.client.pingSchedule.stop();
            },
            'openUrlWithCookies': function(clientName, jsonData, callback) {
                var data = JSON.parse(jsonData);
                document.cookie = data.cookies;
                var newTab = window.open(data.url, data.target || '_blank');
                if (!newTab) {
                    EzyLogger.console('Failed to open the new tab.');
                }
            }
        }
    },

    setEventHandlerCallback: function (callback) {
        ezy.eventHandlerCallback = callback;
    },
    
    setDataHandlerCallback: function (callback) {
        ezy.dataHandlerCallback = callback;
    },
    
    setDebug: function (value) {
        EzyLogger.debug = value;
    },

    isMobile: function () {
        var userAgent = window.navigator.userAgent.toLowerCase();
        var mobilePattern = /android|iphone|ipad|ipod/i;

        return userAgent.search(mobilePattern) !== -1
            || (userAgent.indexOf("macintosh") !== -1 && "ontouchend" in document);
    },

    run4: function (clientName, functionName, jsonData, callback) {
        var clientNameString = UTF8ToString(clientName);
        var functionNameString = UTF8ToString(functionName);
        var jsonDataString = UTF8ToString(jsonData);
        EzyLogger.console(
            'run4(clientName=' + clientNameString + ', functionName=' +
            functionNameString + ', jsonData=' + jsonDataString + ')'
        );
        ezy.map[functionNameString](clientNameString, jsonDataString, callback);
    },

    run3: function (clientName, functionName, callback) {
        var clientNameString = UTF8ToString(clientName);
        var functionNameString = UTF8ToString(functionName);
        EzyLogger.console('run3(clientName=' + clientNameString + ', functionName=' + functionNameString + ')');
        ezy.map[functionNameString](clientNameString, callback);
    }
};

autoAddDeps(EzyFoxServerClientPlugin, '$ezy');
mergeInto(LibraryManager.library, EzyFoxServerClientPlugin);
