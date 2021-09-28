using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Shift.Models
{
  public class ShiftToDoViewModel
  {
    public int Id { get; set; }

    public int ShiftId { get; set; }

    public int EmployeeId { get; set; }

    public string Description { get; set; }
  }
}
