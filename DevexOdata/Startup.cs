using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DevexOdata.Startup))]
namespace DevexOdata
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
