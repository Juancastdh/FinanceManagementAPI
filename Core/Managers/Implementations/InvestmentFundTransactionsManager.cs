using FinanceManagement.Core.Entities;
using FinanceManagement.Core.Exceptions;
using FinanceManagement.Core.Repositories;
using FinanceManagement.Core.UnitOfWork;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

        public IEnumerable<InvestmentFundTransaction> GetAllInvestmentFundTransactions()
        {
            IRepository<InvestmentFundTransaction> investmentFundTransactionsRepository = UnitOfWork.GetRepository<InvestmentFundTransaction>();
            return investmentFundTransactionsRepository.GetAll();
        }

        public void AddInvestmentFundTransaction(InvestmentFundTransaction investmentFundTransaction)
        {
            IRepository<InvestmentFundTransaction> investmentFundTransactionsRepository = UnitOfWork.GetRepository<InvestmentFundTransaction>();

            if(investmentFundTransaction.Type == InvestmentFundTransactionType.Dividend)
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
                InvestmentFundTransaction simplifiedContributionInvestmentFundTransaction = new InvestmentFundTransaction
                {
                    Date = investmentFundTransaction.Date,
                    Amount = investmentFundTransaction.Amount,
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
    }
}

            