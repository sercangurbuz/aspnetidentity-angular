using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;
using Rota.Security.Helpers;
using Rota.Security.Models;
using Microsoft.Owin.Security;
using Rota.Security.Providers;
using Rota.Security.ViewModels;
using System.Security.Claims;
using Microsoft.Owin.Security.OAuth;

namespace Rota.Security.Web
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        public AccountController()
            : this(StartUp.UserManagerFactory(), StartUp.RoleManagerFactory(), StartUp.OAuthOptions.AccessTokenFormat)
        {
        }

        public AccountController(RotaUserManager userManager, RotaRoleManager roleManager, ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public RotaUserManager UserManager { get; private set; }
        public RotaRoleManager RoleManager { get; private set; }
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

        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("GetUserById")]
        public RotaUser GetUserById(long id)
        {
            return UserManager.FindById(id);
        }

        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("GetUsers")]
        public IEnumerable<RotaUser> GetUsers()
        {
            return UserManager.Users;
        }

        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("GetRoles")]
        public IEnumerable<RotaRole> GetRoles()
        {
            return RoleManager.Roles;
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            RotaUser user = new RotaUser
            {
                UserName = model.UserName
            };

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);
            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

    }
}