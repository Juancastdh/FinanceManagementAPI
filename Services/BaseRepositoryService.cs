using FinanceManagement.Core.Repositories;
using FinanceManagement.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace FinanceManagement.Services
{
    public class BaseRepositoryService<T> : IRepository<T> where T : class
    {

        internal DatabaseContext Context;
        internal DbSet<T> DatabaseSet;

        public BaseRepositoryService(DatabaseContext context)
        {
            Context = context;
            DatabaseSet = context.Set<T>();
        }

        public void Add(T entity)
        {
            DatabaseSet.Add(entity);
        }

        public void DeleteById(int id)
        {
            T entityToDelete = DatabaseSet.Find(id);
            Delete(entityToDelete);
        }

        private void Delete(T entity)
        {
            if(Context.Entry(entity).State == EntityState.Detached)
            {
                DatabaseSet.Attach(entity);
            }

            DatabaseSet.Remove(entity);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = DatabaseSet;

            if(filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
       (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public T GetById(int id)
        {
            return DatabaseSet.Find(id);
        }

        public void Update(T entity)
        {
            DatabaseSet.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }
    }
}
