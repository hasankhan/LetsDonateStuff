using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LetsDonateStuff.Startup))]
namespace LetsDonateStuff
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
