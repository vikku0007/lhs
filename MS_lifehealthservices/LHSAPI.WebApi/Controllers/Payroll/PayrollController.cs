using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHSAPI.Application.PayRoll.Commands.Create.AddShiftCheckoutDetails;
using LHSAPI.Application.PayRoll.Queries.GetEmployesHours;
using LHSAPI.Application.PayRoll.Queries.GetIncompleteShift;
using LHSAPI.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LHSAPI.Controllers.Payroll
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayrollController : BaseController
    {
        [HttpGet]
        [Route("GetEmployeeHoursList")]
        public async Task<IActionResult> GetEmployeeHoursList(int employeeId, DateTime startDate, DateTime endDate, bool IsOnTime)
        {
            return Ok(await Mediator.Send(new GetEmployesHoursQuery { SearchByEmpName = employeeId, StartDate = startDate, EndDate = endDate, IsOnTime = IsOnTime }));
        }
        [HttpGet]
        [Route("GetEmployeeHoursDetail")]
        public async Task<IActionResult> GetEmployeeHoursDetail(int employeeId, DateTime startDate, DateTime endDate, bool IsOnTime)
        {
            return Ok(await Mediator.Send(new GetEmployeeHoursDetail { SearchByEmpName = employeeId, StartDate = startDate, EndDate = endDate, IsOnTime = IsOnTime }));
        }
        [HttpPost]
        [Route("ApproveRejectedShift")]
        public async Task<IActionResult> ApproveRejectedShift([FromBody] UpdateRejectedShift model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("InCompleteShiftDetail")]
        public async Task<IActionResult> InCompleteShiftDetail([FromBody] GetInCompleteShiftsInfoQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("AddShiftCheckoutDetails")]
        public async Task<IActionResult> AddShiftCheckoutDetails([FromBody] AddShiftCheckoutCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetEmployeeMyObHoursDetail")]
        public async Task<IActionResult> GetEmployeeMyObHoursDetail(int employeeId, DateTime startDate, DateTime endDate, bool IsOnTime)
        {
            return Ok(await Mediator.Send(new GetEmployeeMyObHoursDetail { SearchByEmpName = employeeId, StartDate = startDate, EndDate = endDate, IsOnTime = IsOnTime }));
        }
    }
}
