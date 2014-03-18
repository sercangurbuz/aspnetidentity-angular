using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using Rota.Security.DataContext;

namespace Rota.Security.Models
{
    public class RotaUserStore : UserStore<RotaUser, RotaRole, long, RotaUserLogin, RotaUserRole, RotaUserClaim>
    {
        public RotaUserStore()
            : this(new RotaDbContext())
        {
        }

        public RotaUserStore(DbContext context)
            : base(context)
        {
        }
    }
}