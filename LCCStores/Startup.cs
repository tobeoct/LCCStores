using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LCCStores.Startup))]
namespace LCCStores
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
