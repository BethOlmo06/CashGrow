namespace CashGrow.Migrations
{
    using CashGrow.Enums;
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

            var demoHeadEmail = WebConfigurationManager.AppSettings["DemoHeadEmail"];
            var demoMemberEmail = WebConfigurationManager.AppSettings["DemoMemberEmail"];

            var demoHeadPassword = WebConfigurationManager.AppSettings["DemoHeadPassword"];
            var demoMemberPassword = WebConfigurationManager.AppSettings["DemoMemberPassword"];


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

            #region Seed Household
            Household newHousehold = null;
            if(!context.Households.Any())
            {
                newHousehold = new Household
                {
                    Created = DateTime.Now,
                    Greeting = "This is a Seeded House",
                    IsDeleted = false,
                    HouseholdName = "Seeded House"
                };
                context.Households.Add(newHousehold);
            }
            context.SaveChanges();
            #endregion

            #region Seed Bank Account
            var ownerId = context.Users.FirstOrDefault(u => u.Email == demoHeadEmail).Id;
            if (!context.BankAccounts.Any())
            {
                context.BankAccounts.Add(new BankAccount
                {
                    AccountName = "USAA Checking",
                    AccountType = AccountType.Checking,
                    Created = DateTime.Now,
                    CurrentBalance = 10000,
                    HouseholdId = newHousehold.Id,
                    IsDeleted = false,
                    OwnerId = ownerId,
                    StartingBalance = 10000,
                    WarningBalance = 1000
                });
                context.SaveChanges();
            }
            #endregion

            #region Seed Budget
            Budget budget = null;
            if(!context.Budgets.Any())
            {
                Console.WriteLine($"House Id: {newHousehold.Id}");
                Console.WriteLine($"Head of Household Id: {ownerId}");

                budget = new Budget(true)
                {
                    BudgetName = "Utilities",
                    Created = DateTime.Now,
                    HouseholdId = newHousehold.Id,
                    OwnerId = ownerId,
                    CurrentAmount = 0
                };
                context.Budgets.Add(budget);
                context.SaveChanges();
            }
            #endregion

            #region Seed Item
                if(!context.BudgetItems.Any())
            {
                context.BudgetItems.Add(new BudgetItem()
                {
                    CurrentAmount = 0,
                    TargetAmount = 150,
                    BudgetId = budget.Id,
                    Created = DateTime.Now,
                    ItemName = "Electric Bill",
                    IsDeleted = false
                });
                context.SaveChanges();
            }
            #endregion
        }
    }
}
