import { Injectable } from '@angular/core';
import { ApiService } from 'projects/core/src/projects';
import { Constants } from '../config/constants';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class EmpDashboardService {

  constructor(private apiService: ApiService) { }

  getAssignedShifts(data) {
    return this.apiService.post(Constants.getAssignedShifts, data).pipe(map(res => {
      return res;
    }))
  }

  updateAcceptStatus(data) {
    return this.apiService.post(Constants.updateAcceptStatus, data).pipe(map(res => {
      return res;
    }))
  }

  updateRejectStatus(data) {
    return this.apiService.post(Constants.updateRejectStatus, data).pipe(map(res => {
      return res;
    }))
  }

  getCurrentShifts(data) {
    return this.apiService.post(Constants.getCurrentShifts, data).pipe(map(res => {
      return res;
    }))
  }

  getShiftDetail(data) {
    return this.apiService.get(Constants.getShiftDetails + '?Id=' + data.id).pipe(map(res => {
      return res;
    }))
  }
 
  addCheckInDetails(data) {
    return this.apiService.post(Constants.addCheckInDetails, data).pipe(map(res => {
      return res;
    }))
  }
  GetEmployeeShiftList(data) {
    return this.apiService.post(Constants.getEmployeeShiftList, data).pipe(map(res => {
      return res;
    }))
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
  DeleteLeaveDetails(data) {
    return this.apiService.post(Constants.DeleteLeaveInfo, data, null).pipe(map(res => {
      return res;
    }))
  }
  getEmployeeshifthours(EmployeeId) {
    return this.apiService.get(Constants.getEmployeeshifthours+'?EmployeeId='+EmployeeId, null).pipe(map(res => {
      return res;
    }));
  }
  getEmployeeNotification(EmployeeId) {
    return this.apiService.get(Constants.getEmployeeNotification+'?EmployeeId='+EmployeeId, null).pipe(map(res => {
      return res;
    }));
  }
  UpdateNotification(data) {
    return this.apiService.post(Constants.updateNotification,data,null).pipe(map(res => {
      return res;
    }))
  }
  getApplyleave(data) {
    return this.apiService.post(Constants.getApplyleave, data).pipe(map(res => {
      return res;
    }));
  }
  updateLeaveApproveStatus(data) {
    return this.apiService.post(Constants.updateLeaveApproveStatus, data).pipe(map(res => {
      return res;
    }));
  }
  updateLeaveRejectStatus(data) {
    return this.apiService.post(Constants.updateLeaveRejectStatus, data).pipe(map(res => {
      return res;
    }));
  }
  getClientUploadedDocument(data) {
    return this.apiService.post(Constants.getClientUploadedDocument,data).pipe(map(res => {
      return res;
    }))
  }
  GetShiftHistory(data) {
    return this.apiService.post(Constants.getShiftHistory, data, null).pipe(map(res => {
      return res;
    }));
  }
}
