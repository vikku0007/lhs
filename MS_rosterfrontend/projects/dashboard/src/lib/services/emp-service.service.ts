import { Injectable } from '@angular/core';
import { ApiService } from 'projects/core/src/projects';
import { Constants } from '../config/constants';
import { map } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class EmpServiceService {
  constructor(private apiService: ApiService) { }
  getEmployeeList(data) {
    return this.apiService.post(Constants.getAllEmployee, data).pipe(map(res => {
      return res;
    }))
  }

  addEmployeeDetails(data) {
    return this.apiService.post(Constants.addEmployeeDetails, data, null).pipe(map(res => {
      return res;
    }))
  }
  getEmployeeDetails(id) {
    return this.apiService.get(Constants.getEmployeeDetails + '?id=' + id, null).pipe(map(res => {
      return res;
    }));
  }
  EditEmployeeDetails(data) {
    return this.apiService.post(Constants.editEmployeeDetails, data, null).pipe(map(res => {
      return res;
    }));
  }

  updateEmployeeDetails(data) {
    return this.apiService.post(Constants.updateEmployeeAwardDetails, data, null).pipe(map(res => {
      return res;
    }))
  }
  updateEmployeeKinInfo(data) {
    return this.apiService.post(Constants.updateEmployeeKinInfo, data, null).pipe(map(res => {
      return res;
    }))
  }
  updateEmployeeJobProfile(data) {
    return this.apiService.post(Constants.updateEmployeeJobProfile, data, null).pipe(map(res => {
      return res;
    }));
  }
  updateLicenseInfo(data) {
    return this.apiService.post(Constants.updateLicenseInfo, data, null).pipe(map(res => {
      return res;
    }))
  }

  AddEmpEducation(data) {
    return this.apiService.post(Constants.updateEducationInfo, data, null).pipe(map(res => {
      return res;
    }))
  }
  updatePayRate(data) {
    return this.apiService.post(Constants.updatePayrate, data, null).pipe(map(res => {
      return res;
    }))
  }
  updateEmployeeMiscInfo(data) {
    return this.apiService.post(Constants.updateMiscInfo, data, null).pipe(map(res => {
      return res;
    }))
  }

  updateWorkExperience(data) {
    return this.apiService.post(Constants.updateWorkExperience, data, null).pipe(map(res => {
      return res;
    }))
  }
  getAccidentIncidentList(data) {
    return this.apiService.post(Constants.getEmployeeAccidentIncident, data).pipe(map(res => {
      return res;
    }));
  }
  AddAccidentIncidentList(data) {
    return this.apiService.post(Constants.updateEmployeeAccidentIncident, data).pipe(map(res => {
      return res;
    }));
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
  getEmpLeaveList(data) {
    return this.apiService.post(Constants.getEmployeeLeaveList, data).pipe(map(res => {
      return res;
    }));
  }
  AddEmpLeave(data) {
    return this.apiService.post(Constants.AddEmpLeave, data, null).pipe(map(res => {
      return res;
    }))
  }
  UpdateEmpLeave(data) {
    return this.apiService.post(Constants.UpdateEmpLeave, data, null).pipe(map(res => {
      return res;
    }))
  }
  getAllEmployeeCommunication(data) {
    return this.apiService.post(Constants.getAllEmployeeCommunication, data, null).pipe(map(res => {
      return res;
    }));
  }
  getAllEmployeeAccident(data) {
    return this.apiService.post(Constants.getAllEmployeeAccident, data, null).pipe(map(res => {
      return res;
    }));
  }
  addEmployeeStaffWarning(data) {
    return this.apiService.post(Constants.addEmployeeStaffWarning, data, null).pipe(map(res => {

      return res;
    }));
  }
  getAllEmployeeAppraisalList(data) {
    return this.apiService.post(Constants.getAllEmployeeAppraisalList, data, null).pipe(map(res => {
      return res;
    }));
  }

  getRequireComplianceList(data) {
    return this.apiService.post(Constants.getRequireComplianceList, data, null).pipe(map(res => {
      return res;
    }));
  }
  getOtherComplianceList(data) {
    return this.apiService.post(Constants.getOtherComplianceList, data, null).pipe(map(res => {
      return res;
    }));
  }
  getAllEmployeeLeaveList(data) {
    return this.apiService.post(Constants.getAllEmployeeLeaveList, data, null).pipe(map(res => {
      return res;
    }));
  }
  getAllEmployeestaffwarningList(data) {
    return this.apiService.post(Constants.getAllEmployeestaffwarningList, data, null).pipe(map(res => {
      return res;
    }));
  }
  AddEmployeeAppraisalDetail(data) {
    return this.apiService.post(Constants.addEmployeeAppraisalInfo, data, null).pipe(map(res => {
      return res;
    }));
  }
  AddRequireCompliance(data) {
    return this.apiService.post(Constants.addEmployeeCompliancesDetail, data, null).pipe(map(res => {
      return res;
    }));
  }

  DeleteAccidentDetails(data) {
    return this.apiService.post(Constants.DeleteAccidentInfo, data, null).pipe(map(res => {
      return res;
    }))
  }
  DeleteAppraisalDetails(data) {
    return this.apiService.post(Constants.DeleteAppraisalInfo, data, null).pipe(map(res => {
      return res;
    }))
  }
  DeleteCommunicationDetails(data) {
    return this.apiService.post(Constants.DeleteCommunicationInfo, data, null).pipe(map(res => {
      return res;
    }))
  }
  DeleteLeaveDetails(data) {
    return this.apiService.post(Constants.DeleteLeaveInfo, data, null).pipe(map(res => {
      return res;
    }))
  }
  DeleteStaffDetails(data) {
    return this.apiService.post(Constants.DeleteStaffInfo, data, null).pipe(map(res => {
      return res;
    }))
  }
  DeleteRequireComplianceDetails(data) {
    return this.apiService.post(Constants.DeleteRequireComplianceDetails, data, null).pipe(map(res => {
      return res;
    }))
  }
  DeleteOtherComplianceDetails(data) {
    return this.apiService.post(Constants.DeleteOtherComplianceDetails, data, null).pipe(map(res => {
      return res;
    }))
  }
  getEmployeestaffwarningList(data) {
    return this.apiService.post(Constants.getEmployeestaffwarningList, data, null).pipe(map(res => {
      return res;
    }));
  }
  UpdateEmployeeStaffWarning(data) {
    return this.apiService.post(Constants.UpdateEmployeeStaffWarning, data, null).pipe(map(res => {

      return res;
    }));
  }

  AddOtherComplianceDetails(data) {
    return this.apiService.post(Constants.AddOtherComplianceDetails, data, null).pipe(map(res => {
      return res;
    }))
  }
  GetOtherComplianceDetails(data) {
    return this.apiService.post(Constants.getOtherComplianceDetails, data, null).pipe(map(res => {
      return res;
    }))
  }
  UpdateOtherComplianceDetails(data) {
    return this.apiService.post(Constants.updateOtherComplianceDetails, data, null).pipe(map(res => {
      return res;
    }))
  }
  EditCommunicationInfo(data) {
    return this.apiService.post(Constants.EditCommunicationInfo, data, null).pipe(map(res => {
      return res;
    }));
  }
  EditAccidentIncidentList(data) {
    return this.apiService.post(Constants.updateEmployeeAccidentinfo, data).pipe(map(res => {
      return res;
    }));
  }

  getEmployeePrimaryInfo(id) {
    return this.apiService.get(Constants.getEmployeePrimaryInfo + '?id=' + id, null).pipe(map(res => {
      return res;
    }));
  }
  getEmployeeAppraisalList(data) {
    return this.apiService.post(Constants.getEmployeeAppraisalList, data, null).pipe(map(res => {
      return res;
    }));
  }
  EditRequireCompliance(data) {
    return this.apiService.post(Constants.EditEmployeeCompliancesDetail, data, null).pipe(map(res => {
      return res;
    }));
  }
  EditEmployeeAppraisalDetail(data) {
    return this.apiService.post(Constants.EditEmployeeAppraisalInfo, data, null).pipe(map(res => {
      return res;
    }));
  }
  UpdateEmployeeProfileImage(data) {
    return this.apiService.post(Constants.updateRmployeeProfileImage, data, null).pipe(map(res => {
      return res;
    }));
  }
  DeleteEmployeeRequireDocument(data) {
    return this.apiService.post(Constants.DeleteEmployeeRequireDocument, data,null).pipe(map(res => {
      return res;
    }))
  }
  DeleteEmployeeOtherDocument(data) {
    return this.apiService.post(Constants.DeleteEmployeeOtherDocument, data,null).pipe(map(res => {
      return res;
    }))
  }
  getAllRequireComplianceList(data) {
    return this.apiService.post(Constants.getAllRequireComplianceList, data, null).pipe(map(res => {
      return res;
    }));
  }
  getAllOtherComplianceList(data) {
    return this.apiService.post(Constants.getAllOtherComplianceList, data, null).pipe(map(res => {
      return res;
    }));
  }
  getCommunicationById(data) {
    return this.apiService.post(Constants.getCommunicationById, data, null).pipe(map(res => {
      return res;
    }));
  }
  getLeaveById(data) {
    return this.apiService.post(Constants.getLeaveById, data, null).pipe(map(res => {
      return res;
    }));
  }
  getaccidentById(data) {
    return this.apiService.post(Constants.getaccidentById, data, null).pipe(map(res => {
      return res;
    }));
  }
  getRequiredataById(data) {
    return this.apiService.post(Constants.getRequiredataById, data, null).pipe(map(res => {
      return res;
    }));
  }
  getOtherDetailById(data) {
    return this.apiService.post(Constants.getotherdataById, data, null).pipe(map(res => {
      return res;
    }));
  }
  getStaffwarningById(data) {
    return this.apiService.post(Constants.getstaffwarningdataById, data, null).pipe(map(res => {
      return res;
    }));
  }
  GetEmployeeAppraisaldetails(data) {
    return this.apiService.post(Constants.GetEmployeeAppraisaldetails, data, null).pipe(map(res => {
      return res;
    }));
  }
  DeleteEmployeedetails(data) {
    return this.apiService.post(Constants.DeleteEmployeedetails, data, null).pipe(map(res => {
      return res;
    }));
  }
  DeleteEmployeeEducationdetails(data) {
    return this.apiService.post(Constants.DeleteEmployeeEducationdetails, data, null).pipe(map(res => {
      return res;
    }));
  }
  DeleteEmployeeExperiencedetails(data) {
    return this.apiService.post(Constants.DeleteEmployeeExperiencedetails, data, null).pipe(map(res => {
      return res;
    }));
  }
  AddEmployeeTraining(data) {
    return this.apiService.post(Constants.AddEmployeeTraining, data, null).pipe(map(res => {
      return res;
    }))
  }
  DeleteEmployeeTraining(data) {
    return this.apiService.post(Constants.DeleteEmployeeTraining, data, null).pipe(map(res => {
      return res;
    }))
  }
  UpdateEmployeeTraining(data) {
    return this.apiService.post(Constants.UpdateEmployeeTraining, data, null).pipe(map(res => {
      return res;
    }))
  }

  GetEmployeeTrainingList(data) {
    return this.apiService.post(Constants.GetEmployeeTrainingList, data, null).pipe(map(res => {
      return res;
    }))
  }
  GetEmployeeEducationList(data) {
    return this.apiService.post(Constants.GetEmployeeEducationList, data, null).pipe(map(res => {
      return res;
    }))
  }
  GetEmployeeExperienceList(data) {
    return this.apiService.post(Constants.GetEmployeeExperienceList, data, null).pipe(map(res => {
      return res;
    }))
  }
  DeleteEmployeeEducationDocument(data) {
    return this.apiService.post(Constants.DeleteEmployeeEducationDocument, data,null).pipe(map(res => {
      return res;
    }))
  }
  DeleteEmployeeExperienceDocument(data) {
    return this.apiService.post(Constants.DeleteEmployeeExperienceDocument, data,null).pipe(map(res => {
      return res;
    }))
  }
  DeleteEmployeeTrainingDocument(data) {
    return this.apiService.post(Constants.DeleteEmployeeTrainingDocument, data,null).pipe(map(res => {
      return res
    }))
  }
  UpdateActiveInActiveStatus(data) {
    return this.apiService.post(Constants.UpdateActiveInActiveStatus, data,null).pipe(map(res => {
      return res
    }))
  }
  GetEmployeeTodayShift(data) {
    return this.apiService.post(Constants.GetEmployeeTodayShift, data, null).pipe(map(res => {
      return res;
    }))
  }
  UpdateEmployeePassword(data) {
    return this.apiService.post(Constants.UpdateEmployeePassword, data, null).pipe(map(res => {
      return res;
    }))
  }
  getEmployeePassword(data) {
    return this.apiService.post(Constants.getEmployeePassword, data, null).pipe(map(res => {
      return res;
    }))
  }
  GetEmployeeDriverList(data) {
    return this.apiService.post(Constants.GetEmployeeDriverList, data, null).pipe(map(res => {
      return res;
    }))
  }
  DeleteEmployeeDrivingLicense(data) {
    return this.apiService.post(Constants.DeleteEmployeeDrivingLicense, data, null).pipe(map(res => {
      return res;
    }))
  }
  ApproveLeaveById(data) {
    return this.apiService.post(Constants.ApproveLeaveById, data, null).pipe(map(res => {
      return res;
    }));
  }

  RejectLeaveById(data) {
    return this.apiService.post(Constants.RejectLeaveById, data, null).pipe(map(res => {
      return res;
    }));
  }
 
}