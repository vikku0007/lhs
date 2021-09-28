import { Injectable } from '@angular/core';
import { ApiService } from 'projects/core/src/projects';
import { Constants } from '../config/constants';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class OtherService {

  constructor(private apiService: ApiService) { }

  getAccidentIncidentList(data) {
    return this.apiService.post(Constants.getEmployeeAccidentIncident, data).pipe(map(res => {
      return res;
    }));
  }
  addEmployeeStaffWarning(data) {
    debugger;
    return this.apiService.post(Constants.addEmployeeStaffWarning, data).pipe(map(res => {
    return res;
    }));
  }
   getEmployeestaffwarning(data) {
    return this.apiService.post(Constants.getEmployeestaffwarning, data).pipe(map(res => {
      return res;
    }));
  }
  UpdateEmployeeStaffWarning(data) {
    return this.apiService.post(Constants.UpdateEmployeeStaffWarning, data).pipe(map(res => {
    return res;
    }));
  }

  DeleteStaffDetails(data) {
    return this.apiService.post(Constants.DeleteStaffInfo, data).pipe(map(res => {
      return res;
    }))
  }
  getCommunicationInfo(data) {
    return this.apiService.post(Constants.getCommunicationInfo, data, null).pipe(map(res => {
      return res;
    }));
  }
 addCommunicationInfo(data) {
    return this.apiService.post(Constants.addCommunicationInfo, data, null).pipe(map(res => {
      return res;
    }));
  }

  getComplianceList(data) {
    return this.apiService.post(Constants.getComplianceList, data).pipe(map(res => {
      return res;
    }))
  }

  editComplianceInfo(data) {
    return this.apiService.post(Constants.editComplianceInfo, data).pipe(map(res => {
      return res;
    }));
  }

  deleteComplianceDocument(data){
    return this.apiService.post(Constants.deleteComplianceDoc, data).pipe(map(res => {
      return res;
    }));
  }

  DeleteRequireComplianceDetails(data) {
    return this.apiService.post(Constants.deleteComplianceDetails, data, null).pipe(map(res => {
      return res;
    }))
  }
  AddAccidentIncidentList(data) {
    return this.apiService.post(Constants.updateEmployeeAccidentIncident, data).pipe(map(res => {
      return res;
    }));
  }
  DeleteAccidentDetails(data) {
    return this.apiService.post(Constants.DeleteAccidentInfo, data, null).pipe(map(res => {
      return res;
    }))
  }
  EditAccidentIncidentList(data) {
    return this.apiService.post(Constants.updateEmployeeAccidentinfo, data).pipe(map(res => {
      return res;
    }));
  }
  AddRequireCompliance(data) {
    return this.apiService.post(Constants.addEmployeeCompliancesDetail, data, null).pipe(map(res => {
      return res;
    }));
  }

  GetEmployeeShiftList(data) {
    return this.apiService.post(Constants.getEmployeeShiftList, data).pipe(map(res => {
      return res;
    }))
  }

  getCheckOutClientList(data) {
    return this.apiService.post(Constants.GetCheckOutClientList, data).pipe(map(res => {
      return res;
    }));

  }

  GetAllIncidentDetails(data) {
    return this.apiService.post(Constants.GetAllIncidentDetails, data, null).pipe(map(res => {
      return res
    }))
  }

}
