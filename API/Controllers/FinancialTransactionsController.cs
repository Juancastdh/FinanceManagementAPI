using AutoMapper;
using FinanceManagement.API.DTOs.FinancialTransactions;
using FinanceManagement.Core.Entities;
using FinanceManagement.Core.Managers;
using FinanceManagement.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinancialTransactionsController : ControllerBase
    {
        private readonly IFinancialTransactionsManager FinancialTransactionsManager;
        private readonly IMapper Mapper;

        public FinancialTransactionsController(IFinancialTransactionsManager financialTransactionsManager, IMapper mapper)
        {
            FinancialTransactionsManager = financialTransactionsManager;
            Mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FinancialTransactionReadDto>), 200)]
        public IActionResult GetAllFinancialTransactions()
        {
            IEnumerable<FinancialTransaction> financialTransactions = FinancialTransactionsManager.GetAllFinancialTransactions();

            IEnumerable<FinancialTransactionReadDto> financialTransactionReadDtos = Mapper.Map<IEnumerable<FinancialTransactionReadDto>>(financialTransactions);

            return Ok(financialTransactionReadDtos);
        }

        [HttpPost]
        [ProducesResponseType(typeof(FinancialTransactionReadDto), 201)]
        public IActionResult CreateFinancialTransaction([FromBody] FinancialTransactionCreateDto financialTransaction)
        {
            FinancialTransaction financialTransactionToBeCreated = Mapper.Map<FinancialTransaction>(financialTransaction);

            FinancialTransactionsManager.AddFinancialTransaction(financialTransactionToBeCreated);

            FinancialTransactionReadDto financialTransactionReadDto = Mapper.Map<FinancialTransactionReadDto>(financialTransactionToBeCreated);

            return CreatedAtRoute("GetFinancialTransactionById", new {id = financialTransactionReadDto.Id}, financialTransactionReadDto);
        }

        [HttpPut]
        [ProducesResponseType(200)]
        public IActionResult UpdateFinancialTransaction([FromBody] FinancialTransactionReadDto financialTransaction)
        {
            FinancialTransaction financialTransactionToBeUpdated = Mapper.Map<FinancialTransaction>(financialTransaction);

            FinancialTransactionsManager.UpdateFinancialTransaction(financialTransactionToBeUpdated);

            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        public IActionResult DeleteFinancialTransactionById(int id)
        {
            FinancialTransactionsManager.DeleteFinancialTransactionById(id);

            return Ok();           
        } 

        [HttpGet("FinancialReport")]
        [ProducesResponseType(typeof(FinancialReport), 200)]
        public IActionResult GetFinancialReport(int? periodId = null, int? categoryId = null, bool? isExpense = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            FinancialReport financialReport = FinancialTransactionsManager.GetFinancialReport(periodId, categoryId, isExpense, startDate, endDate);

            FinancialReportReadDto financialReportReadDto = Mapper.Map<FinancialReportReadDto>(financialReport);

            return Ok(financialReportReadDto);

        }

        [HttpGet("{id}", Name = "GetFinancialTransactionById")]
        [ProducesResponseType(typeof(FinancialTransactionReadDto), 200)]
        public IActionResult GetFinancialTransactionById(int id)
        {
            FinancialTransaction financialTransaction = FinancialTransactionsManager.GetFinancialTransactionById(id);

            FinancialTransactionReadDto financialTransactionReadDto = Mapper.Map<FinancialTransactionReadDto>(financialTransaction);

            return Ok(financialTransactionReadDto);

        }

        [HttpPost]
        [Route("Many/Json")]
        [ProducesResponseType(typeof(IEnumerable<FinancialTransactionReadDto>), 200)]
        public IActionResult CreateFinancialTransactions([FromBody] IEnumerable<FinancialTransactionCreateDto> financialTransactions)
        {
            IEnumerable<FinancialTransaction> financialTransactionsToBeCreated = Mapper.Map<IEnumerable<FinancialTransaction>>(financialTransactions);
            
            IEnumerable<FinancialTransactionReadDto> createdFinancialTransactions = Mapper.Map<IEnumerable<FinancialTransactionReadDto>>(financialTransactionsToBeCreated);

            return Ok(createdFinancialTransactions);
            //FinancialTransactionsManager.AddFinancialTransactions(financialTransactionsToBeCreated);

            //return Ok();
        }

        [HttpPost]
        [Route("Many/Xml")]
        [ProducesResponseType(typeof(IEnumerable<FinancialTransactionReadDto>), 200)]
        [Consumes("application/xml")]
        [Produces("application/xml")]
        public IActionResult CreateFinancialTransactionsXml([FromBody] FinancialTransactionsXmlCreateDto financialTransactionsXml)
        {
            IEnumerable<FinancialTransaction> financialTransactionsToBeCreated = Mapper.Map<IEnumerable<FinancialTransaction>>(financialTransactions.Transactions);

            IEnumerable<FinancialTransactionReadDto> createdFinancialTransactions = Mapper.Map<IEnumerable<FinancialTransactionReadDto>>(financialTransactionsToBeCreated);

            return Ok(createdFinancialTransactions);
            //FinancialTransactionsManager.AddFinancialTransactions(financialTransactionsToBeCreated);

            //return Ok();
        }

    }
}
