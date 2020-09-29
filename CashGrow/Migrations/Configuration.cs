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
            var demoNewUserEmail = WebConfigurationManager.AppSettings["DemoNewUserEmail"];

            var demoHeadPassword = WebConfigurationManager.AppSettings["DemoHeadPassword"];
            var demoMemberPassword = WebConfigurationManager.AppSettings["DemoMemberPassword"];
            var demoNewUserPassword = WebConfigurationManager.AppSettings["DemoNewUserPassword"];



            if (!context.Users.Any(u => u.Email == demoHeadEmail))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = demoHeadEmail,
                    UserName = demoHeadEmail,
                    FirstName = "Erin",
                    LastName = "Crommett, MBA",
                }, demoHeadPassword);
                var userId = userManager.FindByEmail(demoHeadEmail).Id;
                userManager.AddToRole(userId, "Head");

            };

            if (!context.Users.Any(u => u.Email == demoMemberEmail))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = demoMemberEmail,
                    UserName = demoMemberEmail,
                    FirstName = "Tiffany",
                    LastName = "Thomas, MBA",
                }, demoMemberPassword);
                var userId = userManager.FindByEmail(demoMemberEmail).Id;
                userManager.AddToRole(userId, "Member");
            };

            if (!context.Users.Any(u => u.Email == demoNewUserEmail))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = demoNewUserEmail,
                    UserName = demoNewUserEmail,
                    FirstName = "Ruth",
                    LastName = "Bader Ginsburg",
                }, demoMemberPassword);
                var userId = userManager.FindByEmail(demoNewUserEmail).Id;
                userManager.AddToRole(userId, "New User");
            };

            #endregion

            #region Seed Household
            Household newHousehold = new Household(true);
            //if (!context.Households.Any())
            //{
                //newHousehold = new Household(true);

                newHousehold.Created = DateTime.Now;
                newHousehold.Greeting = "This is the Seed House";
                newHousehold.IsDeleted = false;
                newHousehold.HouseholdName = "Semillas";
                newHousehold.OwnerId = userManager.FindByEmail(demoHeadEmail).Id;

                context.Households.AddOrUpdate(newHousehold);
                context.SaveChanges();
            //};

            #endregion

            #region Assign Members to Household

            var owner = userManager.FindByEmail(demoHeadEmail);
            var household = context.Households.FirstOrDefault(h => h.OwnerId == owner.Id);
            owner.HouseholdId = household.Id;

            foreach (var member in context.Users.ToList())
            {
                member.HouseholdId = household.Id;
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
            if (!context.Budgets.Any())
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
            if (!context.BudgetItems.Any())
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
