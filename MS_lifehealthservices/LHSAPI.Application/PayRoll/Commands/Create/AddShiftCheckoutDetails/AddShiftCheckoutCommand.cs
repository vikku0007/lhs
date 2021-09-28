using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.PayRoll.Commands.Create.AddShiftCheckoutDetails
{
    public class AddShiftCheckoutCommand : IRequest<ApiResponse>
    {
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string CheckInTime { get; set; }
        public string CheckOutTime { get; set; }
        public int ShiftId { get; set; }
        public int EmployeeId { get; set; }
        public string Remark { get; set; }
    }
}
