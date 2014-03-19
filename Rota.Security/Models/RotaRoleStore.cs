using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using Rota.Security.DataContext;

namespace Rota.Security.Models
{
    public class RotaRoleStore : RoleStore<RotaRole, long, RotaUserRole>
    {
        public RotaRoleStore()
            : this(new RotaDbContext())
        {
        }

        public RotaRoleStore(DbContext context)
            : base(context)
        {
        }
    }
}