"use strict";
var contactos = null;
var proxy = $.connection.chatHub;
$.connection.hub.start().done(function () {
    proxy.server.getContactos().done(function (result) {
        contactos = result;
    });

});