
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Notification.Queries.GetAdminNotification
{
    public class UpdateAdminNotificationCommand : IRequest<ApiResponse>
    {
        public bool IsAdmin { get; set; }
        public int EmployeeId { get; set; }
       
    }
}
