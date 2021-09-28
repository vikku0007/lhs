using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LHSAPI.Application.Employee.Models
{
  public  class EmployeeToDoListShift 
    {
        
        public int Id { get; set; }
        public int EmployeeId { get; set; }

        public string DescriptionId { get; set; }

        public bool IsInitials { get; set; }

        public int? Initials { get; set; }

        public bool IsReceived { get; set; }

        public int? Received { get; set; }
        public int ShiftId { get; set; }
        public DateTime? DateTime { get; set; }
    }
}
