using FinanceManagement.Core.Entities;
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

        public IEnumerable<Period> GetPeriods()
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
    }
}
