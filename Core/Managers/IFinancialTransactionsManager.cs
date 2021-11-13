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
        void DeleteFinancialTransaction(FinancialTransaction financialTransaction);
        IEnumerable<FinancialTransaction> GetAllFinancialTransactions();
        decimal GetSumOfFinancialTransactionValues(int? periodId = null, int? categoryId = null, bool? isExpense = null);
        FinancialReport GetFinancialReport(int? periodId = null, int? categoryId = null, bool? isExpense = null);


    }
}
