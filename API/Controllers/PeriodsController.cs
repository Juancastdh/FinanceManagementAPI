﻿using AutoMapper;
using FinanceManagement.API.DTOs.Periods;
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
            IEnumerable<Period> periods = PeriodsManager.GetAllPeriods();

            IEnumerable<PeriodReadDto> periodReadDtos = Mapper.Map < IEnumerable<PeriodReadDto>>(periods);

            return Ok(periodReadDtos);
        }

        [HttpPost]
        public IActionResult CreatePeriod([FromBody] PeriodCreateDto period)
        {
            Period periodToCreate = Mapper.Map<Period>(period);

            PeriodsManager.AddPeriod(periodToCreate);

            PeriodReadDto periodReadDto = Mapper.Map<PeriodReadDto>(periodToCreate);

            return CreatedAtRoute("GetPeriodById", new {id = periodReadDto.Id}, periodReadDto);
        }

        [HttpGet("{id}", Name = "GetPeriodById")]
        public IActionResult GetPeriodById(int id)
        {
            Period period = PeriodsManager.GetPeriodById(id);

            PeriodReadDto periodReadDto = Mapper.Map<PeriodReadDto>(period);

            return Ok(periodReadDto);

        }

        [HttpPut]
        public IActionResult UpdatePeriod([FromBody] PeriodReadDto period)
        {
            Period periodToBeUpdated = Mapper.Map<Period>(period);

            PeriodsManager.UpdatePeriod(periodToBeUpdated);

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePeriodById(int id)
        {
            PeriodsManager.DeletePeriodById(id);

            return Ok();
        }

        [HttpPost("Many")]
        public IActionResult CreatePeriods([FromBody] IEnumerable<PeriodReadDto> periods)
        {
            IEnumerable<Period> periodsToCreate = Mapper.Map<IEnumerable<Period>>(periods);

            PeriodsManager.AddPeriods(periodsToCreate);

            return Ok();
        }

        [HttpDelete("Many")]
        public IActionResult DeletePeriods([FromBody] IEnumerable<PeriodReadDto> periods)
        {
            IEnumerable<Period> periodsToDelete = Mapper.Map<IEnumerable<Period>>(periods);

            PeriodsManager.DeletePeriods(periodsToDelete);

            return Ok();
        }

    }
}
