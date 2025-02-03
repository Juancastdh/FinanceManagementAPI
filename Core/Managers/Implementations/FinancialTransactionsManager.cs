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

        public void DeleteFinancialTransactionById(int id)
        {


            IRepository<FinancialTransaction> financialTransactionsRepository = UnitOfWork.GetRepository<FinancialTransaction>();

            financialTransactionsRepository.DeleteById(id);

            UnitOfWork.SaveChanges();


        }

        public IEnumerable<FinancialTransaction> GetAllFinancialTransactions()
        {

            IRepository<FinancialTransaction> financialTransactionsRepository = UnitOfWork.GetRepository<FinancialTransaction>();
            return financialTransactionsRepository.GetAll(includeProperties: "Category,Account");


        }

        public FinancialReport GetFinancialReport(int? periodId = null, int? categoryId = null, bool? isExpense = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            ValidateGetFinancialReportParameters(startDate, endDate);
            IRepository<FinancialTransaction> financialTransactionsRepository = UnitOfWork.GetRepository<FinancialTransaction>();
            IEnumerable<FinancialTransaction> financialTransactions = financialTransactionsRepository.GetAll(f => (periodId == null || f.PeriodId == periodId) && (categoryId == null || f.CategoryId == categoryId) && (isExpense == null || f.IsExpense == isExpense) && (startDate == null || f.Date >= startDate) && (endDate == null || f.Date <= endDate), includeProperties: "Category,Account");

            financialTransactions = financialTransactions.OrderBy(transaction => transaction.Date);

            decimal sumOfFinancialTransactionsValue = GetSumOfFinancialTransactionValues(financialTransactions);

            FinancialReport financialReport = new FinancialReport
            {
                FinancialTransactions = financialTransactions,
                TotalValue = sumOfFinancialTransactionsValue
            };

            return financialReport;



        }

        private void ValidateGetFinancialReportParameters(DateTime? startDate = null, DateTime? endDate = null)
        {
            if(startDate != null && endDate != null)
            {
                if(startDate > endDate)
                {
                    throw new InvalidOperationException("End date cannot be lower than start date");
                }
            }
        }

        public FinancialTransaction GetFinancialTransactionById(int id)
        {
            FinancialTransaction? financialTransaction;


            IRepository<FinancialTransaction> financialTransactionsRepository = UnitOfWork.GetRepository<FinancialTransaction>();
            financialTransaction = financialTransactionsRepository.GetById(id);


            if (financialTransaction == null)
            {
                throw new DataNotFoundException();
            }

            return financialTransaction;
        }

        public decimal GetSumOfFinancialTransactionValues(IEnumerable<FinancialTransaction> financialTransactions)
        {


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

        public void AddFinancialTransactions(IEnumerable<FinancialTransaction> financialTransactions)
        {
            IRepository<FinancialTransaction> financialTransactionsRepository = UnitOfWork.GetRepository<FinancialTransaction>();

            foreach(FinancialTransaction financialTransaction in financialTransactions)
            {
                financialTransactionsRepository.Add(financialTransaction);
            }     

            UnitOfWork.SaveChanges();
        }

        public FinancialTransaction GetFixedFinancialTransaction(FinancialTransaction financialTransaction)
        {
            FinancialTransaction fixedFinancialTransaction = financialTransaction;

            fixedFinancialTransaction.IsExpense = IsExpenseValue(financialTransaction.Value);
            fixedFinancialTransaction.PeriodId = GetPeriodIdByDate(financialTransaction.Date);
            fixedFinancialTransaction.Value = Math.Abs(financialTransaction.Value);

            return fixedFinancialTransaction;

        }

        public IEnumerable<FinancialTransaction> GetFixedFinancialTransactions(IEnumerable<FinancialTransaction> financialTransactions)
        {
            List<FinancialTransaction> fixedFinancialTransactions = new List<FinancialTransaction>();

            foreach(FinancialTransaction financialTransaction in financialTransactions)
            {
                fixedFinancialTransactions.Add(GetFixedFinancialTransaction(financialTransaction));
            }

            return fixedFinancialTransactions;
        }

        private bool IsExpenseValue(decimal value)
        {
            return value > 0;
        }

        private int GetPeriodIdByDate(DateTime date)
        {
            int periodId = 0;
            IRepository<Period> periodsRepository = UnitOfWork.GetRepository<Period>();

            IEnumerable<Period> periods = periodsRepository.GetAll();

            Period? selectedPeriod = periods.FirstOrDefault(period => date >= period.StartDate && date < period.EndDate);

            if (selectedPeriod != null)
            {
                periodId = selectedPeriod.Id;
            }

            return periodId;
        }
    }
}
