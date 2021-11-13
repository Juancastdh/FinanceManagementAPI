using AutoMapper;
using FinanceManagement.API.DTOs.FinancialTransactions;
using FinanceManagement.Core.Entities;
using FinanceManagement.Core.Models;

namespace FinanceManagement.API.MapperProfiles
{
    public class FinancialTransactionsProfile: Profile
    {

        public FinancialTransactionsProfile()
        {
            CreateMap<FinancialTransaction, FinancialTransactionReadDto>();
            CreateMap<FinancialTransactionCreateDto, FinancialTransaction>();
            CreateMap<FinancialTransactionReadDto, FinancialTransaction>();
            CreateMap<FinancialReport, FinancialReportReadDto>();
        }

    }
}
