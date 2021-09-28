using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Commands.Create.Update.EditEmployee
{
    public class EditEmployeeCommand : IRequest<ApiResponse>
    {
        public int Id { get; set; }
        public int Salutation { get; set; }

        public string Firstname { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public int Role { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public int MaritalStatus { get; set; }

        public string MobileNo { get; set; }

        public int Gender { get; set; }

        public string EmailId { get; set; }

        public int EmployeeId { get; set; }

        public int EmployeeLevel { get; set; }

        public DateTime? DateOfJoining { get; set; }

        public bool Status { get; set; }

        public int? LocationId { get; set; }

        public string Address1 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public int? Code { get; set; }

        public int EmpType { get; set; }

        public int Language { get; set; }
        public bool HasVisa { get; set; }
        public bool IsAustralian { get; set; }
        public string PassportNumber { get; set; }
        public string VisaNumber { get; set; }
        public string OtherHobbies { get; set; }
        public string OtherReligion { get; set; }
        public string OtherLanguage { get; set; }
        public int? VisaType { get; set; }
        public DateTime? VisaExpiryDate { get; set; }
        public int? Religion { get; set; }
        public int[] Hobbies { get; set; }

    }
}
