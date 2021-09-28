using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Shift.Commands.Create.AddDragDropShift
{
    public class AddDragDropShiftInfoCommand : IRequest<ApiResponse>
    {
        public int ShiftId { get; set; }
        public int EmployeeId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Duration { get; set; }
    }
}
