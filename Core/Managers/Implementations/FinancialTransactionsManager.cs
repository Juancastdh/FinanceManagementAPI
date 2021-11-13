using FinanceManagement.Core.Entities;
using FinanceManagement.Core.Models;
using FinanceManagement.Core.Repositories;
using FinanceManagement.Core.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinanceManagement.Core.Managers.Implementations
{
    public class FinancialTransactionsManager : IFinancialTransactionsManager
    {

        private readonly IUnitOfWork UnitOfWork;

        public FinancialTransactionsManager(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }


        public void AddFinancialTransaction(FinancialTransaction financialTransaction)
        {
            IRepository<FinancialTransaction> financialTransactionsRepository = UnitOfWork.GetRepository<FinancialTransaction>();

            financialTransactionsRepository.Add(financialTransaction);

            UnitOfWork.SaveChanges();
        }

        public void DeleteFinancialTransaction(FinancialTransaction financialTransaction)
        {
            IRepository<FinancialTransaction> financialTransactionsRepository = UnitOfWork.GetRepository<FinancialTransaction>();

            financialTransactionsRepository.DeleteById(financialTransaction.Id);

            UnitOfWork.SaveChanges();
        }

        public IEnumerable<FinancialTransaction> GetAllFinancialTransactions()
        {
            IRepository<FinancialTransaction> financialTransactionsRepository = UnitOfWork.GetRepository<FinancialTransaction>();
            return financialTransactionsRepository.GetAll();
        }

        public FinancialReport GetFinancialReport(int? periodId = null, int? categoryId = null, bool? isExpense = null)
        {
            throw new NotImplementedException();
        }

        public decimal GetSumOfFinancialTransactionValues(int? periodId = null, int? categoryId = null, bool? isExpense = null)
        {
            IRepository<FinancialTransaction> financialTransactionsRepository = UnitOfWork.GetRepository<FinancialTransaction>();

            IEnumerable<FinancialTransaction> financialTransactions = financialTransactionsRepository.GetAll(f => (periodId == null || f.PeriodId == periodId) && (categoryId == null || f.CategoryId == categoryId) && (isExpense == null || f.IsExpense == isExpense));

            IEnumerable<FinancialTransaction> incomeTransactions = financialTransactions.Where(f => f.IsExpense == false);

            IEnumerable<FinancialTransaction> expenseTransactions = financialTransactions.Where(f => f.IsExpense == true);

            decimal sumOfIncomeTransactionValues = incomeTransactions.Sum(f => f.Value);

            decimal sumOfExpenseTransactionValues = expenseTransactions.Sum(f => f.Value);

            decimal sumOfFinancialTransactionValues = sumOfIncomeTransactionValues - sumOfExpenseTransactionValues;

            return sumOfFinancialTransactionValues;
        }

        public void UpdateFinancialTransaction(FinancialTransaction financialTransaction)
        {
            IRepository<FinancialTransaction> financialTransactionsRepository = UnitOfWork.GetRepository<FinancialTransaction>();

            financialTransactionsRepository.Update(financialTransaction);

            UnitOfWork.SaveChanges();
        }
    }
}
