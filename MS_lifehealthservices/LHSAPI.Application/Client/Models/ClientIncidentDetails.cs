using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LHSAPI.Application.Client.Models
{
   public class ClientIncidentDetails 
    {
        
        public int Id { get; set; }
        public int ClientId { get; set; }

        public int LocationId { get; set; }

        public int? LocationType { get; set; }

        public string OtherLocation { get; set; }

        public DateTime? DateTime { get; set; }

        public string UnknowndateReason { get; set; }

        public TimeSpan? NdisProviderTime { get; set; }

        public DateTime? NdisProviderDate { get; set; }

        public string IncidentAllegtion { get; set; }

        public string AllegtionCircumstances { get; set; }
        public string StartTimeString { get; set; }
        public string LocationTypeName { get; set; }
        public string LocationName { get; set; }
        public string Address { get; set; }
    }
}
