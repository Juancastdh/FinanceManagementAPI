using AutoMapper;
using FinanceManagement.API.DTOs.Periods;
using FinanceManagement.Core.Entities;

namespace FinanceManagement.API.MapperProfiles
{
    public class PeriodsProfile: Profile
    {
        public PeriodsProfile()
        {
            CreateMap<Period, PeriodReadDto>();
            CreateMap<PeriodCreateDto, Period>();
            CreateMap<PeriodReadDto, Period>();
        }
    }
}
