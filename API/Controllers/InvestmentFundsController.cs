using AutoMapper;
using FinanceManagement.API.DTOs;
using FinanceManagement.API.DTOs.InvestmentFunds;
using FinanceManagement.Core.Entities;
using FinanceManagement.Core.Managers;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvestmentFundsController : ControllerBase
    {
        private readonly IInvestmentFundsManager InvestmentFundsManager;
        private readonly IMapper Mapper;

        public InvestmentFundsController(IInvestmentFundsManager investmentFundsManager, IMapper mapper)
        {
            InvestmentFundsManager = investmentFundsManager;
            Mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<InvestmentFundReadDto>), 200)]
        public IActionResult GetAllInvestmentFunds(bool? deleted = null)
        {
            IEnumerable<InvestmentFund> investmentFunds = InvestmentFundsManager.GetAllInvestmentFunds(deleted);

            IEnumerable<InvestmentFundReadDto> investmentFundReadDtos = Mapper.Map<IEnumerable<InvestmentFundReadDto>>(investmentFunds);

            return Ok(investmentFundReadDtos);
        }

        [HttpPost]
        [ProducesResponseType(typeof(InvestmentFundReadDto), 201)]
        public IActionResult CreateInvestmentFund([FromBody] InvestmentFundCreateDto investmentFund)
        {
            InvestmentFund investmentFundToCreate = Mapper.Map<InvestmentFund>(investmentFund);

            InvestmentFundsManager.AddInvestmentFund(investmentFundToCreate);

            InvestmentFundReadDto investmentFundReadDto = Mapper.Map<InvestmentFundReadDto>(investmentFundToCreate);

            return CreatedAtRoute("GetInvestmentFundById", new { id = investmentFundReadDto.Id }, investmentFundReadDto);
            
        }

        [HttpGet("{id}", Name = "GetInvestmentFundById")]
        [ProducesResponseType(typeof(InvestmentFundReadDto), 200)]
        public IActionResult GetInvestmentFundById(int id)
        {
            InvestmentFund investmentFund = InvestmentFundsManager.GetInvestmentFundById(id);

            InvestmentFundReadDto investmentFundReadDto = Mapper.Map<InvestmentFundReadDto>(investmentFund);

            return Ok(investmentFundReadDto);
        }

        [HttpPut]
        [ProducesResponseType(200)]
        public IActionResult UpdateInvestmentFund([FromBody] InvestmentFundReadDto investmentFund)
        {
            InvestmentFund investmentFundToBeUpdated = Mapper.Map<InvestmentFund>(investmentFund);

            InvestmentFundsManager.UpdateInvestmentFund(investmentFundToBeUpdated);

            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        public IActionResult DeleteInvestmentFundById(int id)
        {
            InvestmentFundsManager.DeleteInvestmentFundById(id);

            return Ok();
        }
    }
}
