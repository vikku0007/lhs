using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.EmployeeStaff.Models
{
    public class ShiftHistory
    {
        public int Id { get; set; }
        public int ShiftId { get; set; }

        public int? EmployeeId { get; set; }

        public TimeSpan? CheckInTime { get; set; }
        public string CheckInTimestring { get; set; }

        public TimeSpan? CheckOutTime { get; set; }
        public string CheckOutTimestring { get; set; }

        public DateTime? CheckInDate { get; set; }

        public DateTime? CheckOutDate { get; set; }

        public string Description { get; set; }
        public int? ProgreeNotesId { get; set; }
        public int? ToDoItemId { get; set; }
        public double Duration { get; set; }
        public bool IsShiftCompleted { get; set; }
        public string CustomDuration { get; set; }
        public bool IsActiveNight { get; set; }
        public bool IsSleepover { get; set; }

    }
}
