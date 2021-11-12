using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FinanceManagement.Core.Entities
{
    public class BaseEntity
    {
        [Required]
        public int Id { get; set; }
    }
}
