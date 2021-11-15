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

        public void DeletePeriodById(int id)
        {
            IRepository<Period> periodsRepository = UnitOfWork.GetRepository<Period>();

            periodsRepository.DeleteById(id);

            UnitOfWork.SaveChanges();
        }

        public IEnumerable<Period> GetAllPeriods()
        {

            IRepository<Period> periodsRepository = UnitOfWork.GetRepository<Period>();

            IEnumerable<Period> periods = periodsRepository.GetAll();

            return periods;

        }

        public Period GetPeriodById(int id)
        {
            Period? period;


            IRepository<Period> periodsRepository = UnitOfWork.GetRepository<Period>();
            period = periodsRepository.GetById(id);


            if (period == null)
            {
                throw new DataNotFoundException();
            }

            return period;

        }

        public void UpdatePeriod(Period period)
        {

            IRepository<Period> periodsRepository = UnitOfWork.GetRepository<Period>();
            periodsRepository.Update(period);
            UnitOfWork.SaveChanges();

        }
    }
}
