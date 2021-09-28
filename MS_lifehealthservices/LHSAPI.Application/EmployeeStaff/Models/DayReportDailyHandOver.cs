using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.EmployeeStaff.Models
{
   public class DayReportDailyHandOver 
    {
        
        public int Id { get; set; }
       
        public string Description { get; set; }

        public bool? IsMorningWorker { get; set; }

        public string MorningWorkerSignature { get; set; }

        public bool? IsAfterNoonWorker { get; set; }

        public string AfterNoonWorkerSign { get; set; }

        public bool? IsSleepOverWorker { get; set; }

        public string SleepOverWorkerSign { get; set; }

        public int ShiftId { get; set; }
    }
}
