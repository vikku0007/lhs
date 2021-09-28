using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Commands.Create.AddIncidentDeclaration
{
  public class AddIncidentDeclarationCommand : IRequest<ApiResponse>
  {

        public int EmployeeId { get; set; }
        public int ClientId { get; set; }
        public int? ShiftId { get; set; }
        public string Name { get; set; }

        public string PositionAtOrganisation { get; set; }
        public DateTime? Date { get; set; }
        public bool? IsDeclaration { get; set; }

    }
}


