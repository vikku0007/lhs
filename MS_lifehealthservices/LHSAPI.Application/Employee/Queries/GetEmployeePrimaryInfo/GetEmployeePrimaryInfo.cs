
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Queries.GetEmployeePrimaryInfo
{
    public class GetEmployeePrimaryInfo : IRequest<ApiResponse>
    {
    public int Id { get; set; }
    

  }
}
