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

        public async Task<List<Contacto>> GetContactosAsync()
        {
            var data = await db.Users.OrderBy(x => x.UserName).ToListAsync();
            return data.Select(x => new Contacto { 
                UserName = x.UserName,
                Email = x.Email,
                Imagen = x.ImagenUrl
            }).ToList();
        }

        public async Task<Conversacion> GetConversacionesAsync(string usuario, string correoParticipante)
        {
            // Usuario 
            var user = await db.Users.FirstOrDefaultAsync(x => x.UserName == usuario);
            // Participante  
            var partic = await db.Users.FirstOrDefaultAsync(x => x.Email == correoParticipante);
            // Buscar la conversacion.
            var conversacion = await db.Conversaciones
                .Where(
                x =>
                // Si es el creador de la conversacion
                (x.ApplicationUserId == user.Id &&
                x.Participantes.Count(y => y.ApplicationUserId == partic.Id) > 0) ||
                // o si es participante
                (x.ApplicationUserId == user.Id &&
                x.Participantes.Count(y => y.ApplicationUserId == partic.Id) > 0)

                ).FirstOrDefaultAsync();
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
            return conversacion;
        }
    }
    
}