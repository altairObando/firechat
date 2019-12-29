"use strict";
var contactos = null;
var conversacionId = null;
const monthNames = ["January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"
];
var proxy = $.connection.chatHub;
proxy.client.notificarMensaje = function (mensaje, usuario) {
    $("#" + usuario).find("p").html(mensaje);
    
}
$.connection.hub.start().done(function () {
    proxy.server.getContactos().done(function (result) {
        contactos = result;
        $(".inbox_chat").html("");
        fillChatBox(contactos, true);
    });
});

function EnviarMensaje() {
    var mensaje = $(".write_msg").val()
    if (mensaje.trim()) {
        proxy.server.enviarMensaje(conversacionId, mensaje);
    }
    // Agregar el mensaje enviado al contenedor de mensajes
    var sendmsg = "";
    sendmsg += '<div class="outgoing_msg">';
    sendmsg += '<div class="sent_msg">';
    sendmsg += '<p>'+mensaje+'</p>';
    sendmsg += '<span class="time_date"> ' + formatAMPM(new Date()) + '    |    ' + monthNames[new Date().getMonth()]+' ' + new Date().getDate()+' </span>';
    sendmsg += '</div>';
    sendmsg += '</div>';
    $(".msg_history").append(sendmsg);
    $(".write_msg").val('')
    var d = $('.msg_history');
    d.scrollTop(d.prop("scrollHeight"));

}
// Convertir la fecha local a formato horario
function formatAMPM(date) {
    var hours = date.getHours();
    var minutes = date.getMinutes();
    var ampm = hours >= 12 ? 'pm' : 'am';
    hours = hours % 12;
    hours = hours ? hours : 12; // the hour '0' should be '12'
    minutes = minutes < 10 ? '0' + minutes : minutes;
    var strTime = hours + ':' + minutes + ' ' + ampm;
    return strTime;
}

function getConversacion(email) {
    $(".msg_history").html('');
    $(".chat_list").removeClass('active_chat');
    //var user = email.split('@')[0];
    $("#" + email.split('@')[0]).addClass('active_chat');
    proxy.server.getConversaciones(email).done(function (convr) {
        conversacionId = convr.Id;
        // Ordenar las conversaciones.
        $.each(convr.Mensajes, function (i, value) {
            var fecha = formatAMPM(new Date(value.FechaEnvio));
            if (value.ApplicationUser.UserName === user) { //Mensajes salientes
                var msg = "";
                msg += '<div class="outgoing_msg">';
                msg += '<div class="sent_msg">';
                msg += '<p>' + value.Contenido + '</p>';
                msg += '<span class="time_date"> ' + fecha + '  |    ' + monthNames[new Date().getMonth()] + ' ' + new Date().getDate() + '</span>';
                msg += '</div>';
                msg += '</div>';

                $(".msg_history").append(msg);
            } else {
                var msg = "";
                msg += '<div class="incoming_msg">';
                msg += '<div class="incoming_msg_img">';
                msg += '<img src="' + value.ApplicationUser.ImagenUrl + '" alt="avatar-' + value.ApplicationUser.UserName + '"> </div>'
                msg += '<div class="received_msg">';
                msg += '<div class="received_withd_msg">';
                msg += '<p>' + value.Contenido + '</p>';
                msg += '<span class="time_date"> ' + fecha + '  |    ' + monthNames[new Date().getMonth()] + ' ' + new Date().getDate() + '</span>';
                msg += '</div>';
                msg += '</div>';
                msg += '</div>';
                $(".msg_history").append(msg);
            }
        });
        var d = $('.msg_history');
        d.scrollTop(d.prop("scrollHeight"));



    });
}

/// tipo = true contacto, false = conversacion
function fillChatBox(data, tipo) {
    $(".inbox_chat").html("");
    $.each(data, function (i, value) {
        if (tipo) {
            var obj = "";
            obj += '<div class="chat_list" id="'+value.UserName+'">';
            obj += '<div class="chat_people">';
            obj += '<div class="chat_img"> <img src="'+value.Imagen+'" alt="avatar" class="rounded-circle"></div>';
            obj += '<div class="chat_ib">';
            obj += '<h5>' + value.Email +' <span class="chat_date">';
            obj += '<button type="button" class="btn btn-primary btn-sm" onclick=getConversacion("'+value.Email+'")>Chat <i class="fa fa-comment"></i></button>';
            obj += '</span></h5><p></p>';
            obj += '</div>';
            obj += '</div>';
            obj += '</div>';
            $(".inbox_chat").append(obj);
        } else {
            // Crear conversaciones
        }
    });
}