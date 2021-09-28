using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Commands.Create.AddClientPrimaryInfo
{
  public class AddClientPrimaryInfoCommand : IRequest<ApiResponse>
  {
    public int ClientId { get; set; }

    public int Salutation { get; set; }

    public string FirstName { get; set; }

    public string MiddleName { get; set; }

    public string LastName { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string MaritalStatus { get; set; }

    public string MobileNo { get; set; }

    public int Gender { get; set; }

    public string EmailId { get; set; }
    public string Address { get; set; }
    public int? LocationId { get; set; }

    public int EmployeeId { get; set; }
        public string OtherLocation { get; set; }

        public int? LocationType { get; set; }
        public bool Status { get; set; }

    }
}
