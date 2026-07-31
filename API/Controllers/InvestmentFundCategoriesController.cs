using AutoMapper;
using FinanceManagement.API.DTOs;
using FinanceManagement.API.DTOs.InvestmentFundCategories;
using FinanceManagement.API.DTOs.InvestmentFunds;
using FinanceManagement.Core.Entities;
using FinanceManagement.Core.Managers;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvestmentFundCategoriesController : ControllerBase
    {
        private readonly IInvestmentFundCategoriesManager InvestmentFundCategoriesManager;
        private readonly IMapper Mapper;

        public InvestmentFundCategoriesController(IInvestmentFundCategoriesManager investmentFundCategoriesManager, IMapper mapper)
        {
            InvestmentFundCategoriesManager = investmentFundCategoriesManager;
            Mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<InvestmentFundCategoryReadDto>), 200)]
        public IActionResult GetAllInvestmentFundCategories(bool? deleted = null, int? investmentFundId = null)
        {
            IEnumerable<InvestmentFundCategory> investmentFundCategories = InvestmentFundCategoriesManager.GetAllInvestmentFundCategories(deleted, investmentFundId);

            IEnumerable<InvestmentFundCategoryReadDto> investmentFundCategoryReadDtos = Mapper.Map<IEnumerable<InvestmentFundCategoryReadDto>>(investmentFundCategories);

            return Ok(investmentFundCategoryReadDtos);
        }

        [HttpPost]
        [ProducesResponseType(typeof(InvestmentFundCategoryReadDto), 201)]
        public IActionResult CreateInvestmentFundCategory([FromBody] InvestmentFundCategoryCreateDto investmentFundCategory)
        {
            InvestmentFundCategory investmentFundCategoryToCreate = Mapper.Map<InvestmentFundCategory>(investmentFundCategory);

            InvestmentFundCategoriesManager.AddInvestmentFundCategory(investmentFundCategoryToCreate);

            InvestmentFundCategoryReadDto investmentFundCategoryReadDto = Mapper.Map<InvestmentFundCategoryReadDto>(investmentFundCategoryToCreate);

            return CreatedAtRoute("GetInvestmentFundCategoryById", new { id = investmentFundCategoryReadDto.Id }, investmentFundCategoryReadDto);
            
        }

        [HttpGet("{id}", Name = "GetInvestmentFundCategoryById")]
        [ProducesResponseType(typeof(InvestmentFundCategoryReadDto), 200)]
        public IActionResult GetInvestmentFundCategoryById(int id)
        {
            InvestmentFundCategory investmentFundCategory = InvestmentFundCategoriesManager.GetInvestmentFundCategoryById(id);

            InvestmentFundCategoryReadDto investmentFundCategoryReadDto = Mapper.Map<InvestmentFundCategoryReadDto>(investmentFundCategory);

            return Ok(investmentFundCategoryReadDto);
        }

        [HttpPut]
        [ProducesResponseType(200)]
        public IActionResult UpdateInvestmentFundCategory([FromBody] InvestmentFundCategoryReadDto investmentFundCategory   )
        {
            InvestmentFundCategory investmentFundCategoryToBeUpdated = Mapper.Map<InvestmentFundCategory>(investmentFundCategory);

            InvestmentFundCategoriesManager.UpdateInvestmentFundCategory(investmentFundCategoryToBeUpdated);

            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        public IActionResult DeleteInvestmentFundCategoryById(int id)
        {
            InvestmentFundCategoriesManager.DeleteInvestmentFundCategoryById(id);

            return Ok();
        }
    }
}
