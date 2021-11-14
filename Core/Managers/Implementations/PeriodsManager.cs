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
        private readonly ILogger<PeriodsManager> Logger;

        public PeriodsManager(IUnitOfWork unitOfWork, ILogger<PeriodsManager> logger)
        {
            UnitOfWork = unitOfWork;
            Logger = logger;
        }

        public void AddPeriod(Period period)
        {

            try
            {
                IRepository<Period> periodsRepository = UnitOfWork.GetRepository<Period>();

                periodsRepository.Add(period);

                UnitOfWork.SaveChanges();
            }
            catch (Exception exception)
            {
                Logger.LogError(exception.Message, exception);
                throw;
            }

        }

        public void DeletePeriodById(int id)
        {
            try
            {
                IRepository<Period> periodsRepository = UnitOfWork.GetRepository<Period>();

                periodsRepository.DeleteById(id);

                UnitOfWork.SaveChanges();
            }
            catch (Exception exception)
            {
                Logger.LogError(exception.Message, exception);
                throw;
            }
        }

        public IEnumerable<Period> GetAllPeriods()
        {
            try
            {
                IRepository<Period> periodsRepository = UnitOfWork.GetRepository<Period>();

                IEnumerable<Period> periods = periodsRepository.GetAll();

                return periods;
            }
            catch (Exception exception)
            {
                Logger.LogError(exception.Message, exception);
                throw;
            }
        }

        public Period GetPeriodById(int id)
        {
            Period? period;

            try
            {
                IRepository<Period> periodsRepository = UnitOfWork.GetRepository<Period>();
                period = periodsRepository.GetById(id);
            }
            catch (Exception exception)
            {
                Logger.LogError(exception.Message, exception);
                throw;
            }

            if(period == null)
            {
                throw new DataNotFoundException();
            }

            return period;

        }

        public void UpdatePeriod(Period period)
        {
            try
            {
                IRepository<Period> periodsRepository = UnitOfWork.GetRepository<Period>();
                periodsRepository.Update(period);
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
