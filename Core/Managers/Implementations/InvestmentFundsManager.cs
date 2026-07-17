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
    public class InvestmentFundsManager : IInvestmentFundsManager
    {
        private readonly IUnitOfWork UnitOfWork;

        public InvestmentFundsManager(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public IEnumerable<InvestmentFund> GetAllInvestmentFunds(bool? deleted = null)
        {


            IRepository<InvestmentFund> investmentFundsRepository = UnitOfWork.GetRepository<InvestmentFund>();

            IEnumerable<InvestmentFund> investmentFunds;

            if (deleted != null)
            {
                investmentFunds = investmentFundsRepository.GetAll(investmentFund => investmentFund.Deleted == deleted);
            }
            else
            {
                investmentFunds = investmentFundsRepository.GetAll();
            }



            return investmentFunds;


        }

        public void AddInvestmentFund(InvestmentFund investmentFund)
        {
            IRepository<InvestmentFund> investmentFundsRepository = UnitOfWork.GetRepository<InvestmentFund>();

            investmentFundsRepository.Add(investmentFund);

            UnitOfWork.SaveChanges();
        }

        public InvestmentFund GetInvestmentFundById(int id)
        {
            InvestmentFund? investmentFund;

            IRepository<InvestmentFund> investmentFundsRepository = UnitOfWork.GetRepository<InvestmentFund>();
            investmentFund = investmentFundsRepository.GetById(id);

            if (investmentFund == null)
            {
                throw new DataNotFoundException();
            }

            return investmentFund;
        }

        public void UpdateInvestmentFund(InvestmentFund investmentFund)
        {
            IRepository<InvestmentFund> investmentFundsRepository = UnitOfWork.GetRepository<InvestmentFund>();
            investmentFundsRepository.Update(investmentFund);
            UnitOfWork.SaveChanges();
        }

        public void DeleteInvestmentFundById(int id)
        {

            IRepository<InvestmentFund> investmentFundsRepository = UnitOfWork.GetRepository<InvestmentFund>();

            InvestmentFund investmentFundToDelete = investmentFundsRepository.GetById(id);

            investmentFundToDelete.Deleted = true;

            investmentFundsRepository.Update(investmentFundToDelete);

            UnitOfWork.SaveChanges();

        }
    }
}
