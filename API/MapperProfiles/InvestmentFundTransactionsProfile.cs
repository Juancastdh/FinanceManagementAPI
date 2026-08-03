using AutoMapper;
using FinanceManagement.API.DTOs;
using FinanceManagement.API.DTOs.InvestmentFundTransactions;
using FinanceManagement.Core.Entities;

namespace FinanceManagement.API.MapperProfiles
{
    public class InvestmentFundTransactionsProfile: Profile
    {
        public InvestmentFundTransactionsProfile()
        {
            CreateMap<InvestmentFundTransaction, InvestmentFundTransactionReadDto>();
            CreateMap<InvestmentFundTransactionCreateDto, InvestmentFundTransaction>();
            CreateMap<InvestmentFundTransactionReadDto, InvestmentFundTransaction>();
            CreateMap<InvestmentFundTransactionUpdateDto, InvestmentFundTransaction>();
        }        
    }
}
