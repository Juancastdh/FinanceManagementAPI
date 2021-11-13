using FinanceManagement.Core.Entities;
using FinanceManagement.Core.Repositories;
using FinanceManagement.Core.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceManagement.Core.Managers.Implementations
{
    public class CategoriesManager
    {
        private readonly IUnitOfWork UnitOfWork;

        public CategoriesManager(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public IEnumerable<Category> GetCategories()
        {
            IRepository<Category> categoriesRepository = UnitOfWork.GetRepository<Category>();

            IEnumerable<Category> categories = categoriesRepository.GetAll();

            return categories;
        }

        public void AddCategory(Category category)
        {
            IRepository<Category> categoriesRepository = UnitOfWork.GetRepository<Category>();

            categoriesRepository.Add(category);

            UnitOfWork.SaveChanges();
        }


    }
}
