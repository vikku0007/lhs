using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHSAPI.Application.Administration.Commands.Create.AddLatLongLocation;
using LHSAPI.Application.Administration.Commands.Create.AddLocation;
using LHSAPI.Application.Administration.Commands.Create.AddMasterEntries;
using LHSAPI.Application.Administration.Commands.Create.AddPublicHoliday;
using LHSAPI.Application.Administration.Commands.Create.AddToDoListItem;
using LHSAPI.Application.Administration.Commands.Create.UploadServicePrice;
using LHSAPI.Application.Administration.Commands.Delete.DeleteLocation;
using LHSAPI.Application.Administration.Commands.Delete.DeleteMasterEntries;
using LHSAPI.Application.Administration.Commands.Delete.DeleteToDoItem;
using LHSAPI.Application.Administration.Commands.Delete.PublicHoliday;
using LHSAPI.Application.Administration.Commands.Update.EditLatLongLocation;
using LHSAPI.Application.Administration.Commands.Update.EditLocation;
using LHSAPI.Application.Administration.Commands.Update.EditMasterEntries;
using LHSAPI.Application.Administration.Commands.Update.UpdateGlobalPayRate;
using LHSAPI.Application.Administration.Commands.Update.UpdatePublicHoliday;
using LHSAPI.Application.Administration.Commands.Update.UpdateToDoListItem;
using LHSAPI.Application.Administration.Queries;
using LHSAPI.Application.Administration.Queries.GetAllLocationList;
using LHSAPI.Application.Administration.Queries.GetAllMasterEntries;
using LHSAPI.Application.Administration.Queries.GetAllPublicHoliday;
using LHSAPI.Application.Administration.Queries.GetLocationDetails;
using LHSAPI.Application.Administration.Queries.GetServiceRate;
using LHSAPI.Application.Administration.Queries.GetToDoItems;
using LHSAPI.Application.Administration.Queries.GetUserActivityLog;
using LHSAPI.Application.Employee.Commands.Update.UpdateMasterActiveInActive;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using LHSAPI.WebApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace LHSAPI.Controllers.Administration
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer", Policy = "RequireAdmin")]
    [ApiController]
    public class AdministrationController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly LHSDbContext _dbContext;
        // GET: api/Employee
        public AdministrationController(UserManager<ApplicationUser> userManager, LHSDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        [HttpPost]
        // [Authorize(Policy = "RequireAdmin")]
        [Route("AddLocation")]
        public async Task<IActionResult> AddLocation([FromBody] AddLocationCommand model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
         
        [Route("GetLocationList")]
        public async Task<IActionResult> GetLocationList([FromBody] GetLocationListQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
       
        [Route("GetLocationDetail")]
        public async Task<IActionResult> GetLocationDetail([FromBody]GetLocationDetailQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("EditLocationInfo")]
        public async Task<IActionResult> EditLocationInfo([FromBody] EditLocationCommand model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("DeleteLocationInfo")]
        public async Task<IActionResult> DeleteLocationInfo([FromBody] DeleteLocationCommand model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        [Route("AddMasterEntries")]
        public async Task<IActionResult> AddMasterEntries([FromBody] AddMasterEntriesCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("EditMasterEntries")]
        public async Task<IActionResult> EditMasterEntries([FromBody] EditMasterEntriesCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("DeleteMasterEntries")]
        public async Task<IActionResult> DeleteMasterEntries([FromBody] DeleteMasterEntriesCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("GetAllMasterEntries")]
        public async Task<IActionResult> GetAllMasterEntries([FromBody] GetAllMasterEntriesListQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("UpdateMasterActiveInActive")]
        public async Task<IActionResult> UpdateMasterActiveInActive([FromBody] UpdateMasterActiveInActiveCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("AddPublicHoliday")]
        public async Task<IActionResult> AddPublicHoliday([FromBody] AddPublicHolidayCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("DeletePublicHoliday")]
        public async Task<IActionResult> DeletePublicHoliday([FromBody] DeletePublicHolidayCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("UpdatePublicHoliday")]
        public async Task<IActionResult> UpdatePublicHoliday([FromBody] UpdatePublicHolidayCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("GetAllPublicHoliday")]
        public async Task<IActionResult> GetAllPublicHoliday([FromBody] GetAllPublicHolidayQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        [Route("AddToDoListItem")]
        public async Task<IActionResult> AddToDoListItem([FromBody] AddToDoListItemCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("DeleteToDoItem")]
        public async Task<IActionResult> DeleteToDoItem([FromBody] DeleteToDoItemCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("UpdateToDoListItem")]
        public async Task<IActionResult> UpdateToDoListItem([FromBody] UpdateToDoListItemCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        
       [HttpPost]
        [Route("GetUserAuditLog")]
        public async Task<IActionResult> GetUserAuditLog([FromBody] GetUserActivityLogQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("GetToDoItems")]
        public async Task<IActionResult> GetToDoItems([FromBody] GetToDoItemsQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("EditPayRateDetails")]
        public async Task<IActionResult> EditPayRateDetails([FromBody] UpdateGlobalPayRateCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        
       [HttpPost]
        [Route("UploadServicePrice")]
        public async Task<IActionResult> UploadServicePrice([FromForm] UploadServicePriceCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("GetServiceRate")]
        public async Task<IActionResult> GetServiceRate([FromBody] GetServiceRateQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("AddLatLongLocation")]
        public async Task<IActionResult> AddLatLongLocation([FromBody] AddLatLongLocationCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("EditLatLongLocation")]
        public async Task<IActionResult> EditLatLongLocation([FromBody] EditLatLongLocationCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
    }
}
