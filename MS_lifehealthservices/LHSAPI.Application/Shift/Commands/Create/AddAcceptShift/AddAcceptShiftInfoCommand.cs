using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Shift.Commands.Create.AddAcceptShift
{
    public class AddAcceptShiftInfoCommand : IRequest<ApiResponse>
    {
        public int Id { get; set; }
        public int ShiftId { get; set; }
        public int EmployeeId { get; set; }
        public int ClientId { get; set; }
        public DateTime CheckInDate { get; set; }
        public string CheckInTime { get; set; }
        public string CheckInRemarks { get; set; }

      
        //public bool ToDoList_Flag { get; set; }
        //public bool AccidentIncident_Flag { get; set; }
        //public bool ProgressNotes_Flag { get; set; }
        
    }
}
