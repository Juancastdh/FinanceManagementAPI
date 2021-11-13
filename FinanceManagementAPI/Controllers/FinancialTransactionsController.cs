using AutoMapper;
using FinanceManagement.API.DTOs.FinancialTransactions;
using FinanceManagement.Core.Entities;
using FinanceManagement.Core.Managers;
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
    }
}
