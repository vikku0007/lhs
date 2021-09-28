using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Domain.Entities
{
  public class ShiftToDo :BaseEntity
  {
    public int Id { get; set; }

    public int ShiftId { get; set; }

    public int EmployeeId { get; set; }

    public string Description { get; set; }
  }
}
