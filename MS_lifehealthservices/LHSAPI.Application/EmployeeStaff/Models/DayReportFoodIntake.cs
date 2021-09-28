using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.EmployeeStaff.Models
{ 
  public class DayReportFoodIntake 
    {
       
        public int Id { get; set; }
      
        public string ResidentName { get; set; }

        public DateTime? Date { get; set; }
        public string StaffName { get; set; }

        public string Breakfast { get; set; }

        public string Lunch { get; set; }

        public string Dinner { get; set; }

        public string Snacks { get; set; }

        public string fluids { get; set; }

        public string Signature { get; set; }

        public int ShiftId { get; set; }
    }
}
