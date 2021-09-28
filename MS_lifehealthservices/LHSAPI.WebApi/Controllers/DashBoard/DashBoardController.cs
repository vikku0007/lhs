using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHSAPI.Application.DashBoard.Queries.GetClientFundingList;
using LHSAPI.Application.DashBoard.Queries.GetShiftDetailsAdminDashboard;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using LHSAPI.WebApi.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace LHSAPI.Controllers.DashBoard
{
    [Route("api/[controller]")]
   
    //[Authorize(AuthenticationSchemes = "Bearer", Policy = "RequireAdminOrAccountOfficer")]
    [ApiController]
    public class DashBoardController : BaseController
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly LHSDbContext _dbContext;
        public DashBoardController(UserManager<ApplicationUser> userManager, LHSDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }


        [HttpPost]
        [Route("GetClientFundingListInfo")]
        public async Task<IActionResult> GetClientFundingListInfo([FromBody] GetClientFundingListInfoQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetShiftHours")]
        public async Task<IActionResult> GetShiftHours(DateTime StartDate, DateTime EndDate)
        {
            return Ok(await Mediator.Send(new GetAdminDashboardHours { StartDate = StartDate, EndDate = EndDate }));
        }
        [HttpGet]
        [Route("GetAdminDashboardShiftTimeStatus")]
        public async Task<IActionResult> GetAdminDashboardShiftTimeStatus(DateTime StartDate, DateTime EndDate)
        {
            return Ok(await Mediator.Send(new GetAdminDashboardShiftTimePer { StartDate = StartDate, EndDate = EndDate }));
        }
        [HttpGet]
        [Route("GetSchedulesShiftAdminDashboard")]
        public async Task<IActionResult> GetSchedulesShiftAdminDashboard()
        {
            return Ok(await Mediator.Send(new GetSchedulesShiftAdminDashboardCommand { }));
        }
        [HttpGet]
        [Route("GetCompleteShiftAdminDashboard")]
        public async Task<IActionResult> GetCompleteShiftAdminDashboard()
        {
            return Ok(await Mediator.Send(new GetCompleteShiftAdminDashboardCommand { }));
        }
        [HttpGet]
        [Route("GetOnShiftShiftAdminDashboard")]
        public async Task<IActionResult> GetOnShiftShiftAdminDashboard()
        {
            return Ok(await Mediator.Send(new GetOnShiftShiftAdminDashboardCommand { }));
        }


    }
}
