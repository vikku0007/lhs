import { Injectable } from '@angular/core';
import { ApiService } from 'projects/core/src/projects';
import { Constants } from '../../config/constants';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class CommonService {

  constructor(private apiService: ApiService) { }

  getLanguageList() {
    return this.apiService.get(Constants.GetLanguage).pipe(map(res => {
      return res;
    }))
  }

  getGenderList() {
    return this.apiService.get(Constants.getGenderList).pipe(map(res => {
      return res;
    }))
  }

  getDepartmentList() {
    return this.apiService.get(Constants.getDepartmentList).pipe(map(res => {
      return res;
    }))
  }

  getEmployeeType() {
    return this.apiService.get(Constants.getEmployeeType).pipe(map(res => {
      return res;
    }))
  }

  getLevel() {
    return this.apiService.get(Constants.getLevel).pipe(map(res => {
      return res;
    }))
  }

  getMaritalStatus() {
    return this.apiService.get(Constants.getMaritalStatus).pipe(map(res => {
      return res;
    }))
  }

  getRelationship() {
    return this.apiService.get(Constants.getRelationship).pipe(map(res => {
      return res;
    }))
  }

  getRoles() {
    return this.apiService.get(Constants.getRoles).pipe(map(res => {
      return res;
    }))
  }

  getSalutation() {
    return this.apiService.get(Constants.getSalutation).pipe(map(res => {
      return res;
    }))
  }

  getSourceOfHire() {
    return this.apiService.get(Constants.getSourceOfHire).pipe(map(res => {
      return res;
    }))
  }

  getEventType() {
    return this.apiService.get(Constants.getEventType).pipe(map(res => {
      return res;
    }))
  }

  getLocation() {
    return this.apiService.get(Constants.getLocation).pipe(map(res => {
      return res;
    }))
  }
  getWarningType() {
    return this.apiService.get(Constants.getWarningType).pipe(map(res => {
      return res;
    }))
  }
  getOffenseType() {
    return this.apiService.get(Constants.getOffenseType).pipe(map(res => {
      return res;
    }))
  }
  getReportedTo() {
    return this.apiService.get(Constants.getReportedTo).pipe(map(res => {
      return res;
    }))
  }
  getRaisedBy(data) {
    return this.apiService.post(Constants.getRaisedBy, data, null).pipe(map(res => {
      return res;
    }));
  }
  getRelationShip() {
    return this.apiService.get(Constants.getRelationship).pipe(map(res => {
      return res;
    }))
  }
  getAppraisalType() {
    return this.apiService.get(Constants.getAppraisalType).pipe(map(res => {
      return res;
    }));
  }
  getAwardGroup() {
    return this.apiService.get(Constants.GetAwardGroup).pipe(map(res => {
      return res;
    }));
  }
  getGlobalPayrate(level: number) {
    return this.apiService.get(Constants.GetGlobalPayRate + '?level=' + level).pipe(map(res => {
      return res;
    }));
  }

  getFundType() {
    return this.apiService.get(Constants.getFundType).pipe(map(res => {
      return res;
    }))
  }

  getDocumentType() {
    return this.apiService.get(Constants.getDocumentType).pipe(map(res => {
      return res;
    }))
  }
  getServiceType() {
    return this.apiService.get(Constants.getServiceType).pipe(map(res => {
      return res;
    }))
  }
  getAllClientNameList() {
    return this.apiService.get(Constants.GetAllClientNameList).pipe(map(res => {
      return res;
    }))
  }
  getDocumentName() {
    return this.apiService.get(Constants.getDocumentName).pipe(map(res => {
      return res;
    }))
  }
  getOtherDocumentName() {
    return this.apiService.get(Constants.getOtherDocumentName).pipe(map(res => {
      return res;
    }))
  }

  getDepartmentbyemployee(employeeId: number) {
    return this.apiService.get(Constants.getDepartmentbyemployee + '?employeeId=' + employeeId).pipe(map(res => {
      return res;
    }));
  }
  getReportedbyemployee(employeeId: number) {
    return this.apiService.get(Constants.getReportedbyemployee + '?employeeId=' + employeeId).pipe(map(res => {
      return res;
    }));
  }

  getLeaveType() {
    return this.apiService.get(Constants.getLeavetype).pipe(map(res => {
      return res;
    }))
  }
  getVisaType() {
    return this.apiService.get(Constants.getVisatype).pipe(map(res => {
      return res;
    }))
  }
  getShiftStatusList() {
    return this.apiService.get(Constants.getShiftStatusList ).pipe(map(res => {
      return res;
    }));
  }
  getConditionType() {
    return this.apiService.get(Constants.getConditionType).pipe(map(res => {
      return res;
    }))
  }
  getSymptomsType() {
    return this.apiService.get(Constants.getSymptomsType).pipe(map(res => {
      return res;
    }))
  }
  getEthnicityType() {
    return this.apiService.get(Constants.getEthnicityType).pipe(map(res => {
      return res;
    }))
  }
  getReligion() {
    return this.apiService.get(Constants.getreligion).pipe(map(res => {
      return res;
    }))
  }
  getCountry() {
    return this.apiService.get(Constants.getCountry).pipe(map(res => {
      return res;
    }))
  }
  getstate() {
    return this.apiService.get(Constants.getState).pipe(map(res => {
      return res;
    }))
  }
  getCourseype() {
    return this.apiService.get(Constants.getCourseype).pipe(map(res => {
      return res;
    }))
  }

  getMandatoryTraining() {
    return this.apiService.get(Constants.getMandatoryTraining).pipe(map(res => {
      return res;
    }))
  }
  getOptionalTraining() {
    return this.apiService.get(Constants.getOptionalTraining).pipe(map(res => {
      return res;
    }))
  }
  getTrainingType() {
    return this.apiService.get(Constants.getTrainingType).pipe(map(res => {
      return res;
    }))
  }
  getServiceList() {
    return this.apiService.get(Constants.getServiceList).pipe(map(res => {
      return res;
    }))
  }
  getServicerate(Id: number) {
    return this.apiService.get(Constants.GetServiceRate + '?Id=' + Id).pipe(map(res => {
      return res;
    }));
  }
  getLicenseType() {
    return this.apiService.get(Constants.getLicenseType).pipe(map(res => {
      return res;
    }))
  }

  getCodeType() {
    return this.apiService.get(Constants.getCodeType).pipe(map(res => {
      return res;
    }))
  }
  getLocationType() {
    return this.apiService.get(Constants.getLocationType).pipe(map(res => {
      return res;
    }))
  }
  getqualificationType() {
    return this.apiService.get(Constants.getqualificationType).pipe(map(res => {
      return res;
    }))
  }
  getHobbies() {
    return this.apiService.get(Constants.getHobbies).pipe(map(res => {
      return res;
    }))
  }
  getAppraisalCrieteria() {
    return this.apiService.get(Constants.getAppraisalCrieteria).pipe(map(res => {
      return res;
    }));
  }
  getTotalWarning(data) {
    return this.apiService.post(Constants.getTotalWarning, data, null).pipe(map(res => {
      return res;
    }));
  }
  getPaymentterm() {
    return this.apiService.get(Constants.getPaymentterm).pipe(map(res => {
      return res;
    }))
  }
  getPayers() {
    return this.apiService.get(Constants.getPayers).pipe(map(res => {
      return res;
    }))
  }
  getClientDocuments() {
    return this.apiService.get(Constants.getClientDocuments,null).pipe(map(res => {
      return res;
    }))
  }
  getOptionalDocument() {
    return this.apiService.get(Constants.getOptionalDocument).pipe(map(res => {
      return res;
    }))
  }
 
  GetCommunicationType() {
    return this.apiService.get(Constants.GetCommunicationType).pipe(map(res => {
      return res;
    }))
  }
  GetConcernBehaviour() {
    return this.apiService.get(Constants.GetConcernBehaviour).pipe(map(res => {
      return res;
    }))
  }
  GetPrimaryCategory() {
    return this.apiService.get(Constants.GetPrimaryCategory).pipe(map(res => {
      return res;
    }))
  }
  GetSecondaryCategory() {
    return this.apiService.get(Constants.GetSecondaryCategory).pipe(map(res => {
      return res;
    }))
  }
  GetPrimaryDisability() {
    return this.apiService.get(Constants.GetPrimaryDisability).pipe(map(res => {
      return res;
    }))
  }
  GetSecondaryDisability() {
    return this.apiService.get(Constants.GetSecondaryDisability).pipe(map(res => {
      return res;
    }))
  }
  GetShiftTime() {
    return this.apiService.get(Constants.GetShiftTime).pipe(map(res => {
      return res;
    }))
  }
  
  GetShiftItemList(data) {
    return this.apiService.post(Constants.GetShiftItem,data, null).pipe(map(res => {
      return res;
    }))
  }
 
  getallemployee() {
    return this.apiService.get(Constants.GetEmployeeList).pipe(map(res => {
      return res;
    }));
  }
  getYear() {
    return this.apiService.get(Constants.getYear).pipe(map(res => {
      return res;
    }))
  }
  GetCheckListDocument() {
    return this.apiService.get(Constants.GetCheckListDocument).pipe(map(res => {
      return res;
    }))
  }
  getEmployeelevel(EmployeeId: number) {
    return this.apiService.get(Constants.getEmployeelevel + '?EmployeeId=' + EmployeeId).pipe(map(res => {
      return res;
    }));
  }
  getImageDetails(data) {
    return this.apiService.post(Constants.getImageDetails,data,null).pipe(map(res => {
      return res;
    }));
  }
  getdriverlicense() {
    return this.apiService.get(Constants.getdriverlicense).pipe(map(res => {
      return res;
    }))
  }
  getcomplianceType() {
    return this.apiService.get(Constants.getcomplianceType).pipe(map(res => {
      return res;
    }))
  }
  getReferenceNumber(data) {
    return this.apiService.post(Constants.getReferenceNumber,data,null).pipe(map(res => {
      return res;
    }));
  }
  GetEmpCheckListDocument() {
    return this.apiService.get(Constants.GetEmpCheckListDocument).pipe(map(res => {
      return res;
    }))
  }
}
