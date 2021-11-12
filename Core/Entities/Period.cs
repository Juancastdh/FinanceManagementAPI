using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceManagement.Core.Entities
{
    public class Period: BaseEntity
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
