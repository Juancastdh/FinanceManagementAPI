using AutoMapper;
using FinanceManagement.API.DTOs;
using FinanceManagement.API.DTOs.InvestmentFundCategories;
using FinanceManagement.Core.Entities;

namespace FinanceManagement.API.MapperProfiles
{
    public class InvestmentFundCategoriesProfile: Profile
    {
        public InvestmentFundCategoriesProfile()
        {
            CreateMap<InvestmentFundCategory, InvestmentFundCategoryReadDto>();
            CreateMap<InvestmentFundCategoryCreateDto, InvestmentFundCategory>();
            CreateMap<InvestmentFundCategoryReadDto, InvestmentFundCategory>();
        }        
    }
}
