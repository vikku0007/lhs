using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Models
{
    public class ClientGuardianModels
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public string Name { get; set; }

        public string RelationShip { get; set; }

        public string ContactNo { get; set; }

        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string RelationShipName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
    }
}
