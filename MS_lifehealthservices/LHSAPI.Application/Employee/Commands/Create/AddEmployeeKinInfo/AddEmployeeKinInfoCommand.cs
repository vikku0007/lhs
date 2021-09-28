using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Commands.Create.AddEmployeeKinInfo
{
  public class AddEmployeeKinInfoCommand : IRequest<ApiResponse>
  {
    public int Id { get; set; }
    public int EmployeeId { get; set; }

    public string FirstName { get; set; }

    public string MiddelName { get; set; }

    public string LastName { get; set; }

    public int RelationShip { get; set; }

    public string ContactNo { get; set; }

    public string Email { get; set; }
    public string OtherRelation { get; set; }

    }
}
