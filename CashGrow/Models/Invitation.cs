﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CashGrow.Models
{
    public class Invitation
    {
        public int Id { get; set; }

        public int HouseholdId { get; set; }

        public virtual Household Household { get; set; }

        public string Body { get; set; }

        //This should start as true and then be flipped to false under certain conditions
        public bool IsValid { get; set; }

        public DateTime Created { get; set; }

        public int TTL { get; internal set; }

        //if(DateTime.Now > Created.AddDays(TTL){IsValid = false


        [Display(Name ="Recipient Email")]
        public string RecipientEmail { get; set; }

        public Guid Code { get; set; }

        public Invitation(int hhId)
        { 
            Created = DateTime.Now;
            IsValid = true;
            TTL = 3;
            HouseholdId = hhId;
        }

        public Invitation()
        {
            Created = DateTime.Now;
            IsValid = true;
            TTL = 3;
        }
    }
}