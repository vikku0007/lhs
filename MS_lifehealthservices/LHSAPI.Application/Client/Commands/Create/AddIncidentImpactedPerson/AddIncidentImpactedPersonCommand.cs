using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Commands.Create.AddIncidentImpactedPerson
{
  public class AddIncidentImpactedPersonCommand : IRequest<ApiResponse>
  {
        public int ClientId { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string NdisParticipantNo { get; set; }

        public int? GenderId { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string PhoneNo { get; set; }
        public string Title { get; set; }

        public string Email { get; set; }
        public string OtherDetail { get; set; }

        public int[] CommunicationId { get; set; }

        public int[] ConcerBehaviourId { get; set; }
        public int ImpactPersonId { get; set; }

        public int[] PrimaryDisability { get; set; }
        public int[] OtherDisability { get; set; }
        public int? ShiftId { get; set; }
        public int? EmployeeId { get; set; }

    }
}
