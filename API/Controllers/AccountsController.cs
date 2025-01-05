using AutoMapper;
using FinanceManagement.API.DTOs.Accounts;
using FinanceManagement.Core.Entities;
using FinanceManagement.Core.Managers;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountsManager AccountsManager;
        private readonly IMapper Mapper;

        public AccountsController(IAccountsManager accountsManager, IMapper mapper)
        {
            AccountsManager = accountsManager;
            Mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AccountReadDto>), 200)]
        public IActionResult GetAllAccounts()
        {
            IEnumerable<Account> accounts = AccountsManager.GetAllAccounts();

            IEnumerable<AccountReadDto> accountReadDtos = Mapper.Map<IEnumerable<AccountReadDto>>(accounts);

            return Ok(accountReadDtos);
        }

        [HttpPost]
        [ProducesResponseType(typeof(AccountReadDto), 201)]
        public IActionResult CreateAccount([FromBody] AccountCreateDto account)
        {
            Account accountToCreate = Mapper.Map<Account>(account);

            AccountsManager.AddAccount(accountToCreate);

            AccountReadDto accountReadDto = Mapper.Map<AccountReadDto>(accountToCreate);

            return CreatedAtRoute("GetAccountById", new { id = accountReadDto.Id }, accountReadDto);
            
        }

        [HttpGet("{id}", Name = "GetAccountById")]
        [ProducesResponseType(typeof(AccountReadDto), 200)]
        public IActionResult GetAccountById(int id)
        {
            Account account = AccountsManager.GetAccountById(id);
            
            AccountReadDto accountReadDto = Mapper.Map<AccountReadDto>(account);

            return Ok(accountReadDto);

        }

        [HttpPut]
        [ProducesResponseType(200)]
        public IActionResult UpdateAccount([FromBody] AccountReadDto account)
        {
            Account accountToBeUpdated = Mapper.Map<Account>(account);

            AccountsManager.UpdateAccount(accountToBeUpdated);

            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        public IActionResult DeleteAccountById(int id)
        {
            AccountsManager.DeleteAccountById(id);

            return Ok();
        }
    }
}
