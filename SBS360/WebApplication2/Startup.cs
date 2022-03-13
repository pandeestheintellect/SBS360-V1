using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Eng360Web.App_Start.Startup))]
namespace Eng360Web.App_Start
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
