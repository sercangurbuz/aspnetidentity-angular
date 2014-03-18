using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using Rota.Security.Models;

namespace Rota.Security.DataContext
{
    public class RotaDbContext : IdentityDbContext<RotaUser, RotaRole, long, RotaUserLogin, RotaUserRole, RotaUserClaim>
    {
        public RotaDbContext()
            : base("RotaIdentityConnection")
        { }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //User map
            modelBuilder.Entity<IdentityUser>().ToTable("RotaUsers");
            modelBuilder.Entity<RotaUser>().ToTable("RotaUsers");
            //Roles Map
            modelBuilder.Entity<IdentityRole>().ToTable("RotaRoles");
            modelBuilder.Entity<RotaRole>().ToTable("RotaRoles");
            //User Roles Map
            modelBuilder.Entity<IdentityUserRole>().ToTable("RotaUserRoles");
            modelBuilder.Entity<RotaUserRole>().ToTable("RotaUserRoles");
            //User Logins Map
            modelBuilder.Entity<IdentityUserLogin>().ToTable("RotaUserLogins");
            modelBuilder.Entity<RotaUserLogin>().ToTable("RotaUserLogins");
            //User Claims Map
            modelBuilder.Entity<IdentityUserClaim>().ToTable("RotaUserClaims");
            modelBuilder.Entity<RotaUserClaim>().ToTable("RotaUserClaims");
        }
    }
}