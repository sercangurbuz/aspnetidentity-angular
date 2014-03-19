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
            modelBuilder.Entity<RotaUser>().ToTable("RotaUsers");
            //Roles Map
            modelBuilder.Entity<RotaRole>().ToTable("RotaRoles");
            //User Roles Map
            modelBuilder.Entity<RotaUserRole>().ToTable("RotaUserRoles");
            //User Logins Map
            modelBuilder.Entity<RotaUserLogin>().ToTable("RotaUserLogins");
            //User Claims Map
            modelBuilder.Entity<RotaUserClaim>().ToTable("RotaUserClaims");
            //
            modelBuilder.Entity<RotaUser>().HasKey<long>(l => l.Id);
            modelBuilder.Entity<RotaRole>().HasKey<long>(r => r.Id);
            modelBuilder.Entity<RotaUserRole>().HasKey(r => new { r.RoleId, r.UserId });
        }
    }
}