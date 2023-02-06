using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interestrelar.Application.Contract;
using Microsoft.AspNetCore.Mvc;

namespace Interestrelar.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeralController : ControllerBase
    {

        private readonly IGeral _Geral;
        public GeralController(IGeral geral)
        {
            _Geral = geral;
        }

        [HttpGet("get-all/{skip}")]
        public async Task<IActionResult> GetAll(int skip)
        {
            var ret = await _Geral.GetAllCargoAsync(skip, 10);
            return StatusCode(ret.StatusCode, ret);
        }

        [HttpGet("get-all-exits/by-year-and-month/{skip}/{year}/{month}")]
        public async Task<IActionResult> GetAllExitsByYearAndMonth(int skip, int year, int month)
        {
            var ret = await _Geral.GetAllExitsByYearAndMonthCargoAsync(skip, 10, year, month);
            return StatusCode(ret.StatusCode, ret);
        }

        [HttpGet("get-all/by-year-and-month/{skip}/{year}/{month}")]
        public async Task<IActionResult> GetAllByYearAndMonth(int skip, int year, int month)
        {
            var ret = await _Geral.GetAllByYearAndMonthCargoAsync(skip, 10, year, month);
            return StatusCode(ret.StatusCode, ret);
        }

        [HttpGet("get-all/avaliable-year")]
        public async Task<IActionResult> GetAllAvaliableYear()
        {
            var ret = await _Geral.GetAllAvaliableYear();
            return StatusCode(ret.StatusCode, ret);
        }

        [HttpGet("get-all/avaliable-month/by-year/{year}")]
        public async Task<IActionResult> GetAllAvaliableMonthsByYear(int year)
        {
            var ret = await _Geral.GetAllAvaliableMonthsByYear(year);
            return StatusCode(ret.StatusCode, ret);
        }

    }
}