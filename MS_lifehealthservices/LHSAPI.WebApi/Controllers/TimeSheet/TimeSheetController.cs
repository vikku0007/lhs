using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHSAPI.Application.TimeSheet.Queries.GetEmployeeHourReport;
using LHSAPI.Application.TimeSheet.Queries.GetEmployeeTimeSheet;
using LHSAPI.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LHSAPI.Controllers.TimeSheet
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeSheetController : BaseController
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", DateTime.Now.ToShortDateString() };
        }
        [HttpGet]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("GetEmployeeTimeSheet")]
        public async Task<IActionResult> GetEmployeeTimeSheet(DateTime StartDate, DateTime EndDate, int Id = 0)
        {
            return Ok(await Mediator.Send(new GetEmployeeTimeSheet { EmployeeId = Id, StartDate = StartDate, EndDate = EndDate }));
        }

        [HttpGet]
        [Route("GetEmployeeHourlyReport")]
        public async Task<IActionResult> GetEmployeeHourlyReport(DateTime StartDate, DateTime EndDate, int EmployeeId = 0)
        {
            return Ok(await Mediator.Send(new GetEmployeeHourReport { SearchByEmpId = EmployeeId, StartDate = StartDate, EndDate = EndDate }));
        }

    }
}
