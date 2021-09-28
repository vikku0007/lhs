using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Shift.Models
{
    public class ShiftInfoViewModel
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public int ClientCount { get; set; }

        public int EmployeeCount { get; set; }

        public int? LocationId { get; set; }
        public string LocationName { get; set; }

        public string OtherLocation { get; set; }

        public int? StatusId { get; set; }
        public string StatusName { get; set; }

        public bool IsPublished { get; set; }
        public bool Reminder { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }
        public string StartTimeString { get; set; }

        public string EndTimeString { get; set; }

        public double? Allowances { get; set; }

        public double? Mileage { get; set; }
        public double Duration { get; set; }
        public double? Expense { get; set; }

        public bool IsDeleted { get; set; }
        public int LocationType { get; set; }
        public string Name { get; set; }
        public int EmployeeId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ShiftRepeatType { get; set; }
        public bool IsSleepOver { get; set; }
        public bool IsActiveNight { get; set; }
        public bool IsShiftCompleted { get; set; }
        public bool IsLogin { get; set; }
        public string Remark { get; set; }
        public string CustomDuration { get; set; }
        public string AdminCheckoutRemark { get; set; }
        public string CheckoutRemark { get; set; }
        public List<EmployeeShiftInfoViewModel> EmployeeShiftInfoViewModel { get; set; }
        public List<ClientShiftInfoViewModel> ClientShiftInfoViewModel { get; set; }
        public List<ServiceTypeViewModel> ServiceTypeViewModel { get; set; }
    }
    public class ShiftViewLoadTemplate
    {
        public List<ShiftInfoViewModel> ShiftInfoViewModel { get; set; }
        public List<ShiftResponse> ShiftResponseList { get; set; }
    }
}
