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
    public class CategoriesManager : ICategoriesManager
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
            Category? category;

            IRepository<Category> categoriesRepository = UnitOfWork.GetRepository<Category>();
            category = categoriesRepository.GetById(id);

            if (category == null)
            {
                throw new DataNotFoundException();
            }

            return category;
        }

        public void UpdateCategory(Category category)
        {
            IRepository<Category> categoriesRepository = UnitOfWork.GetRepository<Category>();
            categoriesRepository.Update(category);
            UnitOfWork.SaveChanges();
        }

        public void DeleteCategoryById(int id)
        {

            IRepository<Category> categoriesRepository = UnitOfWork.GetRepository<Category>();

            Category categoryToDelete = categoriesRepository.GetById(id);

            categoryToDelete.Deleted = true;

            categoriesRepository.Update(categoryToDelete);

            UnitOfWork.SaveChanges();

        }
    }
}
