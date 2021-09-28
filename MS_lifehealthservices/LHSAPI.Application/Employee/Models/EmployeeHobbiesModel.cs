using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LHSAPI.Application.Employee.Models
{
  public class EmployeeHobbiesModel 
    {
        
        public int Id { get; set; }
        public int Hobbies { get; set; }

        public int EmployeeId { get; set; }
        public string HobbiesName { get; set; }
    }
}
