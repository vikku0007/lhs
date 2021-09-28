using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Shift.Models
{
    public class EmployeeShiftInfoViewModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public bool IsSleepOver { get; set; }
        public bool IsHoliday { get; set; }
        public bool IsActiveNight { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
    }

    public class ShiftResponse
    {
        public string Description { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Action { get; set; }
        public string EmployeeName { get; set; }
  }
}
