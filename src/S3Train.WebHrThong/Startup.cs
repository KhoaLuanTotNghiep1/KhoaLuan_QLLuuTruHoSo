using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(S3Train.WebHrThong.Startup))]
namespace S3Train.WebHrThong
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
