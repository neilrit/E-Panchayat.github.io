using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EGram.Startup))]
namespace EGram
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
