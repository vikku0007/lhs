using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LHSAPI.Application.Client.Models
{
   public class IncidentAllegationBehaviour
    {
       
        public int Id { get; set; }
        public int ClientId { get; set; }

        public int DisableAllegationId { get; set; }

        //public int ConcerBehaviourId { get; set; }
        public int CodeId { get; set; }

        public string OtherBehaviour { get; set; }
        //public string ConcernBehaviourName { get; set; }
        public string CodeName { get; set; }
    }
}
