using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(projectMoo.Startup))]
namespace projectMoo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
