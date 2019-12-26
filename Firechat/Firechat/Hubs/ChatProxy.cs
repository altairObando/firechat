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
            var data = await db.Users.ToListAsync();
            return data.Select(x => new Contacto { UserName = x.UserName }).ToList();
        }
    }
    
}