
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Notification.Queries.GetAdminNotification
{
    public class GetEmployeeNotificationListQuery : IRequest<ApiResponse>
    {
    public int EmployeeId { get; set; }
   

    }
}
