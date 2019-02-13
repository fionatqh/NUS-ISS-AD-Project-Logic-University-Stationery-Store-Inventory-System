using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SSISApplication.Startup))]
namespace SSISApplication
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
