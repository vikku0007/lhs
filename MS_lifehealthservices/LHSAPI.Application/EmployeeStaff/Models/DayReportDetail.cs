using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.EmployeeStaff.Models
{
   public class DayReportDetail 
    {
       
        public int Id { get; set; }
        public string HouseAddress { get; set; }

        public DateTime? Date { get; set; }

        public int ShiftId { get; set; }
    }
}
