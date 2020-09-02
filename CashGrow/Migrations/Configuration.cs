namespace CashGrow.Migrations
{
    using CashGrow.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Configuration;

    internal sealed class Configuration : DbMigrationsConfiguration<CashGrow.Models.ApplicationDbContext>
    {

        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(CashGrow.Models.ApplicationDbContext context)
        {

            #region Role Creation
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "Head"))
            {
                roleManager.Create(new IdentityRole { Name = "Head" });
            }

            if (!context.Roles.Any(r => r.Name == "Member"))
            {
                roleManager.Create(new IdentityRole { Name = "Member" });
            }

            if (!context.Roles.Any(r => r.Name == "New User"))
            {
                roleManager.Create(new IdentityRole { Name = "New User" });
            }
            #endregion

            #region Users Creation
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            var demoAdminEmail = WebConfigurationManager.AppSettings["DemoAdminEmail"];
            var demoHeadEmail = WebConfigurationManager.AppSettings["DemoHeadEmail"];
            var demoMemberEmail = WebConfigurationManager.AppSettings["DemoMemberEmail"];
            var demoAdminPassword = WebConfigurationManager.AppSettings["DemoAdminPassword"];
            var demoHeadPassword = WebConfigurationManager.AppSettings["DemoHeadPassword"];
            var demoMemberPassword = WebConfigurationManager.AppSettings["DemoMemberPassword"];

            if (!context.Users.Any(u => u.Email == "DemAd06@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = demoAdminEmail,
                    UserName = demoAdminEmail,
                    FirstName = "Beth",
                    LastName = "Olmo, MS",
                }, demoAdminPassword);

                var userId = userManager.FindByEmail("DemAd06@mailinator.com").Id;
                userManager.AddToRole(userId, "Admin");
            };

            if (!context.Users.Any(u => u.Email == "DemHead@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = demoHeadEmail,
                    UserName = demoHeadEmail,
                    FirstName = "Erin",
                    LastName = "Crommett, MBA",
                }, demoHeadPassword);
                var userId = userManager.FindByEmail("DemHead@mailinator.com").Id;
                userManager.AddToRole(userId, "Head");
            };

            if (!context.Users.Any(u => u.Email == "DemMemb@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = demoMemberEmail,
                    UserName = demoMemberEmail,
                    FirstName = "Tiffany",
                    LastName = "Thomas, MBA",
                }, demoMemberPassword);
                var userId = userManager.FindByEmail("DemMemb@mailinator.com").Id;
                userManager.AddToRole(userId, "Member");
            };
            #endregion
        }
    }
}
