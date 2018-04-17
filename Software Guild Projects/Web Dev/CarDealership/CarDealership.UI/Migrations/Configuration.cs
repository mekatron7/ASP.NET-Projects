namespace CarDealership.UI.Migrations
{
    using CarDealership.UI.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CarDealership.UI.Models.Identity.CarDealershipDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CarDealership.UI.Models.Identity.CarDealershipDbContext context)
        {
            MakeUserInRole("masteradmin", "kurama", "admin", context);
            MakeUserInRole("salesman", "test123", "sales", context);
            MakeUserInRole("johncripple", "test123", "disabled", context);
            
        }

        void MakeUserInRole(string username, string password, string role, CarDealership.UI.Models.Identity.CarDealershipDbContext context)
        {
            //Load the user and role managers with our custom models
            var userMgr = new UserManager<AppUser>(new UserStore<AppUser>(context));
            var roleMgr = new RoleManager<AppRole>(new RoleStore<AppRole>(context));

            //Have we loaded roles already?
            CreateNewRole(role, roleMgr);


            //Create the admin role


            //Create the default user
            if (userMgr.FindByName(username) == null)
            {
                var newUser = new AppUser()
                {
                    UserName = username
                };

                //Create the user with the manager class
                userMgr.Create(newUser, password);
            }

            var user = userMgr.FindByName(username);
            if (!userMgr.IsInRole(user.Id, role))
            {
                //Add the user to the specified role
                userMgr.AddToRole(user.Id, role);
            }
        }

        void CreateNewRole(string role, RoleManager<AppRole> mgr)
        {
            if (!mgr.RoleExists(role))
            {
                mgr.Create(new AppRole() { Name = role });
            }
        }
    }
}
