using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Rota.Security.DataContext;
using Rota.Security.Models;

namespace Rota.Security
{
    public class RotaDbInitializer : DropCreateDatabaseAlways<RotaDbContext>
    {
        protected override void Seed(RotaDbContext context)
        {
            InitializeIdentityForEF(context);
            base.Seed(context);
        }

        private async void InitializeIdentityForEF(RotaDbContext context)
        {
            var UserManager = new RotaUserManager(context);
            var RoleManager = new RotaRoleManager(context);
            // var myinfo = new MyUserInfo() { FirstName = "Pranav", LastName = "Rastogi" };
            string name = "Admin";
            string password = "123456";
            string test = "test";

            //Create Role Test and User Test
            await RoleManager.CreateAsync(new RotaRole(test));
            await UserManager.CreateAsync(new RotaUser() { UserName = test });

            //Create Role Admin if it does not exist
            if (!(await RoleManager.RoleExistsAsync(name)))
            {
                var roleresult = await RoleManager.CreateAsync(new RotaRole(name));
            }
            //Create User=Admin with password=123456
            var user = new RotaUser();
            user.UserName = name;
            //user.HomeTown = "Seattle";
            //user.MyUserInfo = myinfo;
            var adminresult = await UserManager.CreateAsync(user, password);

            //Add User Admin to Role Admin
            if (adminresult.Succeeded)
            {
                var result = await UserManager.AddToRoleAsync(user.Id, name);
            }
        }
    }
}