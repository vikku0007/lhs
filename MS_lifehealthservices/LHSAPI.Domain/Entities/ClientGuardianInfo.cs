using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LHSAPI.Domain.Entities
{
   public class ClientGuardianInfo:BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

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
