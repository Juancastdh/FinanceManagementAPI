using FinanceManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceManagement.Core.Managers
{
    public interface ICategoriesManager
    {
        IEnumerable<Category> GetAllCategories();
        void AddCategory(Category category);
    }
}
