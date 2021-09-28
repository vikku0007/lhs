
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Queries.GetAllEmployeeAvailableList
{
    public class GetAllEmployeeAvailableListQuery : IRequest<ApiResponse>
    {
      
        public int PageSize { get; set; }

        public int PageNo { get; set; }

    }
}
