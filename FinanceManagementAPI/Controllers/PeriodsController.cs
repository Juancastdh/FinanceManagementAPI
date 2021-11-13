using AutoMapper;
using FinanceManagement.API.DTOs.Period;
using FinanceManagement.Core.Entities;
using FinanceManagement.Core.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeriodsController : ControllerBase
    {

        private readonly IPeriodsManager PeriodsManager;
        private readonly IMapper Mapper;

        public PeriodsController(IPeriodsManager periodsManager, IMapper mapper)
        {
            PeriodsManager = periodsManager;
            Mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllPeriods()
        {
            IEnumerable<Period> periods = PeriodsManager.GetPeriods();

            IEnumerable<PeriodReadDto> periodReadDtos = Mapper.Map < IEnumerable<PeriodReadDto>>(periods);

            return Ok(periodReadDtos);
        }

        [HttpPost]
        public IActionResult CreatePeriod([FromBody] PeriodCreateDto period)
        {
            Period periodToCreate = Mapper.Map<Period>(period);

            PeriodsManager.AddPeriod(periodToCreate);

            return Ok();
        }

    }
}
