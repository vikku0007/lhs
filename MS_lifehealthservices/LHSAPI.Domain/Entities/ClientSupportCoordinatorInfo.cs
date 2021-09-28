using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LHSAPI.Domain.Entities
{
   public class ClientSupportCoordinatorInfo:BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ClientId { get; set; }

        public int Salutation { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string OtherRelation { get; set; }
        public int Relationship { get; set; }
        public string MobileNo { get; set; }
        public string PhoneNo { get; set; }

        public int Gender { get; set; }
        
        public string EmailId { get; set; }
    }
}
