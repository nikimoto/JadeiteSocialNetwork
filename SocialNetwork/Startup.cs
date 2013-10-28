using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SocialNetwork.Startup))]
namespace SocialNetwork
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
