﻿using FinanceManagement.Core.Entities;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace FinanceManagement.API.DTOs.FinancialTransactions
{
    public class FinancialTransactionsXmlCreateDto
    {
        public string Xml { get; set; }
    }
}
