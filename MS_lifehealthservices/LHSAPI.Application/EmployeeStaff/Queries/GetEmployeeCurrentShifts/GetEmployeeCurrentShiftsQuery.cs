using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.EmployeeStaff.Queries.GetEmployeeCurrentShifts
{
    public class GetEmployeeCurrentShiftsQuery: IRequest<ApiResponse>   
    {
        public int Id { get; set; }
    }
}
