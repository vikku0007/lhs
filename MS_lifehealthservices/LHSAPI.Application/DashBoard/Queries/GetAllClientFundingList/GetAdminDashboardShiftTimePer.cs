
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.DashBoard.Queries.GetClientFundingList
{
    public class GetAdminDashboardShiftTimePer : IRequest<ApiResponse>
    {
        
        public DateTime StartDate { get; set; }
       public DateTime EndDate { get; set; }
    }
}
