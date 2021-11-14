using FinanceManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceManagement.Core.Managers
{
    public interface IPeriodsManager
    {
        void AddPeriod(Period period);
        IEnumerable<Period> GetAllPeriods();
        Period GetPeriodById(int id);
        void UpdatePeriod(Period period);
    }
}
