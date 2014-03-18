using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Rota.Security.Models
{
    public class RotaUserClaim : IdentityUserClaim<long>
    {
    }
}