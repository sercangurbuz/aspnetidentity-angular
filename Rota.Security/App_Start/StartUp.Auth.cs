using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Rota.Security.DataContext;
using Rota.Security.Models;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin;
using Rota.Security.Providers;

namespace Rota.Security
{
    public partial class StartUp
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }
        public static Func<RotaUserManager> UserManagerFactory { get; set; }

        static StartUp()
        {
            //
            UserManagerFactory = () => new RotaUserManager();
            //
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                //
                TokenEndpointPath = new PathString("/Token"),
                //
                Provider = new RotaOAuthProvider("self", UserManagerFactory),
                //
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                //Demo icinmiş ???
                AllowInsecureHttp = true
            };
        }

        public void ConfigureAuth(IAppBuilder app)
        {
            // Bearer tokens aktive et
            app.UseOAuthBearerTokens(OAuthOptions);
        }
    }
}