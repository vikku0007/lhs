using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LHSAPI.Application.Client.Models
{
   public class IncidentOtherAllegation 
    {
        
        public int Id { get; set; }
        public int ClientId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Title { get; set; }

        public string Relationship { get; set; }

        public int? Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string PhoneNo { get; set; }

        public string Email { get; set; }
        public string OtherFullName { get; set; }
        public string GenderName { get; set; }
    }
}
