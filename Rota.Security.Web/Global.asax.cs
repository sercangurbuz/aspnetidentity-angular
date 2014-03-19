using System.Data.Entity;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Rota.Security.DataContext;

namespace Rota.Security.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            //Database.SetInitializer<RotaDbContext>(new RotaDbInitializer());
        }
    }
}
