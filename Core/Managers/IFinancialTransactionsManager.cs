using FinanceManagement.Core.Entities;
using FinanceManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace FinanceManagement.Core.Managers
{
    public interface IFinancialTransactionsManager
    {
        void AddFinancialTransaction(FinancialTransaction financialTransaction);
        void UpdateFinancialTransaction(FinancialTransaction financialTransaction);
        void DeleteFinancialTransactionById(int id);
        IEnumerable<FinancialTransaction> GetAllFinancialTransactions();
        decimal GetSumOfFinancialTransactionValues(IEnumerable<FinancialTransaction> financialTransactions);
        FinancialReport GetFinancialReport(int? periodId = null, int? categoryId = null, bool? isExpense = null);
        FinancialTransaction GetFinancialTransactionById(int id);
        void AddFinancialTransactions(IEnumerable<FinancialTransaction> financialTransactions);
        void DeleteFinancialTransactions(IEnumerable<FinancialTransaction> financialTransactions);


    }
}
