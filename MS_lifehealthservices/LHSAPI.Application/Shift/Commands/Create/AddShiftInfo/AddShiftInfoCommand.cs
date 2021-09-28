using LHSAPI.Application.Shift.Models;
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Shift.Commands.Create.AddShiftInfo
{
  public class AddShiftInfoCommand : IRequest<ApiResponse>
  {
    public string Description { get; set; }

    public int? LocationId { get; set; }

    public string OtherLocation { get; set; }

    public int StatusId { get; set; }

    public bool IsPublished { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string StartTime { get; set; }

    public string EndTime { get; set; }

    public double? Allowances { get; set; }

    public double? Mileage { get; set; }
    public int LocationType { get; set; }
    public List<EmployeeShiftInfoViewModel> EmployeeId { get; set; }
    public int[] ClientId { get; set; }
    public int[] ServiceTypeId { get; set; }
    public bool Reminder { get; set; }
    public string ShiftRepeat { get; set; }
    public int Days { get; set; }
  }
}
