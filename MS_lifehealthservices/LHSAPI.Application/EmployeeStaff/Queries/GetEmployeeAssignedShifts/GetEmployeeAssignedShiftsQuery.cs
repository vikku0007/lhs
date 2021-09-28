using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.EmployeeStaff.Queries.GetEmployeeAssignedShifts
{
    public class GetEmployeeAssignedShiftsQuery : IRequest<ApiResponse>
    {
        public int Id { get; set; }
        public int PageSize { get; set; }
        public int PageNo { get; set; }
        public LHSAPI.Common.Enums.Employee.EmployeeAccidentOrderBy OrderBy { get; set; }
        public LHSAPI.Common.Enums.SortOrder SortOrder { get; set; }
    }

    public class GetEmployeeCalendarShiftsQuery : IRequest<ApiResponse>
    {
        public int Id { get; set; }
        public DateTime FromDate { get; set; }        
        public DateTime ToDate { get; set; }
    }
}
