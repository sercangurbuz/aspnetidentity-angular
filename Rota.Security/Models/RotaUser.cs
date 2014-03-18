using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Rota.Security.Models
{
    public class RotaUser : IdentityUser<long, RotaUserLogin, RotaUserRole, RotaUserClaim>
    {
       
    }
}