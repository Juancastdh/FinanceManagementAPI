using FinanceManagement.Core.Repositories;
using FinanceManagement.Core.UnitOfWork;
using FinanceManagement.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Services.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private DatabaseContext Context;

        private bool disposed = false;

        public UnitOfWork(DatabaseContext context)
        {
            Context = context;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            return new BaseRepositoryService<T>(Context);
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }
    }
}
