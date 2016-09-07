using System.Threading;
using System.Web.Http;
using Autofac.Integration.WebApi;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof(ContractManagement.WebApi.App_Start.Startup))]
namespace ContractManagement.WebApi.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            AutoMapperManager.Configure();

            HttpConfiguration config = new HttpConfiguration();

            AutofacConfig.Register(config);

            WebApiConfig.Register(config);


            app.UseAutofacMiddleware(config.DependencyResolver.GetRootLifetimeScope());
            app.UseAutofacWebApi(config);
            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(config);




            //handle OWIN shutdown
            string tokenKey = "host.OnAppDisposing";
            if (app.Properties.ContainsKey(tokenKey))
            {
                var context = new OwinContext(app.Properties);
                var token = context.Get<CancellationToken>(tokenKey);
                if (token != CancellationToken.None)
                {
                    token.Register(() =>
                    {
                        config.DependencyResolver.Dispose();
                    });
                }
            }

        }
    }
}