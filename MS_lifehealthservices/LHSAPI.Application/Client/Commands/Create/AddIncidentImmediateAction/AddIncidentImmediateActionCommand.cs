using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Commands.Create.AddIncidentImmediateAction
{
  public class AddIncidentImmediateActionCommand : IRequest<ApiResponse>
  {

        public int EmployeeId { get; set; }
        public int ClientId { get; set; }
        public bool IsPoliceInformed { get; set; }

        public string OfficerName { get; set; }

        public string PoliceStation { get; set; }

        public string PoliceNo { get; set; }

        public string ProviderPosition { get; set; }

        public string PhoneNo { get; set; }

        public int IsFamilyAware { get; set; }

        public string ContacttoFamily { get; set; }

        public int IsUnder18 { get; set; }

        public string ContactChildProtection { get; set; }

        public string DisabilityPerson { get; set; }

        public string SubjectWorkerAllegation { get; set; }

        public string SubjectDisabilityPerson { get; set; }
        public int? ShiftId { get; set; }
        
    }
}


