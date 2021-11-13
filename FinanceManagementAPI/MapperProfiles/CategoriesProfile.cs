using AutoMapper;
using FinanceManagement.API.DTOs;
using FinanceManagement.Core.Entities;

namespace FinanceManagement.API.MapperProfiles
{
    public class CategoriesProfile: Profile
    {
        public CategoriesProfile()
        {
            CreateMap<Category, CategoryReadDto>();
        }        
    }
}
