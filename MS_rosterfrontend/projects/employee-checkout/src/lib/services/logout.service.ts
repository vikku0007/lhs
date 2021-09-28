import { Injectable } from '@angular/core';
import { ApiService } from 'projects/core/src/projects';
import { map } from 'rxjs/operators';
import { Constants } from '../config/constants';

@Injectable({
  providedIn: 'root'
})
export class LogoutService {

  constructor(private apiService: ApiService) { }

  getCheckOutClientList(data) {
    return this.apiService.post(Constants.GetCheckOutClientList, data).pipe(map(res => {
      return res;
    }));

  }
  getClientPrimaryInfo(data) {
    return this.apiService.post(Constants.GetClientPrimaryInfo, data).pipe(map(res => {
      return res;
    }))
  }

  getClientMedicalHistory(data) {
    return this.apiService.post(Constants.GetClientMedicalHistoryDetails, data).pipe(map(res => {
      return res;
    }))
  }

  AddIncidentCategory(data) {
    return this.apiService.post(Constants.AddClientIncidentCategory, data, null).pipe(map(res => {
      return res
    }))
  }
  AddIncidentDeclaration(data) {
    return this.apiService.post(Constants.AddIncidentDeclaration, data, null).pipe(map(res => {
      return res
    }))
  }
  AddIncidentDetails(data) {
    return this.apiService.post(Constants.AddIncidentDetails, data, null).pipe(map(res => {
      return res
    }))
  }
  AddIncidentDocuments(data) {
    return this.apiService.post(Constants.AddIncidentDocuments, data, null).pipe(map(res => {
      return res
    }))
  }
  AddIncidentImmediateAction(data) {
    return this.apiService.post(Constants.AddIncidentImmediateAction, data, null).pipe(map(res => {
      return res
    }))
  }
  AddIncidentImpactedPerson(data) {
    return this.apiService.post(Constants.AddIncidentImpactedPerson, data, null).pipe(map(res => {
      return res
    }))
  }
  AddIncidentContactInfo(data) {
    return this.apiService.post(Constants.AddIncidentContactInfo, data, null).pipe(map(res => {
      return res
    }))
  }
  AddIncidentProviderdetail(data) {
    return this.apiService.post(Constants.AddIncidentProviderdetail, data, null).pipe(map(res => {
      return res
    }))
  }
  AddIncidentRiskAssesment(data) {
    return this.apiService.post(Constants.AddIncidentRiskAssesment, data, null).pipe(map(res => {
      return res
    }))
  }
  AddIncidentSubjectAllegation(data) {
    return this.apiService.post(Constants.AddIncidentSubjectAllegation, data, null).pipe(map(res => {
      return res
    }))
  }
  UpdateIncidentSubjectAllegation(data) {
    return this.apiService.post(Constants.UpdateIncidentSubjectAllegation, data, null).pipe(map(res => {
      return res
    }))
  }
  GetAllIncidentDetails(data) {
    return this.apiService.post(Constants.GetAllIncidentDetails, data, null).pipe(map(res => {
      return res
    }))
  }
  UpdateIncidentDocuments(data) {
    return this.apiService.post(Constants.UpdateIncidentDocuments, data, null).pipe(map(res => {
      return res
    }))
  }
  DeleteIncidentDocument(data) {
    return this.apiService.post(Constants.DeleteIncidentDocument, data, null).pipe(map(res => {
      return res;
    }))
  }
  GetIncidentDocumentDetail(data) {
    return this.apiService.post(Constants.GetIncidentDocumentDetail, data, null).pipe(map(res => {
      return res;
    }))
  }
  GetIncidentImpactedPerson(data) {
    return this.apiService.post(Constants.GetIncidentImpactedPerson, data, null).pipe(map(res => {
      return res;
    }))
  }
  GetIncidentSubjectAllegation(data) {
    return this.apiService.post(Constants.GetIncidentSubjectAllegation, data, null).pipe(map(res => {
      return res;
    }))
  }
  GetClientIncidentCategory(data) {
    return this.apiService.post(Constants.GetClientIncidentCategory, data, null).pipe(map(res => {
      return res;
    }))
  }
  addClientMedicalHistory(data) {
    return this.apiService.post(Constants.AddClientMedicalHistory, data, null).pipe(map(res => {
      return res;
    }))
  }
  getProgressNotesList(data) {
    return this.apiService.post(Constants.getProgressNotesList, data).pipe(map(res => {
      return res;
    }));
  }
  AddClientCompliancesInfo(data) {
    return this.apiService.post(Constants.AddClientDocument, data, null).pipe(map(res => {
      return res;
    }))
  }
  GetClientCompliancesList(data) {
    return this.apiService.post(Constants.getClientdocumentList, data, null).pipe(map(res => {
      return res;
    }))
  }
  UpdateClientCompliancesInfo(data) {
    return this.apiService.post(Constants.UpdateClientDocument, data, null).pipe(map(res => {
      return res;
    }))
  }
  DeleteClientComplianceInfo(data) {
    return this.apiService.post(Constants.DeleteClientComplianceInfo, data, null).pipe(map(res => {
      return res;
    }))
  }
  DeleteClientDocument(data) {
    return this.apiService.post(Constants.DeleteClientDocument, data, null).pipe(map(res => {
      return res;
    }))
  }

  updateProgressNotesList(data) {
    return this.apiService.post(Constants.updateProgressNotesList, data).pipe(map(res => {
      return res;
    }));
  }

  addClientProgressNotes(data) {
    return this.apiService.post(Constants.addProgressNotesList, data).pipe(map(res => {
      return res;
    }));
  }

  deleteClientProgressNotesItem(data) {
    return this.apiService.post(Constants.deleteProgressNotesList, data).pipe(map(res => {
      return res;
    }));
  }

  logoutEmployee(data) {
    return this.apiService.post(Constants.logout, data).pipe(map(res => {
      return res;
    }));
  }
  getIncidentProviderInfo(data) {
    return this.apiService.post(Constants.getIncidentProviderInfo, data).pipe(map(res => {
      return res;
    }));
  }
  getIncidentPrimaryContact(data) {
    return this.apiService.post(Constants.getIncidentPrimaryContact, data).pipe(map(res => {
      return res;
    }));
  }
  getAccidentIncidentInfo(data) {
    return this.apiService.post(Constants.getAccidentIncidentInfo, data).pipe(map(res => {
      return res;
    }));
  }
  getIncidentImmediateAction(data) {
    return this.apiService.post(Constants.getIncidentImmediateAction, data).pipe(map(res => {
      return res;
    }));
  }
  getRiskAssesment(data) {
    return this.apiService.post(Constants.getRiskAssesment, data).pipe(map(res => {
      return res;
    }));
  }
  getIncidentDeclaration(data) {
    return this.apiService.post(Constants.getIncidentDeclaration, data).pipe(map(res => {
      return res;
    }));
  }
  AddToDoList(data) {
    return this.apiService.post(Constants.AddToDoList, data, null).pipe(map(res => {
      return res
    }))
  }

  GetToDoList(data) {
    return this.apiService.post(Constants.GetToDoList, data, null).pipe(map(res => {
      return res
    }))
  }

  EditToDoList(data) {
    return this.apiService.post(Constants.EditToDoList, data, null).pipe(map(res => {
      return res
    }))
  }

  UpdateToDoList(data) {
    return this.apiService.post(Constants.UpdateToDoList, data, null).pipe(map(res => {
      return res
    }))
  }
}



