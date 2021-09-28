using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Models
{
    public class ShiftInfoViewModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Description { get; set; }
        public string ClientName { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string StartTimeString { get; set; }
        public string EndTimeString { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsRejected { get; set; }
        public bool IsShiftCompleted { get; set; }
        public string EmployeeName { get; set; }  
        public string StatusName { get; set; }
    }
}
