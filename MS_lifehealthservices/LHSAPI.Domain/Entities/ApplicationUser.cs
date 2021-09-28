
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LHSAPI.Domain.Entities
{
   
    public class ApplicationUser : IdentityUser
    {
        [StringLength(50), Required]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string MiddleName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        public bool IsDeleted { get; set; }
        public int EmployeeId { get; set; }
        public int ClientId { get; set; }
        public bool IsClient { get; set; }

    //public string RegistrationNumber { get; set; }
  }
}
