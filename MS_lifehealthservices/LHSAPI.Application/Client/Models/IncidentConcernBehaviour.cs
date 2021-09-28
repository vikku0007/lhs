using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LHSAPI.Application.Client.Models
{
   public class IncidentConcernBehaviour 
    {
        
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int ImpactPersonId { get; set; }

        public int ConcerBehaviourId { get; set; }
        public string ConcernBehaviourName { get; set; }
    }
}
