using FinanceManagement.Core.Entities;
using FinanceManagement.Core.Exceptions;
using FinanceManagement.Core.Repositories;
using FinanceManagement.Core.UnitOfWork;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinanceManagement.Core.Managers.Implementations
{
    public class InvestmentFundTransactionsManager : IInvestmentFundTransactionsManager
    {
        private readonly IUnitOfWork UnitOfWork;

        public InvestmentFundTransactionsManager(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public IEnumerable<InvestmentFundTransaction> GetAllInvestmentFundTransactions(int? investmentFundCategoryId = null, int? investmentFundId = null)
        {
            IRepository<InvestmentFundTransaction> investmentFundTransactionsRepository = UnitOfWork.GetRepository<InvestmentFundTransaction>();
            IEnumerable<InvestmentFundTransaction> investmentFundTransactions = investmentFundTransactionsRepository.GetAll();

            if (investmentFundCategoryId.HasValue)
            {
                investmentFundTransactions = investmentFundTransactions.Where(t => t.InvestmentFundCategoryId == investmentFundCategoryId.Value);
            }

            if (investmentFundId.HasValue)
            {
                investmentFundTransactions = investmentFundTransactions.Where(t => t.InvestmentFundId == investmentFundId.Value);
            }

            return investmentFundTransactions;
        }

        public void AddInvestmentFundTransaction(InvestmentFundTransaction investmentFundTransaction)
        {
            IRepository<InvestmentFundTransaction> investmentFundTransactionsRepository = UnitOfWork.GetRepository<InvestmentFundTransaction>();

            if(investmentFundTransaction.Type == InvestmentFundTransactionType.Dividend && investmentFundTransaction.InvestmentFundCategoryId == null)
            {
                IEnumerable<InvestmentFundTransaction> simplifiedContributionInvestmentFundTransactions = GetSimplifiedContributionInvestmentFundTransactionsBasedOnDividendTransaction(investmentFundTransaction);

                foreach (InvestmentFundTransaction simplifiedContributionInvestmentFundTransaction in simplifiedContributionInvestmentFundTransactions)
                {
                    investmentFundTransactionsRepository.Add(simplifiedContributionInvestmentFundTransaction);
                }
            }
            else
            {
                investmentFundTransactionsRepository.Add(investmentFundTransaction);
            }
            UnitOfWork.SaveChanges();
        }

        private IEnumerable<InvestmentFundTransaction> GetSimplifiedContributionInvestmentFundTransactionsBasedOnDividendTransaction(InvestmentFundTransaction investmentFundTransaction)
        {
            List<InvestmentFundTransaction> simplifiedContributionInvestmentFundTransactions = new List<InvestmentFundTransaction>();
            List<InvestmentFundCategory> investmentFundCategories = [.. GetInvestmentFundCategoriesByInvestmentFundId(investmentFundTransaction.InvestmentFundId)];
            

            foreach (InvestmentFundCategory investmentFundCategory in investmentFundCategories)
            {
                decimal percentageOfInvestmentFundCategoryValueInInvestmentFund = GetPercentageofInvestmentFundCategoryValueInInvestmentFund(investmentFundCategory.Id, investmentFundCategory.InvestmentFundId);
                InvestmentFundTransaction simplifiedContributionInvestmentFundTransaction = new InvestmentFundTransaction
                {
                    Date = investmentFundTransaction.Date,
                    Amount = investmentFundTransaction.Amount * percentageOfInvestmentFundCategoryValueInInvestmentFund,
                    Type = investmentFundTransaction.Type,
                    InvestmentFundCategoryId = investmentFundCategory.Id,
                    Description = investmentFundTransaction.Description,
                    InvestmentFundId = investmentFundTransaction.InvestmentFundId
                };
                simplifiedContributionInvestmentFundTransactions.Add(simplifiedContributionInvestmentFundTransaction);
            }


            return simplifiedContributionInvestmentFundTransactions;
        }

        private IEnumerable<InvestmentFundCategory> GetInvestmentFundCategoriesByInvestmentFundId(int? investmentFundId)
        {
            IRepository<InvestmentFundCategory> investmentFundCategoriesRepository = UnitOfWork.GetRepository<InvestmentFundCategory>();
            return investmentFundCategoriesRepository.GetAll(investmentFundCategory => investmentFundCategory.InvestmentFundId == investmentFundId);
        }

        private decimal GetSumOfInvestmentFundTransactionValuesByInvestmentFundId(int investmentFundId)
        {
            IRepository<InvestmentFundTransaction> investmentFundTransactionsRepository = UnitOfWork.GetRepository<InvestmentFundTransaction>();
            IEnumerable<InvestmentFundTransaction> investmentFundTransactions = investmentFundTransactionsRepository.GetAll(investmentFundTransaction => investmentFundTransaction.InvestmentFundId == investmentFundId);

            decimal sumOfInvestmentFundTransactionValues = 0;

            foreach (InvestmentFundTransaction investmentFundTransaction in investmentFundTransactions)
            {
                if(investmentFundTransaction.Type == InvestmentFundTransactionType.Contribution || investmentFundTransaction.Type == InvestmentFundTransactionType.Dividend)
                {
                    sumOfInvestmentFundTransactionValues += investmentFundTransaction.Amount;
                }
                else if(investmentFundTransaction.Type == InvestmentFundTransactionType.Withdrawal)
                {
                    sumOfInvestmentFundTransactionValues -= investmentFundTransaction.Amount;
                }
                else
                {
                    throw new InvalidOperationException($"Invalid investment fund transaction type: {investmentFundTransaction.Type}");
                }
            }

            return sumOfInvestmentFundTransactionValues;
        }

        private decimal GetPercentageofInvestmentFundCategoryValueInInvestmentFund(int investmentFundCategoryId, int investmentFundId)
        {

            decimal sumOfInvestmentFundTransactionValuesByInvestmentFundId = GetSumOfInvestmentFundTransactionValuesByInvestmentFundId(investmentFundId);
            decimal sumOfInvestmentFundTransactionValuesByInvestmentFundCategoryId = GetSumOfInvestmentFundTransactionValuesByInvestmentFundCategoryId(investmentFundCategoryId);

            return sumOfInvestmentFundTransactionValuesByInvestmentFundCategoryId / sumOfInvestmentFundTransactionValuesByInvestmentFundId;
        }



        public InvestmentFundTransaction GetInvestmentFundTransactionById(int id)
        {
            IRepository<InvestmentFundTransaction> investmentFundTransactionsRepository = UnitOfWork.GetRepository<InvestmentFundTransaction>();
            var investmentFundTransaction = investmentFundTransactionsRepository.GetById(id);

            if (investmentFundTransaction == null)
            {
                throw new DataNotFoundException();
            }

            return investmentFundTransaction;
        }

        public void UpdateInvestmentFundTransaction(InvestmentFundTransaction investmentFundTransaction)
        {
            IRepository<InvestmentFundTransaction> investmentFundTransactionsRepository = UnitOfWork.GetRepository<InvestmentFundTransaction>();
            investmentFundTransactionsRepository.Update(investmentFundTransaction);
            UnitOfWork.SaveChanges();
        }

        public void DeleteInvestmentFundTransactionById(int id)
        {
            IRepository<InvestmentFundTransaction> investmentFundTransactionsRepository = UnitOfWork.GetRepository<InvestmentFundTransaction>();

            investmentFundTransactionsRepository.DeleteById(id);

            UnitOfWork.SaveChanges();
        }

        public decimal GetSumOfInvestmentFundTransactionValuesByInvestmentFundCategoryId(int investmentFundCategoryId)
        {
            IRepository<InvestmentFundTransaction> investmentFundTransactionsRepository = UnitOfWork.GetRepository<InvestmentFundTransaction>();
            IEnumerable<InvestmentFundTransaction> investmentFundTransactions = investmentFundTransactionsRepository.GetAll().Where(investmentFundTransaction => investmentFundTransaction.InvestmentFundCategoryId == investmentFundCategoryId);

            decimal sumOfInvestmentFundTransactionValues = 0;

            foreach (InvestmentFundTransaction investmentFundTransaction in investmentFundTransactions)
            {
                if(investmentFundTransaction.Type == InvestmentFundTransactionType.Contribution || investmentFundTransaction.Type == InvestmentFundTransactionType.Dividend)
                {
                    sumOfInvestmentFundTransactionValues += investmentFundTransaction.Amount;
                }
                else if(investmentFundTransaction.Type == InvestmentFundTransactionType.Withdrawal)
                {
                    sumOfInvestmentFundTransactionValues -= investmentFundTransaction.Amount;
                }
                else
                {
                    throw new InvalidOperationException($"Invalid investment fund transaction type: {investmentFundTransaction.Type}");
                }
            }

            return sumOfInvestmentFundTransactionValues;
        }
    }
}

            