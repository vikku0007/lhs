using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Shift.Queries.GetEmployeeShiftList
{
    public class GetEmployeeShiftListQuery : IRequest<ApiResponse>
    {
        public int EmployeeId { get; set; }
        public int SearchByEmpName { get; set; }
        public int SearchByClientName { get; set; }

        public int SearchTextBylocation { get; set; }
        public int SearchTextByStatus { get; set; }
        public int SearchTextByShiftType { get; set; }
        public string SearchTextByManualAddress { get; set; }
        public string SearchByStartDate { get; set; }
        public string SearchByEndDate { get; set; }
        public int PageSize { get; set; }
        public int PageNo { get; set; }
        public LHSAPI.Common.Enums.Employee.EmployeeShiftOrderBy OrderBy { get; set; }
        public LHSAPI.Common.Enums.SortOrder SortOrder { get; set; }
    }
}
