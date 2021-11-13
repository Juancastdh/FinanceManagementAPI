using FinanceManagement.Core.Entities;
using FinanceManagement.Core.Repositories;
using FinanceManagement.Core.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceManagement.Core.Managers.Implementations
{
    public class CategoriesManager: ICategoriesManager
    {
        private readonly IUnitOfWork UnitOfWork;

        public CategoriesManager(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public IEnumerable<Category> GetAllCategories()
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

        public Category GetCategoryById(int id)
        {
            IRepository<Category> categoriesRepository = UnitOfWork.GetRepository<Category>();
            Category category = categoriesRepository.GetById(id);
            return category;
        }

        public void UpdateCategory(Category category)
        {
            IRepository<Category> categoriesRepository = UnitOfWork.GetRepository<Category>();
            categoriesRepository.Update(category);
            UnitOfWork.SaveChanges();
        }


    }
}
