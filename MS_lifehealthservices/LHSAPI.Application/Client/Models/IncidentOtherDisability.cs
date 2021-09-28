using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LHSAPI.Application.Client.Models
{
    public class IncidentOtherDisability 
    {
        
        public int Id { get; set; }
        public int ClientId { get; set; }

        public int ImpactPersonId { get; set; }

        public int OtherDisabilityId { get; set; }
        public string SecondaryDisabilityName { get; set; }

    }
}