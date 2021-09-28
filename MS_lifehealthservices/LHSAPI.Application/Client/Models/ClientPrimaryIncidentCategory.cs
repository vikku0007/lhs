using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LHSAPI.Application.Client.Models
{
    public class ClientPrimaryIncidentCategory 
    {
        
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int PrimaryIncidentId { get; set; }
        public int? IncidentCategoryId { get; set; }
        public string PrimaryIncidentName { get; set; }
    }
}
