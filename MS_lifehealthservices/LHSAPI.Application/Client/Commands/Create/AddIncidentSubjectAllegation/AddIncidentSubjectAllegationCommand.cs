using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Commands.Create.AddInciidentSubjectAllegation
{
  public class AddIncidentSubjectAllegationCommand : IRequest<ApiResponse>
  {
        public int ClientId { get; set; }

        public string FirstName { get; set; }

        public string Title { get; set; }

        public string LastName { get; set; }
        public string Position { get; set; }

        public string NdisParticipantNo { get; set; }

        public int? GenderId { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string PhoneNo { get; set; }

        public string Email { get; set; }

        public int[] CommunicationId { get; set; }

        public int[] ConcerBehaviourId { get; set; }
        public int DisableAllegationId { get; set; }

        public int[] PrimaryDisability { get; set; }
        public int[] OtherDisability { get; set; }
        public string DisableFirstName { get; set; }

        public string DisableLastName { get; set; }

        public string DisableTitle { get; set; }

        public string DisableNdisNumber { get; set; }

        public int? DisableGender { get; set; }

        public DateTime DisableDateOfBirth { get; set; }

        public string DisablePhoneNo { get; set; }

        public string DisableEmail { get; set; }
        public string OtherFirstName { get; set; }

        public string OtherLastName { get; set; }

        public string OtherTitle { get; set; }

        public string OtherRelationship { get; set; }

        public int? OtherGender { get; set; }

        public DateTime OtherDateOfBirth { get; set; }

        public string OtherPhoneNo { get; set; }

        public string OtherEmail { get; set; }
        public string OtherDetail { get; set; }
        public int IsSubjectAllegation { get; set; }
        public int? ShiftId { get; set; }
        public int? EmployeeId { get; set; }
    }
}
