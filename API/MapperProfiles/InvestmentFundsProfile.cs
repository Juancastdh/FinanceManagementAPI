using AutoMapper;
using FinanceManagement.API.DTOs;
using FinanceManagement.API.DTOs.InvestmentFunds;
using FinanceManagement.Core.Entities;

namespace FinanceManagement.API.MapperProfiles
{
    public class InvestmentFundsProfile: Profile
    {
        public InvestmentFundsProfile()
        {
            CreateMap<InvestmentFund, InvestmentFundReadDto>();
            CreateMap<InvestmentFundCreateDto, InvestmentFund>();
            CreateMap<InvestmentFundReadDto, InvestmentFund>();
        }        
    }
}
