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
        public IActionResult GetAllFinancialTransactions()
        {
            IEnumerable<FinancialTransaction> financialTransactions = FinancialTransactionsManager.GetAllFinancialTransactions();

            IEnumerable<FinancialTransactionReadDto> financialTransactionReadDtos = Mapper.Map<IEnumerable<FinancialTransactionReadDto>>(financialTransactions);

            return Ok(financialTransactionReadDtos);
        }

        [HttpPost]
        public IActionResult CreateFinancialTransaction([FromBody] FinancialTransactionCreateDto financialTransaction)
        {
            FinancialTransaction financialTransactionToBeCreated = Mapper.Map<FinancialTransaction>(financialTransaction);

            FinancialTransactionsManager.AddFinancialTransaction(financialTransactionToBeCreated);

            FinancialTransactionReadDto financialTransactionReadDto = Mapper.Map<FinancialTransactionReadDto>(financialTransactionToBeCreated);

            return CreatedAtRoute("GetFinancialTransactionById", new {id = financialTransactionReadDto.Id}, financialTransactionReadDto);
        }

        [HttpPut]
        public IActionResult UpdateFinancialTransaction([FromBody] FinancialTransactionReadDto financialTransaction)
        {
            FinancialTransaction financialTransactionToBeUpdated = Mapper.Map<FinancialTransaction>(financialTransaction);

            FinancialTransactionsManager.UpdateFinancialTransaction(financialTransactionToBeUpdated);

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFinancialTransactionById(int id)
        {
            FinancialTransactionsManager.DeleteFinancialTransactionById(id);

            return Ok();           
        } 

        [HttpGet("FinancialReport")]    
        public IActionResult GetFinancialReport(int? periodId = null, int? categoryId = null, bool? isExpense = null)
        {
            FinancialReport financialReport = FinancialTransactionsManager.GetFinancialReport(periodId, categoryId, isExpense);

            FinancialReportReadDto financialReportReadDto = Mapper.Map<FinancialReportReadDto>(financialReport);

            return Ok(financialReportReadDto);

        }

        [HttpGet("{id}", Name = "GetFinancialTransactionById")]
        public IActionResult GetFinancialTransactionById(int id)
        {
            FinancialTransaction financialTransaction = FinancialTransactionsManager.GetFinancialTransactionById(id);

            FinancialTransactionReadDto financialTransactionReadDto = Mapper.Map<FinancialTransactionReadDto>(financialTransaction);

            return Ok(financialTransactionReadDto);

        }
    }
}
