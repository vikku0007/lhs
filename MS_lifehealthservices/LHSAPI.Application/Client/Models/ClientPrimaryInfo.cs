using System;


namespace LHSAPI.Application.Client.Models
{
    public class ClientPrimaryInfo
    {

        public int Id { get; set; }
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

        public int EmployeeId { get; set; }

        public string FullName { get; set; }
        public int? LocationId { get; set; }

        public int? Age { get; set; }
        public string GenderName { get; set; }
        public string SalutationName { get; set; }
        public string LocationName { get; set; }

        public int ServiceType { get; set; }
        public string NDIS { get; set; }
        public string ServiceTypeName { get; set; }
        public string ImageUrl { get; set; }
        public int? LocationType { get; set; }
        public string LocationTypeName { get; set; }
        public string OtherLocation { get; set; }
    public string UserName { get; set; }

  }

}
