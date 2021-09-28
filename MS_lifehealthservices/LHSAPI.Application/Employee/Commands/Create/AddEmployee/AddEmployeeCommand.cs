using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Commands.Create.AddEmployee
{
    public class AddEmployeeInfoCommand : IRequest<ApiResponse>
    {
        public int Saluation { get; set; }

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

        public int? EmployeeLevel { get; set; }
        public bool Status { get; set; }
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public int? Code { get; set; }

        public int EmpType { get; set; }
        public int Language { get; set; }
       

    }
}
