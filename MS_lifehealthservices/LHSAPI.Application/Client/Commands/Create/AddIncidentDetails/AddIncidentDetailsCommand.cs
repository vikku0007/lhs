using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Commands.Create.AddIncidentDetails
{
  public class AddIncidentDetailsCommand : IRequest<ApiResponse>
  {

        public int Id { get; set; }
        public int ClientId { get; set; }

        public int LocationId { get; set; }

        public int? LocationType { get; set; }

        public string OtherLocation { get; set; }

        public DateTime DateTime { get; set; }

        public string UnknowndateReason { get; set; }

        public string NdisProviderTime { get; set; }

        public DateTime NdisProviderDate { get; set; }

        public string IncidentAllegtion { get; set; }

        public string AllegtionCircumstances { get; set; }
        public string Address { get; set; }
        public int? ShiftId { get; set; }
        public int? EmployeeId { get; set; }
    }
}
