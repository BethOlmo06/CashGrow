using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Services.Configuration;

namespace CashGrow.Models
{
    public class Household
    {
        public int Id { get; set; }

        public string OwnerId { get; set; }

        public virtual ApplicationUser Owner { get; set; }

        [Display(Name = "Name")]
        public string HouseholdName { get; set; }

        public string Greeting { get; set; }

        public DateTime Created { get; set; }

        [Display(Name ="Delete Household")]
        public bool IsDeleted { get; set; }

        public virtual ICollection<ApplicationUser> Members { get; set; }

        public virtual ICollection<BankAccount> Accounts { get; set; }

        public virtual ICollection<Budget> Budgets { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }

        public virtual ICollection<Invitation> Invitations { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }

        public Household ()
        {
            Members = new HashSet<ApplicationUser>();
            Accounts = new HashSet<BankAccount>();
            Budgets = new HashSet<Budget>();
            Transactions = new HashSet<Transaction>();
            Invitations = new HashSet<Invitation>();
            Notifications = new HashSet<Notification>();
            Created = DateTime.Now;
        }
    }
}