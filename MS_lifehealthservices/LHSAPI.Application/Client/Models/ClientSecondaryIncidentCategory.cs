using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LHSAPI.Application.Client.Models
{
   public class ClientSecondaryIncidentCategory 
    {
       
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int SecondaryIncidentId { get; set; }
        public int? IncidentCategoryId { get; set; }
        public string SecondaryIncidentName { get; set; }
    }
}
