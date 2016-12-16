using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(facematchingdemo.Startup))]
namespace facematchingdemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
