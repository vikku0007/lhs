using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Commands.Create.AddIncidentPrimaryContact
{
  public class AddIncidentPrimaryContactCommand : IRequest<ApiResponse>
  {

        public int EmployeeId { get; set; }
        public int ClientId { get; set; }
        public string Title { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string ProviderPosition { get; set; }

        public string PhoneNo { get; set; }

        public string Email { get; set; }

        public string ContactMetod { get; set; }
        public int? ShiftId { get; set; }
       
    }
}
