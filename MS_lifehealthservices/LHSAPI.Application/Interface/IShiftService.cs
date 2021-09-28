using LHSAPI.Application.Shift.Commands.Update.UpdateShiftInfo;
using LHSAPI.Application.Shift.Models;
using LHSAPI.Application.Shift.Queries.GetEmployeeViewCalendar;
using LHSAPI.Application.Shift.Queries.GetShiftPopOverInfo;
using LHSAPI.Common.ApiResponse;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LHSAPI.Application.Interface
{
  public interface IShiftService
  {
    ShiftInfoViewModel GetShiftDetail(int shiftId,bool IsShiftCompleted = false);
    IList<ShiftToDoViewModel> GetShiftToDoList(int shiftId);
    Task<ApiResponse> UpdateShiftStatus(int shiftId, int statusId);
    ApiResponse GetEmployeeViewCalendar(GetEmployeeViewCalendarQuery request, bool IsShiftCompleted = false);
    Task<ApiResponse> SaveShiftTemplate(string templateName, List<int> shiftList);
    ApiResponse GetShiftTemplateList();
    Task<ApiResponse> GetShiftTemplate(DateTime StartDate, DateTime EndDate, int templateId);
     Task<ApiResponse> UpdateShift(UpdateShiftInfoCommand request);
    ApiResponse GetShiftPopOverInfo(GetShiftPopOverInfoQuery request);
  }
}
