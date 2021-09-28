using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Commands.Create.AddClientGuardianInfo
{
  public class AddClientGuardianCommand : IRequest<ApiResponse>
  {

        public int ClientId { get; set; }

        public string FirstName { get; set; }

        public string RelationShip { get; set; }

        public string ContactNo { get; set; }

        public string Email { get; set; }

        public string PhoneNo { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

    }
}
