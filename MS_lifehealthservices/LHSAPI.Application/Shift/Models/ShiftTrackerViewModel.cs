using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Shift.Models
{
    public class ShiftTrackerViewModel
    {
        public int Id { get; set; }
        public int ShiftId { get; set; }
        public int EmployeeId { get; set; }
        public int ClientId { get; set; }
        public Nullable<DateTime> CheckInDate { get; set; }
        public TimeSpan CheckInTime { get; set; }
        public string CheckInRemarks { get; set; }

        public DateTime CheckOutDate { get; set; }
        public TimeSpan CheckOutTime { get; set; }
        public double Duration { get; set; }
        public bool ToDoList_Flag { get; set; }
        public bool AccidentIncident_Flag { get; set; }
        public bool ProgressNotes_Flag { get; set; }
        public string CheckOutRemarks { get; set; }
        public string StartTimeString { get; set; }
        public string EndTimeString { get; set; }
    }
}
