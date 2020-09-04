using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CashGrow.Models
{
    public class ApplicationUser : IdentityUser
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Required]
        [Display(Name = "First Name")]
        [StringLength(50, MinimumLength =2, ErrorMessage ="First Name must be between 2 and 50 characters")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First Name must be between 2 and 50 characters")]
        public string LastName { get; set; }

        public int? HouseholdId { get; set; }

        public virtual Household Household { get; set; }

        public string AvatarPath { get; set; }

        [NotMapped]
        [Display(Name ="Full Name")]
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        [NotMapped]
        public string HouseholdName
        {
            get {
                var hhId = HouseholdId;
                if(hhId == null)
                {
                    return "";
                }
                else
                {
                    return db.Households.Find(hhId).HouseholdName;
                }
            }
        }

        public virtual ICollection<Budget> Budgets{ get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<BankAccount> Accounts { get; set; }

        public ApplicationUser()
        {
            Budgets = new HashSet<Budget>();
            Notifications = new HashSet<Notification>();
            Transactions = new HashSet<Transaction>();
            Accounts = new HashSet<BankAccount>();
            AvatarPath = "/Avatars/DefaultAvatar.png";
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            var hhId = HouseholdId != null ? HouseholdId.ToString() : "";
            userIdentity.AddClaim(new Claim("HouseholdId", hhId));
            userIdentity.AddClaim(new Claim("FullName", FullName));
            userIdentity.AddClaim(new Claim("AvatarPath", AvatarPath));

            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<BudgetItem> BudgetItems { get; set; }

        public System.Data.Entity.DbSet<CashGrow.Models.BankAccount> BankAccounts { get; set; }

        public System.Data.Entity.DbSet<CashGrow.Models.Household> Households { get; set; }

        public System.Data.Entity.DbSet<CashGrow.Models.Budget> Budgets { get; set; }

        public System.Data.Entity.DbSet<CashGrow.Models.Invitation> Invitations { get; set; }

        public System.Data.Entity.DbSet<CashGrow.Models.Notification> Notifications { get; set; }

        public System.Data.Entity.DbSet<CashGrow.Models.Transaction> Transactions { get; set; }

    }
}