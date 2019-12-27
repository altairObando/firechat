using Firechat.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System;

[assembly: OwinStartupAttribute(typeof(Firechat.Startup))]
namespace Firechat
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
            CrearUsuarios();
        }

        private void CrearUsuarios()
        {
            var db = new ApplicationDbContext();
            using (var t = db.Database.BeginTransaction())
            {
                try
                {
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                    for (int i = 1; i <= 10; i++)
                    {
                        string username = "Usuario" + i;
                        bool existe = userManager.FindByName(username) != null ? true : false;
                        if (!existe)
                        {
                            var u = new ApplicationUser();
                            u.UserName = username;
                            u.ImagenUrl = "";
                            u.Email = username + "@firechat.com";
                            string pass = "contra123*";
                            userManager.Create(u, pass);
                        }
                    }
                    db.SaveChanges();
                    t.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    t.Rollback();
                }
            }
        }
    }
}
