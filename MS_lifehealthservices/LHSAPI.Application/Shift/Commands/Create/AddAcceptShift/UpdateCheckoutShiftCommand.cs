using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Shift.Commands.Create.AddAcceptShift
{
    public class UpdateCheckoutShiftCommand : IRequest<ApiResponse>
    {
        public int ShiftId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string CheckOutTime { get; set; }
        public double TotalDuration { get; set; }
        public double ActiveNightDuration { get; set; }
        public double DayDuration { get; set; }
        public string CheckOutRemarks { get; set; }
        public bool IsCheckoutByWeb { get; set; }
        public bool IsCheckoutByApp { get; set; }

        public bool ToDoList_Flag { get; set; }
        public bool AccidentIncident_Flag { get; set; }
        public bool ProgressNotes_Flag { get; set; }
    }
}
