import { Injectable } from '@angular/core';
import { ApiService } from 'projects/core/src/lib/services/api-service/api.service';
import { Constants } from '../config/constants';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ClientService {

  constructor(private apiService: ApiService) { }

  getClientList(data) {
    return this.apiService.post(Constants.getAllClient, data).pipe(map(res => {
      return res;
    }))
  }

  addClientDetails(data) {
    return this.apiService.post(Constants.addClientDetails, data).pipe(map(res => {
      return res;
    }))
  }

  deleteClientDetails(data) {
    return this.apiService.post(Constants.deleteClientDetails, data).pipe(map(res => {
      return res;
    }))
  }
  AddPrimaryCareInfo(data) {
    return this.apiService.post(Constants.AddPrimaryCareInfo, data,null).pipe(map(res => {
      return res;
    }))
  }
  AddClientOnBoardingNotes(data) {
    return this.apiService.post(Constants.AddClientOnBoardingNotes, data,null).pipe(map(res => {
      return res;
    }))
  }
  AddClientAdditionalNotes(data) {
    return this.apiService.post(Constants.AddClientAdditionalNotes, data,null).pipe(map(res => {
      return res;
    }))
  }
  AddClientFundingInfo(data) {
    return this.apiService.post(Constants.AddClientFundingInfo, data,null).pipe(map(res => {
      return res;
    }))
  }
  AddClientFundingTypeInfo(data) {
    return this.apiService.post(Constants.AddClientFundingTypeInfo, data,null).pipe(map(res => {
      return res;
    }))
  }
  EditClientFundingTypeInfo(data) {
    return this.apiService.post(Constants.EditClientFundingTypeInfo, data,null).pipe(map(res => {
      return res;
    }))
  }
  DeleteClientFundingTypeInfo(data) {
    return this.apiService.post(Constants.DeleteClientFundingTypeInfo, data,null).pipe(map(res => {
      return res;
    }))
  }
  GetClientFundingTypeInfo(data) {
    return this.apiService.post(Constants.GetClientFundingList, data,null).pipe(map(res => {
      return res;
    }))
  }
  UpdateClientBasicInfo(data) {
    return this.apiService.post(Constants.UpdateClientBasicInfo, data,null).pipe(map(res => {
      return res;
    }))
  }
  getClientPrimaryInfo(data) {
    return this.apiService.post(Constants.GetClientPrimaryInfo, data).pipe(map(res => {
      return res;
    }))
  }

  GetClientDetailPageInfo(data) {
    return this.apiService.post(Constants.GetClientDetailPageInfo, data,null).pipe(map(res => {
      return res;
    }))
  }
  GetAllClientMedicalHistory(data) {
    return this.apiService.post(Constants.GetAllClientMedicalHistory, data,null).pipe(map(res => {
      return res;
    }))
  }
  deleteClientMedicalHistory(data) {
    return this.apiService.post(Constants.deleteClientMedicalhistory, data, null).pipe(map(res => {
      return res;
    }))
  }

  addClientMedicalHistory(data) {
    return this.apiService.post(Constants.AddClientMedicalHistory, data, null).pipe(map(res => {
      return res;
    }))
  }
  addClientProgressNotes(data) {
    return this.apiService.post(Constants.AddClientProgressNote, data, null).pipe(map(res => {
      return res;
    }))
  }
  addClientrogressNotesItem(data) {
    return this.apiService.post(Constants.AddClientProgressNoteItem, data, null).pipe(map(res => {
      return res;
    }))
  }
  getClientrogressNotes(data) {
    return this.apiService.post(Constants.GetClientProgressNote, data, null).pipe(map(res => {
      return res;
    }))
  }
  getClientrogressNotesItemList(data) {
    return this.apiService.post(Constants.GetClientProgressNoteItem, data, null).pipe(map(res => {
      return res;
    }))
  }
  updateClientrogressNotesItem(data) {
    return this.apiService.post(Constants.UpdateClientProgressNoteItem, data, null).pipe(map(res => {
    
      return res;
    }))
  }
  GetAllClientProgressNotes(data) {
    return this.apiService.post(Constants.GetAllClientProgressNotes, data,null).pipe(map(res => {
      return res;
    }))
  }
  DeleteClientProgressNotes(data) {
    return this.apiService.post(Constants.DeleteClientProgressNotes, data).pipe(map(res => {
      return res;
    }))
  }
  GetAllClientProgressSingleNote(data) {
    return this.apiService.post(Constants.GetClientProgressSingleNote, data,null).pipe(map(res => {
      return res;
    }))
  }
  DeleteClientProgressNotesItem(data) {
    return this.apiService.post(Constants.DeleteClientProgressNotesItem, data).pipe(map(res => {
      return res;
    }))
  }
  getClientMedicalHistory(data) {
    return this.apiService.post(Constants.GetClientMedicalHistoryDetails, data).pipe(map(res => {
      return res;
    }))
  }

  AddClientSuppportCoordinator(data) {
    return this.apiService.post(Constants.AddClientSuppportCoordinator, data,null).pipe(map(res => {
      return res;
    }))
  }

  AddClientCompliancesInfo(data) {
    return this.apiService.post(Constants.AddClientCompliancesInfo, data,null).pipe(map(res => {
      return res;
    }))
  }
  GetClientCompliancesList(data) {
    return this.apiService.post(Constants.GetClientCompliancesList, data,null).pipe(map(res => {
      return res;
    }))
  }
  UpdateClientCompliancesInfo(data) {
    return this.apiService.post(Constants.UpdateClientCompliancesInfo, data,null).pipe(map(res => {
      return res;
    }))
  }
  DeleteClientComplianceInfo(data) {
    return this.apiService.post(Constants.DeleteClientComplianceInfo, data,null).pipe(map(res => {
      return res;
    }))
  }
  GetClientAlldocumentList(data) {
    return this.apiService.post(Constants.GetClientAlldocumentList, data,null).pipe(map(res => {
      return res;
    }))
  }
  DeleteClientDocument(data) {
    return this.apiService.post(Constants.DeleteClientDocument, data,null).pipe(map(res => {
      return res;
    }))
  }
  AddClientAccidentIncidentInfo(data) {
    return this.apiService.post(Constants.AddClientAccidentIncidentInfo, data,null).pipe(map(res => {
      return res;
    }))
  }
  GetClientAccidentIncidentList(data) {
    return this.apiService.post(Constants.GetClientAccidentIncidentList, data,null).pipe(map(res => {
      return res;
    }))
  }
  UpdateClientAccidentIncident(data) {
    return this.apiService.post(Constants.UpdateClientAccidentIncident, data,null).pipe(map(res => {
      return res;
    }))
  }
  DeleteClientAccidentIncident(data) {
    return this.apiService.post(Constants.DeleteClientAccidentIncident, data,null).pipe(map(res => {
      return res;
    }))
  }
  GetAllClientAccidentList(data) {
    return this.apiService.post(Constants.GetAllClientAccidentList, data,null).pipe(map(res => {
      return res;
    }))
  }
  UpdateClientProfileImage(data) {
    return this.apiService.post(Constants.UpdateClientProfileImage, data, null).pipe(map(res => {
      return res;
    }));
  }
  GetClientAccidentDetails(data) {
    return this.apiService.post(Constants.GetClientAccidentDetails, data,null).pipe(map(res => {
      return res;
    }))
  }
  GetClientCompliancesDetails(data) {
    return this.apiService.post(Constants.GetClientCompliancesDetails, data,null).pipe(map(res => {
      return res;
    }))
  }
  GetClientMedicalDetails(data) {
    return this.apiService.post(Constants.GetClientMedicalDetails, data,null).pipe(map(res => {
      return res;
    }))
  }
  GetProgressNotesDetails(data) {
    return this.apiService.post(Constants.GetProgressNotesDetails, data,null).pipe(map(res => {
      return res;
    }))
  }
  AddGuardianInfo(data) {
    return this.apiService.post(Constants.AddGuardianInfo, data,null).pipe(map(res => {
      return res;
    }))
  }
  GetClientContactOtherPerson(data) {
    return this.apiService.post(Constants.GetClientContactOtherPerson, data,null).pipe(map(res => {
      return res;
    }))
  }
  DeleteClientContactOtherPerson(data) {
    return this.apiService.post(Constants.DeleteClientContactOtherPerson, data,null).pipe(map(res => {
      return res;
    }))
  }
 
  UpdateClientActiveStatus(data) {
    return this.apiService.post(Constants.UpdateActiveClientStatus, data,null).pipe(map(res => {
      return res
    }))
  }
  AddClientCultureNeedInfo(data) {
    return this.apiService.post(Constants.AddClientCultureNeedInfo, data,null).pipe(map(res => {
      return res
    }))
  }
  AddClientEatingNutritionInfo(data) {
    return this.apiService.post(Constants.AddClientEatingNutritionInfo, data,null).pipe(map(res => {
      return res
    }))
  }
  AddClientBehaviourConcernInfo(data) {
  
    return this.apiService.post(Constants.AddClientBehaviourConcernInfo, data,null).pipe(map(res => {
      return res
    }))
  }
  AddClientLivingArrangementInfo(data) {
   return this.apiService.post(Constants.AddClientLivingArrangementInfo, data,null).pipe(map(res => {
      return res
    }))
  }
  AddClientOtherInformtionInfo(data) {
    return this.apiService.post(Constants.AddClientOtherInformtionInfo, data,null).pipe(map(res => {
      return res
    }))
  }
  AddClientSocialConnectionsInfo(data) {
     return this.apiService.post(Constants.AddClientSocialConnectionsInfo, data,null).pipe(map(res => {
      return res
    }))
  }
  AddClientPreferencesInfo(data) {
    return this.apiService.post(Constants.AddClientPreferencesInfo, data,null).pipe(map(res => {
      return res
    }))
  }
  GetClientProfileDetails(data) {
    return this.apiService.post(Constants.getClientProfileDetails, data,null).pipe(map(res => {
      return res
    }))
  }

  AddIncidentCategory(data) {
    return this.apiService.post(Constants.AddClientIncidentCategory, data,null).pipe(map(res => {
      return res
    }))
  }
  AddIncidentDeclaration(data) {
    return this.apiService.post(Constants.AddIncidentDeclaration, data,null).pipe(map(res => {
      return res
    }))
  }
  AddIncidentDetails(data) {
    return this.apiService.post(Constants.AddIncidentDetails, data,null).pipe(map(res => {
      return res
    }))
  }
  AddIncidentDocuments(data) {
    return this.apiService.post(Constants.AddIncidentDocuments, data,null).pipe(map(res => {
      return res
    }))
  }
  AddIncidentImmediateAction(data) {
    return this.apiService.post(Constants.AddIncidentImmediateAction, data,null).pipe(map(res => {
      return res
    }))
  }
  AddIncidentImpactedPerson(data) {
    return this.apiService.post(Constants.AddIncidentImpactedPerson, data,null).pipe(map(res => {
      return res
    }))
  }
  AddIncidentContactInfo(data) {
    return this.apiService.post(Constants.AddIncidentContactInfo, data,null).pipe(map(res => {
      return res
    }))
  }
  AddIncidentProviderdetail(data) {
    return this.apiService.post(Constants.AddIncidentProviderdetail, data,null).pipe(map(res => {
      return res
    }))
  }
  AddIncidentRiskAssesment(data) {
    return this.apiService.post(Constants.AddIncidentRiskAssesment, data,null).pipe(map(res => {
      return res
    }))
  }
  AddIncidentSubjectAllegation(data) {
    return this.apiService.post(Constants.AddIncidentSubjectAllegation, data,null).pipe(map(res => {
      return res
    }))
  }
  UpdateIncidentSubjectAllegation(data) {
    return this.apiService.post(Constants.UpdateIncidentSubjectAllegation, data,null).pipe(map(res => {
      return res
    }))
  }
  GetAllIncidentDetails(data) {
    return this.apiService.post(Constants.GetAllIncidentDetails, data,null).pipe(map(res => {
      return res
    }))
  }
  UpdateIncidentDocuments(data) {
    return this.apiService.post(Constants.UpdateIncidentDocuments, data,null).pipe(map(res => {
      return res
    }))
  }
  DeleteIncidentDocument(data) {
    return this.apiService.post(Constants.DeleteIncidentDocument, data,null).pipe(map(res => {
      return res;
    }))
  }
  GetIncidentDocumentDetail(data) {
    return this.apiService.post(Constants.GetIncidentDocumentDetail, data,null).pipe(map(res => {
      return res;
    }))
  }
  GetIncidentImpactedPerson(data) {
    return this.apiService.post(Constants.GetIncidentImpactedPerson, data,null).pipe(map(res => {
      return res;
    }))
  }
  GetIncidentSubjectAllegation(data) {
    return this.apiService.post(Constants.GetIncidentSubjectAllegation, data,null).pipe(map(res => {
      return res;
    }))
  }
  GetClientIncidentCategory(data) {
    return this.apiService.post(Constants.GetClientIncidentCategory, data,null).pipe(map(res => {
      return res;
    }))
  }
  getAccidentIncidentInfo(data) {
    return this.apiService.post(Constants.getAccidentIncidentInfo, data).pipe(map(res => {
      return res;
    }));
  }
  GenerateUserANdPas(data) {
    return this.apiService.post(Constants.generateUserandPass, data,null).pipe(map(res => {
      return res;
    }))
  }
  getClientShitList(data) {
    return this.apiService.post(Constants.getClientShitList, data,null).pipe(map(res => {
      return res;
    }))
  }
  getRiskAssesment(data) {
    return this.apiService.post(Constants.getRiskAssesment, data).pipe(map(res => {
      return res;
    }));
  }
  DeleteClientAgreement(data) {
    return this.apiService.post(Constants.deleteClientAgreement, data,null).pipe(map(res => {
      return res;
    }))
  }
  GetClientAgreementInfo(data) {
    return this.apiService.post(Constants.getClientAgreementInfo, data,null).pipe(map(res => {
      return res;
    }))
  }
}
