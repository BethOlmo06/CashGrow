using CashGrow.Models;
using System.Collections;
using System.Collections.Generic;

namespace CashGrow.ViewModels
{
    public class BudgetWizardVM
    {
        public Budget Budget { get; set; }

        public ICollection<BudgetItem> BudgetItems { get; set; }
    }
}