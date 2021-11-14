using FinanceManagement.Core.Entities;
using FinanceManagement.Core.Exceptions;
using FinanceManagement.Core.Models;
using FinanceManagement.Core.Repositories;
using FinanceManagement.Core.UnitOfWork;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinanceManagement.Core.Managers.Implementations
{
    public class FinancialTransactionsManager : IFinancialTransactionsManager
    {

        private readonly IUnitOfWork UnitOfWork;
        private readonly ILogger<FinancialTransactionsManager> Logger;

        public FinancialTransactionsManager(IUnitOfWork unitOfWork, ILogger<FinancialTransactionsManager> logger)
        {
            UnitOfWork = unitOfWork;
            Logger = logger;
        }


        public void AddFinancialTransaction(FinancialTransaction financialTransaction)
        {
            try
            {
                IRepository<FinancialTransaction> financialTransactionsRepository = UnitOfWork.GetRepository<FinancialTransaction>();

                financialTransactionsRepository.Add(financialTransaction);

                UnitOfWork.SaveChanges();
            }
            catch (Exception exception)
            {
                Logger.LogError(exception.Message, exception);
                throw;
            }

        }

        public void AddFinancialTransactions(IEnumerable<FinancialTransaction> financialTransactions)
        {
            try
            {
                IRepository<FinancialTransaction> financialTransactionsRepository = UnitOfWork.GetRepository<FinancialTransaction>();

                financialTransactionsRepository.AddMany(financialTransactions);

                UnitOfWork.SaveChanges();
            }
            catch (Exception exception)
            {
                Logger.LogError(exception.Message, exception);
                throw;
            }
        }

        public void DeleteFinancialTransactionById(int id)
        {

            try
            {
                IRepository<FinancialTransaction> financialTransactionsRepository = UnitOfWork.GetRepository<FinancialTransaction>();

                financialTransactionsRepository.DeleteById(id);

                UnitOfWork.SaveChanges();
            }
            catch(Exception exception)
            {
                Logger.LogError(exception.Message, exception);
                throw;
            }

        }

        public void DeleteFinancialTransactions(IEnumerable<FinancialTransaction> financialTransactions)
        {
            try
            {
                IRepository<FinancialTransaction> financialTransactionsRepository = UnitOfWork.GetRepository<FinancialTransaction>();

                financialTransactionsRepository.DeleteMany(financialTransactions);

                UnitOfWork.SaveChanges();
            }
            catch (Exception exception)
            {
                Logger.LogError(exception.Message, exception);
                throw;
            }
        }

        public IEnumerable<FinancialTransaction> GetAllFinancialTransactions()
        {
            try
            {
                IRepository<FinancialTransaction> financialTransactionsRepository = UnitOfWork.GetRepository<FinancialTransaction>();
                return financialTransactionsRepository.GetAll();
            }
            catch (Exception exception)
            {
                Logger.LogError(exception.Message, exception);
                throw;
            }

        }

        public FinancialReport GetFinancialReport(int? periodId = null, int? categoryId = null, bool? isExpense = null)
        {

            try
            {
                IRepository<FinancialTransaction> financialTransactionsRepository = UnitOfWork.GetRepository<FinancialTransaction>();
                IEnumerable<FinancialTransaction> financialTransactions = financialTransactionsRepository.GetAll(f => (periodId == null || f.PeriodId == periodId) && (categoryId == null || f.CategoryId == categoryId) && (isExpense == null || f.IsExpense == isExpense));

                financialTransactions = financialTransactions.OrderBy(transaction => transaction.Date);

                decimal sumOfFinancialTransactionsValue = GetSumOfFinancialTransactionValues(financialTransactions);

                FinancialReport financialReport = new FinancialReport
                {
                    FinancialTransactions = financialTransactions,
                    TotalValue = sumOfFinancialTransactionsValue
                };

                return financialReport;
            }
            catch (Exception exception)
            {
                Logger.LogError(exception.Message, exception);
                throw;
            }


        }

        public FinancialTransaction GetFinancialTransactionById(int id)
        {
            FinancialTransaction? financialTransaction;

            try
            {
                IRepository<FinancialTransaction> financialTransactionsRepository = UnitOfWork.GetRepository<FinancialTransaction>();
                financialTransaction = financialTransactionsRepository.GetById(id);
            }
            catch(Exception exception)
            {
                Logger.LogError(exception.Message, exception);
                throw;
            }

            if(financialTransaction == null)
            {
                throw new DataNotFoundException();
            }

            return financialTransaction;
        }

        public decimal GetSumOfFinancialTransactionValues(IEnumerable<FinancialTransaction> financialTransactions)
        {

            try
            {
                IEnumerable<FinancialTransaction> incomeTransactions = financialTransactions.Where(f => f.IsExpense == false);

                IEnumerable<FinancialTransaction> expenseTransactions = financialTransactions.Where(f => f.IsExpense == true);

                decimal sumOfIncomeTransactionValues = incomeTransactions.Sum(f => f.Value);

                decimal sumOfExpenseTransactionValues = expenseTransactions.Sum(f => f.Value);

                decimal sumOfFinancialTransactionValues = sumOfIncomeTransactionValues - sumOfExpenseTransactionValues;

                return sumOfFinancialTransactionValues;
            }
            catch (Exception exception)
            {
                Logger.LogError(exception.Message, exception);
                throw;
            }

        }

        public void UpdateFinancialTransaction(FinancialTransaction financialTransaction)
        {
            try
            {
                IRepository<FinancialTransaction> financialTransactionsRepository = UnitOfWork.GetRepository<FinancialTransaction>();

                financialTransactionsRepository.Update(financialTransaction);

                UnitOfWork.SaveChanges();
            }
            catch (Exception exception)
            {
                Logger.LogError(exception.Message, exception);
                throw;
            }
        }
    }
}
