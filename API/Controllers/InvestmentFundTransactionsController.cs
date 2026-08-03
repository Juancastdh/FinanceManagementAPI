using AutoMapper;
using FinanceManagement.API.DTOs;
using FinanceManagement.API.DTOs.InvestmentFundTransactions;
using FinanceManagement.API.DTOs.InvestmentFunds;
using FinanceManagement.Core.Entities;
using FinanceManagement.Core.Managers;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvestmentFundTransactionsController : ControllerBase
    {
        private readonly IInvestmentFundTransactionsManager InvestmentFundTransactionsManager;
        private readonly IMapper Mapper;

        public InvestmentFundTransactionsController(IInvestmentFundTransactionsManager investmentFundTransactionsManager, IMapper mapper)
        {
            InvestmentFundTransactionsManager = investmentFundTransactionsManager;
            Mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<InvestmentFundTransactionReadDto>), 200)]
        public IActionResult GetAllInvestmentFundTransactions(int? investmentFundCategoryId = null, int? investmentFundId = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            IEnumerable<InvestmentFundTransaction> investmentFundTransactions = InvestmentFundTransactionsManager.GetAllInvestmentFundTransactions(investmentFundCategoryId, investmentFundId, startDate, endDate);

            IEnumerable<InvestmentFundTransactionReadDto> investmentFundTransactionReadDtos = Mapper.Map<IEnumerable<InvestmentFundTransactionReadDto>>(investmentFundTransactions);

            return Ok(investmentFundTransactionReadDtos);
        }

        [HttpPost]
        [ProducesResponseType(typeof(InvestmentFundTransactionReadDto), 201)]
        public IActionResult CreateInvestmentFundTransaction([FromBody] InvestmentFundTransactionCreateDto investmentFundTransaction)
        {
            InvestmentFundTransaction investmentFundTransactionToCreate = Mapper.Map<InvestmentFundTransaction>(investmentFundTransaction);

            InvestmentFundTransactionsManager.AddInvestmentFundTransaction(investmentFundTransactionToCreate);

            InvestmentFundTransactionReadDto investmentFundTransactionReadDto = Mapper.Map<InvestmentFundTransactionReadDto>(investmentFundTransactionToCreate);

            return CreatedAtRoute("GetInvestmentFundTransactionById", new { id = investmentFundTransactionReadDto.Id }, investmentFundTransactionReadDto);
            
        }

        [HttpGet("{id}", Name = "GetInvestmentFundTransactionById")]
        [ProducesResponseType(typeof(InvestmentFundTransactionReadDto), 200)]
        public IActionResult GetInvestmentFundTransactionById(int id)
        {
            InvestmentFundTransaction investmentFundTransaction = InvestmentFundTransactionsManager.GetInvestmentFundTransactionById(id);

            InvestmentFundTransactionReadDto investmentFundTransactionReadDto = Mapper.Map<InvestmentFundTransactionReadDto>(investmentFundTransaction);

            return Ok(investmentFundTransactionReadDto);
        }

        [HttpPut]
        [ProducesResponseType(200)]
        public IActionResult UpdateInvestmentFundTransaction([FromBody] InvestmentFundTransactionUpdateDto investmentFundTransaction)
        {
            InvestmentFundTransaction investmentFundTransactionToBeUpdated = Mapper.Map<InvestmentFundTransaction>(investmentFundTransaction);

            InvestmentFundTransactionsManager.UpdateInvestmentFundTransaction(investmentFundTransactionToBeUpdated);

            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        public IActionResult DeleteInvestmentFundTransactionById(int id)
        {
            InvestmentFundTransactionsManager.DeleteInvestmentFundTransactionById(id);

            return Ok();
        }
    }
}
