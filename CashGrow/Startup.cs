using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CashGrow.Startup))]
namespace CashGrow
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
