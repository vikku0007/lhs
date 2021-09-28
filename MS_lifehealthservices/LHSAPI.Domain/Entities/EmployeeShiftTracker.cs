using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LHSAPI.Domain.Entities
{
    public class EmployeeShiftTracker : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ShiftId { get; set; }
        public int EmployeeId { get; set; }
        public int ClientId { get; set; }
        public Nullable<DateTime> CheckInDate { get; set; }
        public Nullable<TimeSpan> CheckInTime { get; set; }
        public string CheckInRemarks { get; set; }

        public Nullable<DateTime> CheckOutDate { get; set; }
        public Nullable<TimeSpan> CheckOutTime { get; set; }
        public double TotalDuration { get; set; }
        public double ActiveNightDuration { get; set; }
        public double SleepOverDuration { get; set; }
        public double DayDuration { get; set; }
        public bool ToDoList_Flag { get; set; }
        public bool AccidentIncident_Flag { get; set; }
        public bool ProgressNotes_Flag { get; set; }
        public string CheckOutRemarks { get; set; }
        public bool IsShiftCompleted { get; set; }
        public bool IsApproveByAccounts { get; set; }
        public bool IsShiftOnTime { get; set; }
        public bool IsLogin { get; set; }
        public string AdminCheckOutRemark { get; set; }
        public bool IsCheckoutByWeb { get; set; }
        public bool IsCheckoutByApp { get; set; }


    }
}
