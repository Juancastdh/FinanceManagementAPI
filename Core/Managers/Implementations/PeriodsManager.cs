using FinanceManagement.Core.Entities;
using FinanceManagement.Core.Exceptions;
using FinanceManagement.Core.Repositories;
using FinanceManagement.Core.UnitOfWork;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
            Period periodToDelete = periodsRepository.GetById(id);

            if (IsLatestPeriod(periodToDelete) == false)
            {
                throw new InvalidOperationException("Specified period is not the latest. Only the latest period can be deleted");
            }

            periodToDelete.Deleted = true;

            periodsRepository.Update(periodToDelete);

            UnitOfWork.SaveChanges();
        }

        public IEnumerable<Period> GetAllPeriods()
        {

            IRepository<Period> periodsRepository = UnitOfWork.GetRepository<Period>();

            IEnumerable<Period> periods = periodsRepository.GetAll(period => period.Deleted == false);

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

        private Period GetLatestPeriod()
        {
            IRepository<Period> periodsRepository = UnitOfWork.GetRepository<Period>();
            IEnumerable<Period> periods = periodsRepository.GetAll(period => period.Deleted == false);
            periods = periods.OrderByDescending(period => period.StartDate);
            Period latestPeriod = periods.First();

            return latestPeriod;
        }

        private bool IsLatestPeriod(Period period)
        {
            bool isLatestPeriod = false;

            Period latestPeriod = GetLatestPeriod();

            if (period.Id == latestPeriod.Id)
            {
                isLatestPeriod = true;
            }

            return isLatestPeriod;
        }
    }
}
