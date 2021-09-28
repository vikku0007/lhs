using LHSAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;


namespace LHSAPI.Application.Client.Models
{
   public class ClientntImpactedPerson
    {
        public IncidentImpactedPerson IncidentImpactedPerson { get; set; }
        public List<IncidentPrimaryDisability> IncidentPrimaryDisability { get; set; }
        public List<IncidentOtherDisability> IncidentOtherDisability { get; set; }
        public List<IncidentConcernBehaviour> IncidentConcernBehaviour { get; set; }
        public List<ClientIncidentCommunication> ClientIncidentCommunication { get; set; }
        
    }

}
