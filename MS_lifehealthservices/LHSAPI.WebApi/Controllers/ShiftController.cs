using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHSAPI.Application.Shift.Commands.Create.AddAcceptShift;
using LHSAPI.Application.Shift.Commands.Create.AddCopypasteShift;
using LHSAPI.Application.Shift.Commands.Create.AddDragDropShift;
using LHSAPI.Application.Shift.Commands.Create.AddShiftInfo;
using LHSAPI.Application.Shift.Commands.Create.AddShiftTemplate;
using LHSAPI.Application.Shift.Commands.Create.AddShiftToDo;
using LHSAPI.Application.Shift.Commands.Delete.DeleteShiftInfo;
using LHSAPI.Application.Shift.Commands.Update.UpdateShiftInfo;
using LHSAPI.Application.Shift.Queries.GetAcceptedShifts;
using LHSAPI.Application.Shift.Queries.GetCustomHours;
using LHSAPI.Application.Shift.Queries.GetEmployeeViewCalendar;
using LHSAPI.Application.Shift.Queries.GetShiftHistoryDetails;
using LHSAPI.Application.Shift.Queries.GetShiftInfo;
using LHSAPI.Application.Shift.Queries.GetShiftList;
using LHSAPI.Application.Shift.Queries.GetShifToDoList;
using LHSAPI.Application.Shift.Queries.GetShiftPopOverInfo;
using LHSAPI.Application.Shift.Queries.GetShiftTemplate;
using LHSAPI.Application.Shift.Queries.GetShiftTemplateList;
using LHSAPI.Persistence.DbContext;
using LHSAPI.WebApi.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LHSAPI.Controllers
{
    [Route("api/[controller]")]
    // [Authorize(AuthenticationSchemes = "Bearer", Policy = "RequireAdmin")]
    [ApiController]
    public class ShiftController : BaseController
    {

        private readonly LHSDbContext _dbContext;
        public ShiftController(LHSDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpPost]
        [Route("GetAllShiftList")]
        public async Task<IActionResult> GetAllShiftList([FromBody] GetAllShiftListQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("AddShiftInfo")]
        public async Task<IActionResult> AddShiftInfo([FromBody] AddShiftInfoCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("UpdateShiftInfo")]
        public async Task<IActionResult> UpdateShiftInfo([FromBody] UpdateShiftInfoCommand model)
        {

            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetShiftInfo")]
        public async Task<IActionResult> GetShiftInfo(int Id)
        {
            return Ok(await Mediator.Send(new GetShiftInfoQuery { Id = Id }));
        }



        [HttpPost]
        [Route("GetAcceptedShiftsByEmpId")]
        public async Task<IActionResult> GetAcceptedShiftsByEmpId([FromBody] GetAcceptedShiftsInfoCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetShiftToDoList")]
        public async Task<IActionResult> GetShiftToDoList(int ShiftId)
        {
            return Ok(await Mediator.Send(new GetShifToDoListQuery { ShiftId = ShiftId }));
        }
        [HttpPost]
        [Route("AddShiftToDo")]
        public async Task<IActionResult> AddShiftToDo([FromBody] AddShiftToDoCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("GetEmployeeViewCalendar")]
        public async Task<IActionResult> GetEmployeeViewCalendar([FromBody] GetEmployeeViewCalendarQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        // DELETE: api/ApiWithActions/5
        [HttpPost]
        [Route("DeleteShiftInfo")]
        public async Task<IActionResult> DeleteShiftInfo([FromBody] DeleteShiftInfoCommand model)

        {

            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        [Route("AddCopyPasteShiftInfo")]
        public async Task<IActionResult> AddCopyPasteShiftInfo([FromBody] AddCopyPasteShiftInfoCommand model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        [Route("AddDragDropShiftInfo")]
        public async Task<IActionResult> AddDragDropShiftInfo([FromBody] AddDragDropShiftInfoCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("AddShiftTemplate")]
        public async Task<IActionResult> AddShiftTemplate([FromBody] AddShiftTemplateCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("LoadShiftTemplate")]
        public async Task<IActionResult> LoadShiftTemplate([FromBody] GetShiftTemplateQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetShiftTemplateList")]
        public async Task<IActionResult> GetShiftTemplateList()
        {
            return Ok(await Mediator.Send(new GetShiftTemplateListQuery { }));
        }
        [HttpPost]
        [Route("GetShiftHistoryDetails")]
        public async Task<IActionResult> GetShiftHistoryDetails([FromBody] GetShiftHistoryDetailsQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("GetShiftPopOverInfo")]
        public async Task<IActionResult> GetShiftPopOverInfo([FromBody] GetShiftPopOverInfoQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("GetCustomHours")]
        public async Task<IActionResult> GetCustomHours([FromBody] GetCustomHoursInfoCommand model)
        {
            return Ok(await Mediator.Send(model));
        }



    }


}

