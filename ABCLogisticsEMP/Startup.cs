using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ABCLogisticsEMP.Startup))]
namespace ABCLogisticsEMP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
