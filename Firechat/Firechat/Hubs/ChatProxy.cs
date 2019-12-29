using Firechat.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Firechat.Hubs
{
    public class ChatProxy
    {
        // Crear un singleton para manejar una sola conexion
        private readonly static Lazy<ChatProxy> _instancia = new Lazy<ChatProxy>(() => new ChatProxy(GlobalHost.ConnectionManager.GetHubContext<ChatHub>().Clients));
        // Objeto para bloquear transacciones
        private readonly object _update = new object();
        private readonly static ApplicationDbContext db = new ApplicationDbContext();
        private Microsoft.AspNet.SignalR.Hubs.IHubConnectionContext<dynamic> Clientes { get; set; }

        public ChatProxy(Microsoft.AspNet.SignalR.Hubs.IHubConnectionContext<dynamic> clients)
        {
            this.Clientes = clients;
        }
        public static ChatProxy Instancia
        {
            get
            {
                return _instancia.Value;
            }
        }

        public async Task<List<Contacto>> GetContactosAsync(string distinct)
        {
            var data = await db.Users.Where(d => d.UserName != distinct).OrderBy(x => x.UserName).ToListAsync();
            return data.Select(x => new Contacto { 
                UserName = x.UserName,
                Email = x.Email,
                Imagen = x.ImagenUrl
            }).ToList();
        }

        public async Task<Conversacion> GetConversacionesAsync(string usuario, string correoParticipante)
        {
            Conversacion conversacion = null;
            // Usuario 
            var user = await db.Users.FirstOrDefaultAsync(x => x.UserName == usuario);
            // Participante  
            var partic = await db.Users.FirstOrDefaultAsync(x => x.Email == correoParticipante);
            // Participaciones 
            var valor1 = await db.participaciones
                .Where(y => 
                (y.ApplicationUserId == partic.Id && y.Conversacion.ApplicationUserId == user.Id) ||
                (y.ApplicationUserId == user.Id && y.Conversacion.ApplicationUserId == partic.Id))

                .FirstOrDefaultAsync();
            if(valor1 != null)
            {
                conversacion = valor1.Conversacion;
            }
            // Si no hay conversaciones crear una nueva 
            if(conversacion == null)
            {
                using (var t = db.Database.BeginTransaction())
                {
                    try
                    {
                        // Crear la conversacion.
                        var c = new Conversacion
                        {
                            ApplicationUserId = user.Id,
                            FechaCreacion = DateTime.Now,
                            FechaActualizacion = DateTime.Now
                        };
                        db.Conversaciones.Add(c);
                        await db.SaveChangesAsync();
                        // Guardar los participantes que por ahora solo sera uno, mas adelante hago de varios
                        // Para que sean grupos de conversacion.
                        var p = new Participacion
                        {
                            ApplicationUserId = partic.Id,
                            TipoParticipacion = TipoParticipacion.Simple,
                            ConversacionId = c.Id
                        };
                        db.participaciones.Add(p);
                        await db.SaveChangesAsync();
                        t.Commit();
                        return c;
                    }
                    catch (Exception) 
                    {
                        return null;
                    }
                }
            }

            return new Conversacion
            {
                Id =  conversacion.Id,
                Mensajes = conversacion.Mensajes.Select(
                    x => new Mensaje {
                    ApplicationUser = new ApplicationUser { 
                        UserName = x.ApplicationUser.UserName,
                        ImagenUrl = x.ApplicationUser.ImagenUrl
                    },
                    Contenido = x.Contenido,
                    FechaEnvio = x.FechaEnvio
                }).ToList()
            };
        }

        public async Task EnviarMensaje(string usuario, int idConversacion, string mensaje)
        {
            // Buscar el id del enviador 
            var user = db.Users.FirstOrDefault(x => x.UserName == usuario);
            var destino = db.Conversaciones.Find(idConversacion);
            var msg = new Mensaje
            {
                FechaEnvio = DateTime.Now,
                ConversacionId = destino.Id,
                Contenido = mensaje,
                ApplicationUserId = user.Id
            };
            db.Mensajes.Add(msg);
            var data = await db.SaveChangesAsync() > 0;
            if(data)
            {

                var cliente = destino.Participantes[0].ApplicationUser.UserName;

                if(cliente != user.UserName)
                    Clientes.User(cliente).NotificarMensaje(mensaje, user.UserName);
                else
                    Clientes.User(user.UserName).NotificarMensaje(mensaje, cliente);
            }
        }
    }
    
}