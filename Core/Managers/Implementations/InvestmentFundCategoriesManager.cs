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
    public class InvestmentFundCategoriesManager : IInvestmentFundCategoriesManager
    {
        private readonly IUnitOfWork UnitOfWork;

        public InvestmentFundCategoriesManager(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public IEnumerable<InvestmentFundCategory> GetAllInvestmentFundCategories(bool? deleted = null, int? investmentFundId = null)
        {


            IRepository<InvestmentFundCategory> investmentFundCategoriesRepository = UnitOfWork.GetRepository<InvestmentFundCategory>();

            IEnumerable<InvestmentFundCategory> investmentFundCategories;

            if (deleted != null)
            {
                investmentFundCategories = investmentFundCategoriesRepository.GetAll(investmentFundCategory => investmentFundCategory.Deleted == deleted, includeProperties: "InvestmentFund");
            }
            else
            {
                investmentFundCategories = investmentFundCategoriesRepository.GetAll(includeProperties: "InvestmentFund");
            }

            if (investmentFundId != null)
            {
                investmentFundCategories = investmentFundCategories.Where(investmentFundCategory => investmentFundCategory.InvestmentFundId == investmentFundId);
            }

            return investmentFundCategories;


        }

        public void AddInvestmentFundCategory(InvestmentFundCategory investmentFundCategory)
        {
            IRepository<InvestmentFundCategory> investmentFundCategoriesRepository = UnitOfWork.GetRepository<InvestmentFundCategory>();

            investmentFundCategoriesRepository.Add(investmentFundCategory);

            UnitOfWork.SaveChanges();
        }

        public InvestmentFundCategory GetInvestmentFundCategoryById(int id)
        {
            InvestmentFundCategory? investmentFundCategory;

            IRepository<InvestmentFundCategory> investmentFundCategoriesRepository = UnitOfWork.GetRepository<InvestmentFundCategory>();
            investmentFundCategory = investmentFundCategoriesRepository.GetById(id);

            if (investmentFundCategory == null)
            {
                throw new DataNotFoundException();
            }

            return investmentFundCategory;
        }

        public void UpdateInvestmentFundCategory(InvestmentFundCategory investmentFundCategory)
        {
            IRepository<InvestmentFundCategory> investmentFundCategoriesRepository = UnitOfWork.GetRepository<InvestmentFundCategory>();
            investmentFundCategoriesRepository.Update(investmentFundCategory);
            UnitOfWork.SaveChanges();
        }

        public void DeleteInvestmentFundCategoryById(int id)
        {

            IRepository<InvestmentFundCategory> investmentFundCategoriesRepository = UnitOfWork.GetRepository<InvestmentFundCategory>();

            InvestmentFundCategory investmentFundCategoryToDelete = investmentFundCategoriesRepository.GetById(id);

            investmentFundCategoryToDelete.Deleted = true;

            investmentFundCategoriesRepository.Update(investmentFundCategoryToDelete);

            UnitOfWork.SaveChanges();

        }
    }
}
