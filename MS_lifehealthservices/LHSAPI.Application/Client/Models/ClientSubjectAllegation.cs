using LHSAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;


namespace LHSAPI.Application.Client.Models
{
   public class ClientSubjectAllegation
    {
        
        public IncidentWorkerAllegation IncidentWorkerAllegation { get; set; }
        public IncidentDisablePersonAllegation IncidentDisablePersonAllegation { get; set; }
        public IncidentOtherAllegation IncidentOtherAllegation { get; set; }
        public List<IncidentAllegationPrimaryDisability> IncidentAllegationPrimaryDisability { get; set; }
        public List<IncidentAllegationOtherDisability> IncidentAllegationOtherDisability { get; set; }
        public List<IncidentAllegationBehaviour> IncidentAllegationBehaviour { get; set; }
        public List<IncidentAllegationCommunication> IncidentAllegationCommunication { get; set; }
    }

}
