using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GilsbarDemo1.Startup))]
namespace GilsbarDemo1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
