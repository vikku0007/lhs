using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Commands.Create.AddPayRate
{
  public class AddPayRateCommand : IRequest<ApiResponse>
  {
    public int EmployeeId { get; set; }

    public int Level { get; set; }

    public double MonToFri6To12AM { get; set; }

    public double Sat6To12AM { get; set; }

    public double Sun6To12AM { get; set; }

    public double? Holiday6To12AM { get; set; }

    public double MonToFri12To6AM { get; set; }

    public double Sat12To6AM { get; set; }

    public double Sun12To6AM { get; set; }

    public double? Holiday12To6AM { get; set; }
    public double? ActiveNightsAndSleep { get; set; }
    public double? HouseCleaning { get; set; }
    public double? TransportPetrol { get; set; }


    }
}
