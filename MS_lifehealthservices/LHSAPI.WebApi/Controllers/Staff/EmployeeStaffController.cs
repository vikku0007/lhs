using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHSAPI.Application.EmployeeStaff.Commands.Create.AddDailyFoodIntake;
using LHSAPI.Application.EmployeeStaff.Commands.Create.AddDayReportAppointment;
using LHSAPI.Application.EmployeeStaff.Commands.Create.AddDayReportCashHandOver;
using LHSAPI.Application.EmployeeStaff.Commands.Create.AddDayReportDailyHandOver;
using LHSAPI.Application.EmployeeStaff.Commands.Create.AddDayReportSupportWorker;
using LHSAPI.Application.EmployeeStaff.Commands.Create.AddDayReportTelephoneMsg;
using LHSAPI.Application.EmployeeStaff.Commands.Create.AddDayReportVisitor;
using LHSAPI.Application.EmployeeStaff.Commands.Create.AddToDoList;
using LHSAPI.Application.EmployeeStaff.Commands.Update;
using LHSAPI.Application.EmployeeStaff.Commands.Update.UpdateDailyFoodIntake;
using LHSAPI.Application.EmployeeStaff.Commands.Update.UpdateDayReportAppointment;
using LHSAPI.Application.EmployeeStaff.Commands.Update.UpdateDayReportCashHandOver;
using LHSAPI.Application.EmployeeStaff.Commands.Update.UpdateDayReportSupportWorker;
using LHSAPI.Application.EmployeeStaff.Commands.Update.UpdateDayReportTelephoneMsg;
using LHSAPI.Application.EmployeeStaff.Commands.Update.UpdateDayReportVisitor;
using LHSAPI.Application.EmployeeStaff.Commands.Update.UpdateLeaveApproveStatus;
using LHSAPI.Application.EmployeeStaff.Commands.Update.UpdateLeaveRejectStatus;
using LHSAPI.Application.EmployeeStaff.Commands.Update.UpdateShiftRejectStatus;
using LHSAPI.Application.EmployeeStaff.Queries.GetCheckOutClient;
using LHSAPI.Application.EmployeeStaff.Queries.GetDayReportDetails;
using LHSAPI.Application.EmployeeStaff.Queries.GetEmployeeApplyLeave;
using LHSAPI.Application.EmployeeStaff.Queries.GetEmployeeAssignedShifts;
using LHSAPI.Application.EmployeeStaff.Queries.GetEmployeeCurrentShifts;
using LHSAPI.Application.EmployeeStaff.Queries.GetEmployeeDashBoardInfoMobile;
using LHSAPI.Application.EmployeeStaff.Queries.GetEmployeeHours;
using LHSAPI.Application.Shift.Commands.Create.AddAcceptShift;
using LHSAPI.Application.Shift.Queries.GetEmployeeCurrentLocation;
using LHSAPI.Application.Shift.Queries.GetEmployeeShiftList;
using LHSAPI.Application.Shift.Queries.GetEmployeeToDoList;
using LHSAPI.WebApi.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LHSAPI.Controllers.Staff
{
    [Route("api/[controller]")]
    //[Authorize(Policy = "RequireAdmin")]
    //[Authorize(Policy = "RequireEmployee")]
    [ApiController]
    public class EmployeeStaffController : BaseController
    {
        public EmployeeStaffController()
        {
        }

        [HttpPost]
        [Route("GetAssignedShifts")]
        public async Task<IActionResult> GetAssignedShifts([FromBody] GetEmployeeAssignedShiftsQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        [Route("UpdateAssignedAcceptStatus")]
        public async Task<IActionResult> UpdateAssignedAcceptStatus([FromBody] UpdateShiftAcceptStatusCommand model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        [Route("UpdateAssignedRejectStatus")]
        public async Task<IActionResult> UpdateAssignedRejectStatus([FromBody] UpdateShiftRejectStatusCommand model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        [Route("GetCurrentShifts")]
        public async Task<IActionResult> GetCurrentShifts([FromBody] GetEmployeeCurrentShiftsQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        [Route("SaveCurrentShiftInfo")]
        public async Task<IActionResult> SaveCurrentShiftInfo([FromBody] AddAcceptShiftInfoCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("UpdatCurrentShiftInfo")]
        public async Task<IActionResult> UpdatCurrentShiftInfo([FromBody] UpdateCheckoutShiftCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("GetCheckOutClientList")]
        public async Task<IActionResult> GetCheckOutClientList([FromBody] GetCheckOutClientQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("GetEmployeeShiftList")]
        public async Task<IActionResult> GetEmployeeShiftList([FromBody] GetEmployeeShiftListQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("AddToListItem")]
        public async Task<IActionResult> AddToListItem([FromBody] AddToDoListCommand model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        [Route("GetEmployeeCalendarShifts")]
        public async Task<IActionResult> GetEmployeeCalendarShifts([FromBody] GetEmployeeCalendarShiftsQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetEmployeeShiftHours")]
        public async Task<IActionResult> GetEmployeeShiftHours(int EmployeeId)
        {
            return Ok(await Mediator.Send(new GetEmployeeHoursCommand { EmployeeId = EmployeeId }));
        }
        [HttpGet]
        [Route("GetEmployeeDashBoardDetail")]
        public async Task<IActionResult> GetEmployeeDashBoardDetail(int EmployeeId)
        {
            return Ok(await Mediator.Send(new GetEmployeeDashBoardDetail { EmployeeId = EmployeeId }));
        }


        [HttpPost]
        [Route("GetEmployeeApplyLeave")]
        public async Task<IActionResult> GetEmployeeApplyLeave([FromBody] GetEmployeeApplyLeaveQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("UpdateLeaveApproveStatus")]
        public async Task<IActionResult> UpdateLeaveApproveStatus([FromBody] UpdateLeaveApproveStatusCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("UpdateLeaveRejectStatus")]
        public async Task<IActionResult> UpdateLeaveRejectStatus([FromBody] UpdateLeaveRejectStatusCommand model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        [Route("GetEmployeeToDoList")]
        public async Task<IActionResult> GetEmployeeToDoList([FromBody] GetEmployeeNewToDoListQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        [Route("GetEmployeeEditToDoList")]
        public async Task<IActionResult> GetEmployeeEditToDoList([FromBody] GetEmployeeEditToDoListQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        [Route("UpdateToListItem")]
        public async Task<IActionResult> UpdateToListItem([FromBody] UpdateToDoListCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("AddDayReportDailyFoodIntake")]
        public async Task<IActionResult> AddDayReportDailyFoodIntake([FromBody] AddDailyFoodIntakeCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("AddDayReportAppointment")]
        public async Task<IActionResult> AddDayReportAppointment([FromBody] AddDayReportAppointmentCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("AddDayReportCashHandOver")]
        public async Task<IActionResult> AddDayReportCashHandOver([FromBody] AddCashHandOverCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("AddDayReportDailyHandOver")]
        public async Task<IActionResult> AddDayReportDailyHandOver([FromBody] AddDailyHandOverCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("AddDayReportSupportWorker")]
        public async Task<IActionResult> AddDayReportSupportWorker([FromBody] AddSupportWorkerCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("AddDayReportTelephoneMsg")]
        public async Task<IActionResult> AddDayReportTelephoneMsg([FromBody] AddTelephoneMsgCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("AddDayReportVisitor")]
        public async Task<IActionResult> AddDayReportVisitor([FromBody] AddDayReportVisitorCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("UpdateDayReportDailyFoodIntake")]
        public async Task<IActionResult> UpdateDailyFoodIntake([FromBody] UpdateDailyFoodIntakeCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("UpdateDayReportAppointment")]
        public async Task<IActionResult> UpdateDayReportAppointment([FromBody] UpdateDayReportAppointmentCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("UpdateDayReportCashHandOver")]
        public async Task<IActionResult> UpdateDayReportCashHandOver([FromBody] UpdateCashHandOverCommand model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        [Route("UpdateDayReportSupportWorker")]
        public async Task<IActionResult> UpdateDayReportSupportWorker([FromBody] UpdateSupportWorkerCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("UpdateDayReportTelephoneMsg")]
        public async Task<IActionResult> UpdateDayReportTelephoneMsg([FromBody] UpdateTelephoneMsgCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("UpdateDayReportVisitor")]
        public async Task<IActionResult> UpdateDayReportVisitor([FromBody] UpdateDayReportVisitorCommand model)
        {
            return Ok(await Mediator.Send(model));
        } 
        [HttpPost]
        [Route("GetDayReportDetails")]
        public async Task<IActionResult> GetDayReportDetails([FromBody] GetDayReportDetailsQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        [Route("GetEmployeeCurrentLocation")]
        public async Task<IActionResult> GetEmployeeCurrentLocation([FromBody] GetEmployeeCurrentLocationQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

    }

}

