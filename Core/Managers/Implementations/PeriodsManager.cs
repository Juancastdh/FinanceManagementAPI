using FinanceManagement.Core.Entities;
using FinanceManagement.Core.Repositories;
using FinanceManagement.Core.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceManagement.Core.Managers.Implementations
{
    public class PeriodsManager : IPeriodsManager
    {
        private readonly IUnitOfWork UnitOfWork;

        public PeriodsManager(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public void AddPeriod(Period period)
        {
            IRepository<Period> periodsRepository = UnitOfWork.GetRepository<Period>();

            periodsRepository.Add(period);

            UnitOfWork.SaveChanges();
        }

        public IEnumerable<Period> GetPeriods()
        {
            IRepository<Period> periodsRepository = UnitOfWork.GetRepository<Period>();

            IEnumerable<Period> periods = periodsRepository.GetAll();

            return periods;
        }
    }
}
