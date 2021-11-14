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
    public class CategoriesManager: ICategoriesManager
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly ILogger<CategoriesManager> Logger;

        public CategoriesManager(IUnitOfWork unitOfWork, ILogger<CategoriesManager> logger)
        {
            UnitOfWork = unitOfWork;
            Logger = logger;
        }

        public IEnumerable<Category> GetAllCategories()
        {

            try
            {
                IRepository<Category> categoriesRepository = UnitOfWork.GetRepository<Category>();

                IEnumerable<Category> categories = categoriesRepository.GetAll();

                return categories;

            }
            catch (Exception exception)
            {
                Logger.LogError(exception.Message, exception);
                throw;
            }


        }

        public void AddCategory(Category category)
        {
            try
            {
                IRepository<Category> categoriesRepository = UnitOfWork.GetRepository<Category>();

                categoriesRepository.Add(category);

                UnitOfWork.SaveChanges();
            }
            catch(Exception exception)
            {
                Logger.LogError(exception.Message, exception);
                throw;
            }
        }

        public Category GetCategoryById(int id)
        {
            Category? category;


            try
            {
                IRepository<Category> categoriesRepository = UnitOfWork.GetRepository<Category>();
                category = categoriesRepository.GetById(id);
            }
            catch(Exception exception)
            {
                Logger.LogError(exception.Message, exception);
                throw;
            }

            if(category == null)
            {
                throw new DataNotFoundException();
            }

            return category;
        }

        public void UpdateCategory(Category category)
        {
            try
            {
                IRepository<Category> categoriesRepository = UnitOfWork.GetRepository<Category>();
                categoriesRepository.Update(category);
                UnitOfWork.SaveChanges();
            }
            catch (Exception exception)
            {
                Logger.LogError(exception.Message, exception);
                throw;
            }
        }

        public void DeleteCategoryById(int id)
        {
            try
            {
                IRepository<Category> categoriesRepository = UnitOfWork.GetRepository<Category>();

                categoriesRepository.DeleteById(id);

                UnitOfWork.SaveChanges();
            }
            catch (Exception exception)
            {
                Logger.LogError(exception.Message, exception);
                throw;
            }
        }

        public void AddMany(IEnumerable<Category> categories)
        {
            try
            {
                IRepository<Category> categoriesRepository = UnitOfWork.GetRepository<Category>();

                categoriesRepository.AddMany(categories);

                UnitOfWork.SaveChanges();
            }
            catch(Exception exception)
            {
                Logger.LogError(exception.Message, exception);
                throw;
            }
        }

        public void DeleteMany(IEnumerable<Category> categories)
        {
            try
            {
                IRepository<Category> categoriesRepository = UnitOfWork.GetRepository<Category>();

                categoriesRepository.DeleteMany(categories);

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
