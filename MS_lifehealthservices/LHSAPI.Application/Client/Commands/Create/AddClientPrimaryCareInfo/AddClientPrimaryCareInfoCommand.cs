using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Commands.Create.AddClientPrimaryCareInfo
{
  public class AddClientPrimaryCareInfoCommand : IRequest<ApiResponse>
  {
        public int Id { get; set; }
        public int ClientId { get; set; }

        public string FirstName { get; set; }

        public int? RelationShip { get; set; }

        public string ContactNo { get; set; }

        public string Email { get; set; }

        public string PhoneNo { get; set; }
        public string MiddleName { get; set; }
        public string  LastName { get; set; }
        public string OtherRelation { get; set; }
        public int? Gender { get; set; }
    }
}
