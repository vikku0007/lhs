using LHSAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;


namespace LHSAPI.Application.Client.Models
{
   public class AccidentIncidentmodel
    {
       
        public ClientAccidentProviderInfo ClientAccidentProviderInfo { get; set; }
        public ClientAccidentPrimaryContact ClientAccidentPrimaryContact { get; set; }
        public ClientIncidentCategory ClientIncidentCategory { get; set; }
        public ClientIncidentDetails ClientIncidentDetails { get; set; }
        public IncidentImmediateAction IncidentImmediateAction { get; set; }
        public IncidentRiskAssesment IncidentRiskAssesment { get; set; }
        public IncidentDeclaration IncidentDeclaration { get; set; }
        public List<ClientPrimaryIncidentCategory> ClientPrimaryIncidentCategory { get; set; }
        public List<ClientSecondaryIncidentCategory> ClientSecondaryIncidentCategory { get; set; }
        public IncidentImpactedPerson IncidentImpactedPerson { get; set; }
        public IncidentWorkerAllegation IncidentWorkerAllegation { get; set; }
        public IncidentDisablePersonAllegation IncidentDisablePersonAllegation { get; set; }
        public IncidentOtherAllegation IncidentOtherAllegation { get; set; }
        public List<IncidentDocumentDetailModel> IncidentDocumentDetailModel { get; set; }
        public List<IncidentPrimaryDisability> IncidentPrimaryDisability { get; set; }
        public List<IncidentOtherDisability> IncidentOtherDisability { get; set; }
        public List<IncidentConcernBehaviour> IncidentConcernBehaviour { get; set; }
        public List<ClientIncidentCommunication> ClientIncidentCommunication { get; set; }
        public List<IncidentAllegationPrimaryDisability> IncidentAllegationPrimaryDisability { get; set; }
        public List<IncidentAllegationOtherDisability> IncidentAllegationOtherDisability { get; set; }
        public List<IncidentAllegationBehaviour> IncidentAllegationBehaviour { get; set; }
        public List<IncidentAllegationCommunication> IncidentAllegationCommunication { get; set; }
    }

}
