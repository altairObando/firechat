"use strict";

var proxy = $.connection.chatHub;
$.connection.hub.start().done(function () {
    proxy.server.getContactos().done(function (contactos) {
        $.each(contactos, function (index, value) {
            console.log("No. " + index + "  Usuario " + value.UserName);
        });
    });

});