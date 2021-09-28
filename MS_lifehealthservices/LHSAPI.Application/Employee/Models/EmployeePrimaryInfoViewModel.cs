using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Models
{
    public class EmployeePrimaryInfoViewModel
    {
        public int Id { get; set; }
        public int? Saluation { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public int? Role { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public int? MaritalStatus { get; set; }
        public string MaritalStatusName { get; set; }

        public string MobileNo { get; set; }

        public int? Gender { get; set; }

        public string EmailId { get; set; }

        public int EmployeeId { get; set; }

        public int? EmployeeLevel { get; set; }

        public bool Status { get; set; }

        public int? LocationId { get; set; }

        public string Address1 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public int? Code { get; set; }
        public string RoleName { get; set; }

        public int? Language { get; set; }
        public int? EmpType { get; set; }

        public string ImageUrl { get; set; }

        public string GenderName { get; set; }
        public string SalutationName { get; set; }
        public int Age { get; set; }
        public Nullable<DateTime> DateOfJoining { get; set; }
        public string FullName { get; set; }

        public bool HasVisa { get; set; }
        public string PassportNumber { get; set; }
        public string VisaNumber { get; set; }
        public int? VisaType { get; set; }
        public DateTime? VisaExpiryDate { get; set; }

        public string VisaTypeName { get; set; }
        public string StateName { get; set; }

        public string CountryName { get; set; }

        public Nullable<DateTime> CreatedDate { get; set; }
        public int? Religion { get; set; }
        public string OtherHobbies { get; set; }
        public string OtherReligion { get; set; }
        public string OtherLanguage { get; set; }
        public bool? IsAustralian { get; set; }
        public string HobbiesName { get; set; }
        public int PasswordExist { get; set; }
        public List<EmployeeHobbiesModel> EmployeeHobbiesModel { get; set; }
    }
  
}
