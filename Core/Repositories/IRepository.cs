using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FinanceManagement.Core.Repositories
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = "");
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void DeleteById(int id);
        void AddMany(IEnumerable<T> entities);
        void DeleteMany(IEnumerable<T> entities);
    }
}
