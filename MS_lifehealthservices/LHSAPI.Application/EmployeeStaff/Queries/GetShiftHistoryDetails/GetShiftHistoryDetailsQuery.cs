using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Shift.Queries.GetShiftHistoryDetails
{
    public class GetShiftHistoryDetailsQuery : IRequest<ApiResponse>
    {

        public int ShiftId { get; set; }

    }
}
