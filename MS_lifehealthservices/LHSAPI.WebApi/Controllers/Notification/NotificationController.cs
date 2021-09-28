using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHSAPI.Application.Notification.Queries.GetAdminNotification;
using LHSAPI.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LHSAPI.Controllers.Notification
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : BaseController
    {
        [HttpPost]
        [Route("GetAdminNotification")]
        public async Task<IActionResult> GetAdminNotification([FromBody] GetAdminNotificationListQuery model)
        {
            //return Ok(await Mediator.Send(new GetAdminNotificationListQuery { }));
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetEmployeeNotification")]
        public async Task<IActionResult> GetEmployeeNotification(int EmployeeId)
        {

            return Ok(await Mediator.Send(new GetEmployeeNotificationListQuery { EmployeeId = EmployeeId }));
        }
        [HttpPost]
        [Route("UpdateAdminNotification")]
        public async Task<IActionResult> UpdateAdminNotification([FromBody] UpdateAdminNotificationCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
    }
}
