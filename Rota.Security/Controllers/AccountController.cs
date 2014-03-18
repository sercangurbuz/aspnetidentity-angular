using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Rota.Security.Helpers;
using Rota.Security.Models;
using Microsoft.Owin.Security;
using Rota.Security.ViewModels;
using System.Security.Claims;

namespace Rota.Security.Controllers
{
    [Authorize]
    public class AccountController : ApiController
    {
        public AccountController()
            : this(StartUp.UserManagerFactory(), StartUp.OAuthOptions.AccessTokenFormat)
        {
        }

        public AccountController(RotaUserManager userManager, ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public RotaUserManager UserManager { get; private set; }
        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return new UserInfoViewModel
            {
                UserName = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };
        }

    }
}