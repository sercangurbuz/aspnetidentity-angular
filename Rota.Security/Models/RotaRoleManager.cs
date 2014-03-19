using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Rota.Security.DataContext;

namespace Rota.Security.Models
{
    public class RotaRoleManager : RoleManager<RotaRole, long>
    {
        public RotaRoleManager()
            : this(new RotaDbContext())
        {
        }

        public RotaRoleManager(DbContext context)
            : base(new RotaRoleStore(context))
        {
        }
    }
}