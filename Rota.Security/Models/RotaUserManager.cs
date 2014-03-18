using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace Rota.Security.Models
{
    public class RotaUserManager : UserManager<RotaUser, long>
    {
        public RotaUserManager()
            : this(new RotaUserStore())
        {
        }

        public RotaUserManager(RotaUserStore userStore)
            : base(userStore)
        {
        }
    }
}