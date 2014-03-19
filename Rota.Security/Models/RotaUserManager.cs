using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace Rota.Security.Models
{
    public class RotaUserManager : UserManager<RotaUser, long>
    {
        public RotaUserManager()
            : base(new RotaUserStore())
        {
        }

        public RotaUserManager(DbContext context)
            : base(new RotaUserStore(context))
        {
        }

    }
}