using LHSAPI.Application.Services;
using LHSAPI.Application.Shift.Models;
using LHSAPI.Application.Shift.Queries.GetEmployeeViewCalendar;
using LHSAPI.Common.ApiResponse;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace LHSAPI.Application.Interface
{
  public interface INotification
  {
    Task<ApiResponse> SaveNotification(LHSAPI.Domain.Entities.Notification notification, NotiFicationSaveMode mode);
    Task<ApiResponse> UpdateReadNotification(bool IsAdmin, int EmployeeId = 0);
    ApiResponse GetNotification(bool IsAdminAlert = true, int EmployeeId = 0);


  }
}
