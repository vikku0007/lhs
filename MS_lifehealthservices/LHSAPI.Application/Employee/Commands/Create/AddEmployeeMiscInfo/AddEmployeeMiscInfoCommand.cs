using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Commands.Create.AddEmployeeMiscInfo
{
  public class AddEmployeeMiscInfoCommand : IRequest<ApiResponse>
  {
    public int EmployeeId { get; set; }

    public double? Weight { get; set; }

    public double? Height { get; set; }

    public int? Ethnicity { get; set; }

    public int? Religion { get; set; }

    public bool Smoker { get; set; }

   


  }
}
