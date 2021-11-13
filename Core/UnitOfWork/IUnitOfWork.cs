using FinanceManagement.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceManagement.Core.UnitOfWork
{
    public interface IUnitOfWork: IDisposable
    {

        public IRepository<T> GetRepository<T>() where T : class;
        void SaveChanges();
    }
}
