using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Firechat.Models;
using Microsoft.AspNet.SignalR;

namespace Firechat.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ChatProxy _chatProxy;

        public ChatHub(ChatProxy chatProxy)
        {
            _chatProxy = chatProxy;
        }
        public ChatHub() : this(ChatProxy.Instancia) { }
        public async Task<List<Contacto>> GetContactos ()
        {
            return await _chatProxy.GetContactosAsync();
        }
    }


    public class UserProvider : IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            using (var db = new ApplicationDbContext())
            {
                string user = request.User.Identity.Name;
                return db.Users.Where(x => x.UserName == user).FirstOrDefault().Id;
            }
        }
    }
}