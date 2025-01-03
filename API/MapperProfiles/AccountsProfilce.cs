using AutoMapper;
using FinanceManagement.API.DTOs.Accounts;
using FinanceManagement.Core.Entities;

namespace FinanceManagement.API.MapperProfiles
{
    public class AccountsProfile: Profile
    {
        public AccountsProfile()
        {
            CreateMap<Account, AccountReadDto>();
            CreateMap<AccountCreateDto, Account>();
            CreateMap<AccountReadDto, Account>();
        }        
    }
}
