using FinanceManagement.Core.Entities;
using FinanceManagement.Core.Models;
using FinanceManagement.Core.Repositories;
using FinanceManagement.Core.UnitOfWork;
using System;
using System.Collections.Generic;
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

        public decimal GetSumOfFinancialTransactions(int? periodId = null, int? categoryId = null, bool? isExpense = null)
        {
            throw new NotImplementedException();
        }

        public void UpdateFinancialTransaction(FinancialTransaction financialTransaction)
        {
            throw new NotImplementedException();
        }
    }
}
