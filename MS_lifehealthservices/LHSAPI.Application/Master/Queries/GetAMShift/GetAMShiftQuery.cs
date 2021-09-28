
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Master.Queries.GetAMShift
{
    public class GetAMShiftQuery : IRequest<ApiResponse>
    {
       
        public int ShiftType { get; set; }

        public int EmployeeId { get; set; }
    }
}
