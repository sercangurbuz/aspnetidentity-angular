using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Rota.Security.Models
{
    public class RotaRoleStore : RoleStore<RotaRole, long, RotaUserRole>
    {
        public RotaRoleStore(DbContext context)
            : base(context)
        {

        }
    }
}