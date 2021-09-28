using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Shift.Commands.Create.AddCopypasteShift
{
    public class AddCopyPasteShiftInfoCommand : IRequest<ApiResponse>
    {
        public int ShiftId { get; set; }
        public int EmployeeId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }        
    }
}
