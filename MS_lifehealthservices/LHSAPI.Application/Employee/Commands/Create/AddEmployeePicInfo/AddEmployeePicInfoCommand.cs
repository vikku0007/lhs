using LHSAPI.Common.ApiResponse;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Commands.Create.AddEmployeePicInfo
{
  public class AddEmployeePicInfoCommand : IRequest<ApiResponse>
  {
    public string EmployeeId { get; set; }

    public IFormFile files { get; set; }
   }
}
