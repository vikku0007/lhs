using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Queries.GetClientCurrentShifts
{
    public class GetClientCurrentShiftsQuery : IRequest<ApiResponse>
    {
        public int Id { get; set; }
    }

    public class GetClientAssignedShiftsQuery : IRequest<ApiResponse>
    {
        public int Id { get; set; }

        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }

    public class UpdateClientShiftCancelQuery : IRequest<ApiResponse>
    {
        public int Id { get; set; }
        public string Remark { get; set; }
        public int ClientId { get; set; }

    }

    public class GetClientCalendarShifts : IRequest<ApiResponse>
    {
        public int Id { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
