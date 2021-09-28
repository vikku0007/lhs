using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Models
{
   public class ClientSupportCoordinatorModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }

        public int Salutation { get; set; }

        public string Name { get; set; }

        public string MobileNo { get; set; }

        public int Gender { get; set; }

        public string EmailId { get; set; }
        public string PhoneNo { get; set; }
        public string GenderName { get; set; }
        public string SalutationName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string OtherRelation { get; set; }
        public string RelationShipName { get; set; }
        public int Relationship { get; set; }
    }
}
