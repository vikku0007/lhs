using LHSAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;


namespace LHSAPI.Application.Client.Models
{
   public class ClientDetails
    {
        public ClientPrimaryInfo ClientPrimaryInfo { get; set; }
        public List<ClientPrimaryCareInfo> ClientPrimaryCareInfo { get; set; }
        public ClientBoadingNote ClientBoadingNotes { get; set; }
        public ClientAdditionalNote ClientAdditionalNote { get; set; }
        public ClientFunding ClientFunding { get; set; }
        public List<ClientFundingInfo> ClientFundingInfo { get; set; }
        public ClientSupportCoordinatorModel ClientSupportCoordinatorModel { get; set; }
        public ClientGuardianModels ClientGuardianModels { get; set; }
        public ClientBehaviourofConcern ClientBehaviourofConcernModel { get; set; }
        public ClientCultureNeed ClientCultureNeedModel { get; set; }
        public ClientEatingNutrition ClientEatingNutritionModel { get; set; }
        public ClientLivingArrangement ClientLivingArrangementModel { get; set; }
        public ClientOtherInformtion ClientOtherInformtionModel { get; set; }
        public ClientSafetyBehavioursInfo ClientSafetyBehavioursInfoModel { get; set; }
        public ClientSocialConnections ClientSocialConnectionsModel { get; set; }
        public ClientPersonalPreferences ClientPersonalPreferencesModel { get; set; }
       
    }

}
