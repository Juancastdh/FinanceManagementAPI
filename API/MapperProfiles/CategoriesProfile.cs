using AutoMapper;
using FinanceManagement.API.DTOs;
using FinanceManagement.API.DTOs.Categories;
using FinanceManagement.Core.Entities;

namespace FinanceManagement.API.MapperProfiles
{
    public class CategoriesProfile: Profile
    {
        public CategoriesProfile()
        {
            CreateMap<Category, CategoryReadDto>();
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<CategoryReadDto, Category>();
        }        
    }
}
