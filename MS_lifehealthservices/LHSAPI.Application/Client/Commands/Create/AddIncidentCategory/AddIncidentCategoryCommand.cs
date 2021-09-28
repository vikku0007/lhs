using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Commands.Create.AddIncidentCategory
{
  public class AddIncidentCategoryCommand : IRequest<ApiResponse>
  {
        public int ClientId { get; set; }

        public int IsIncidentAnticipated { get; set; }

        public int[] PrimaryIncidentId { get; set; }

        public int[] SecondaryIncidentId { get; set; }
        public int? ShiftId { get; set; }
        public int? EmployeeId { get; set; }
    }
}
