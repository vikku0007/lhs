using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LHSAPI.Application.EmployeeStaff.Models
{
   public class DayReportAppointments 
    {
       
        public int Id { get; set; }
       
        public TimeSpan? Time { get; set; }

        public int ClientId { get; set; }

        public string Details { get; set; }

        public string GeneralInformation { get; set; }

        public string NightReport { get; set; }
        public string TimeString { get; set; }
        public string ClientName { get; set; }

        public int ShiftId { get; set; }

    }
}
