﻿using AutoMapper;
using FinanceManagement.API.DTOs.FinancialTransactions;
using FinanceManagement.Core.Entities;

namespace FinanceManagement.API.MapperProfiles
{
    public class FinancialTransactionsProfile: Profile
    {

        public FinancialTransactionsProfile()
        {
            CreateMap<FinancialTransaction, FinancialTransactionReadDto>();
            CreateMap<FinancialTransactionCreateDto, FinancialTransaction>();
        }

    }
}