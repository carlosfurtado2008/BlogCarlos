using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BlogCarlos.Startup))]
namespace BlogCarlos
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
