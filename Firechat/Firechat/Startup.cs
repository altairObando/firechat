using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Firechat.Startup))]
namespace Firechat
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
