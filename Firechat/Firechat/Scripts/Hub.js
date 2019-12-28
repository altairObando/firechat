"use strict";
var contactos = null;
var proxy = $.connection.chatHub;
$.connection.hub.start().done(function () {
    proxy.server.getContactos().done(function (result) {
        contactos = result;
        $(".inbox_chat").html("");
        fillChatBox(contactos, true);
    });
});

function getConversacion(email) {
    proxy.server.getConversaciones(email).done(function (conversaciones) {
        
    });
}

/// tipo = true contacto, false = conversacion
function fillChatBox(data, tipo) {
    $(".inbox_chat").html("");
    var obj = "";
    obj += '<div class="chat_list">';
    obj += '<div class="chat_people">';
    obj += '<div class="chat_img"> <img src="@imgUrl" alt="avatar" class="rounded-circle"></div>';
    obj += '<div class="chat_ib">';
    obj += '<h5>@contacto <span class="chat_date">';
    obj += '<button type="button" class="btn btn-primary btn-sm" onclick=getConversacion("@contacto")>Chat <i class="fa fa-comment"></i></button>';
    obj += '</span></h5><p>@mensaje</p>';
    obj += '</div>';
    obj += '</div>';
    obj += '</div>';

    $.each(data, function (i, value) {
        if (tipo) {
            var fill = obj // Crear contacto
                .replace('@imgUrl', value.Imagen)
                .replace('@contacto', value.UserName)
                .replace('"@contacto"', '"'+value.Email+'"')
                .replace("@mensaje", value.Email);
            $(".inbox_chat").append(fill);
        } else {
            // Crear conversaciones
        }
    });
}