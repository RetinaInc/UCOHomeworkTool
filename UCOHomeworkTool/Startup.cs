using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UCOHomeworkTool.Startup))]
namespace UCOHomeworkTool
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
