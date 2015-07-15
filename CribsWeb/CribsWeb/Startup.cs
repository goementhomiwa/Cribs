using Microsoft.Owin;
using Owin;
 
[assembly: OwinStartupAttribute(typeof(Cribs.Web.Startup))]
namespace Cribs.Web
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
