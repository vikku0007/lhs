using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.PayRoll.Queries.GetIncompleteShift
{
    public class GetInCompleteShiftsInfoQuery : IRequest<ApiResponse>
    {
        public int SearchByEmpName { get; set; }
        public int SearchByClientName { get; set; }

        public int SearchTextBylocation { get; set; }
        public int SearchTextByStatus { get; set; }
        public int SearchTextByShiftType { get; set; }
        public string SearchTextByManualAddress { get; set; }
        public int PageSize { get; set; }

        public int PageNo { get; set; }
        public string SearchByStartDate { get; set; }
        public string SearchByEndDate { get; set; }
    }
}
