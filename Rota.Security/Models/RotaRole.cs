using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Rota.Security.Models
{
    public class RotaRole : IdentityRole<long, RotaUserRole>
    {
        public RotaRole()
        {
        }

        public RotaRole(string roleName)
        {
            this.Name = roleName;
        }
    }
}