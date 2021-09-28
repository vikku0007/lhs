using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Shift.Models
{
  public class ShiftPopOverInfoViewModel
  {
    public List<EmployeeShiftInfoViewModel> EmployeeShiftInfoViewModel { get; set; }
    public List<ClientShiftInfoViewModel> ClientShiftInfoViewModel { get; set; }
    public List<ServiceTypeViewModel> ServiceTypeViewModel { get; set; }
  }
}
