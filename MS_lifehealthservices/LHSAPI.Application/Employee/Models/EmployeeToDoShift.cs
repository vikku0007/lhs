using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LHSAPI.Application.Employee.Models
{
  public  class EmployeeToDoShift 
    { 
        public int Id { get; set; }
        public int EmployeeId { get; set; }

        public int ShiftId { get; set; }

        public DateTime? DateTime { get; set; }

    }
}
