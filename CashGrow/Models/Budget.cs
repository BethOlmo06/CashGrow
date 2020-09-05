﻿using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;

namespace CashGrow.Models
{
    public class Budget
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public int Id { get; set; }

        public int HouseholdId { get; set; }

        public virtual Household Household { get; set; }

        public string OwnerId { get; set; }

        public virtual ApplicationUser Owner { get; set; }

        public DateTime Created { get; set; }

        [Display(Name ="Name")]
        public string BudgetName { get; set; }

        //amount of money currently spent in category
        [Display(Name ="Current Amount")]
        public decimal CurrentAmount { get; set; }

        //projected total of category for the month based on budget items
        [NotMapped]
        [Display(Name ="Target Amount")]
        public decimal TargetAmount
        {
            get
            {
                var target = db.BudgetItems.Where(bI => bI.BudgetId == Id).Count();
                return target != 0 ? db.BudgetItems.Where(bI => bI.BudgetId == Id).Sum(s => s.TargetAmount) : 0;
            }
        }

        public virtual ICollection<BudgetItem> Items { get; set; }

        public Budget()
        {
            Items = new HashSet<BudgetItem>();
            Created = DateTime.Now;
            OwnerId = HttpContext.Current.User.Identity.GetUserId();
            CurrentAmount = 0;
        }
    }
}