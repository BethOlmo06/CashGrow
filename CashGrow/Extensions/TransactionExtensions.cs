using CashGrow.Enums;
using CashGrow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace CashGrow.Extensions
{
    public static class TransactionExtensions
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        #region Notes to self
        //What does a transaction do?
        //If TransactionType.Deposit - increases the current amount of the BankAccount it was deposited into
        //If TransactionType.Withdrawal = DEcreases the current amount of the BankAccount, INcreases the current amount of Budget and BudgetItem
        //Optional: TransactionType.Transfer - reduces the current amount of BankAccount1 and increases current amount of BankAccount2
        #endregion

        #region Notes to future Devs working on the code after me
        //This code is called in the Account Controller - updates need to be made in both locations together

        #endregion

        public static void UpdateBalances(this Transaction transaction)
        {
            UpdateBankBalance(transaction);

            //Deposits do not affect Budget or BudgetItem, so we can test for the transaction type before calling those methods
            if(transaction.TransactionType == TransactionType.Withdrawal)
            {
                UpdateBudgetAmount(transaction);
                UpdateBudgetItemAmount(transaction);
            }
        }

        private  static void UpdateBankBalance(Transaction transaction)
        {
            var bankAccount = db.BankAccounts.Find(transaction.AccountId);

            if (transaction.TransactionType == TransactionType.Deposit)
            {
                bankAccount.CurrentBalance += transaction.Amount;

            }
                else if(transaction.TransactionType == TransactionType.Withdrawal)
            {
                bankAccount.CurrentBalance -= transaction.Amount;
            }
            db.SaveChanges();

        }

        private static void UpdateBudgetAmount(Transaction transaction)
        {
            //var budgetId = db.BudgetItems.Find(transaction.BudgetItemId);
            //var budget = db.Budgets.Find(budgetId);
            var budget = db.Budgets.Find(transaction.BudgetItem.BudgetId);
            budget.CurrentAmount += transaction.Amount;
            db.SaveChanges();
        }

        private static void UpdateBudgetItemAmount(Transaction transaction)
        {
            var budget = db.Budgets.Find(transaction.BudgetItemId);
            budget.CurrentAmount += transaction.Amount;
            db.SaveChanges();
        }

        //Additional functionality needed
        //What happens when I edit a transaction? - Momento object - .AsNoTracking()
        //What happens 
    }
}