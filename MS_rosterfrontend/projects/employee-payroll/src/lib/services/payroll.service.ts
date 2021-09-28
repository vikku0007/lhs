import { Injectable } from '@angular/core';
import { ApiService } from 'projects/core/src/projects';
import { Constants } from '../config/constants';
import { map } from 'rxjs/operators';
@Injectable({
  providedIn: 'root'
})
export class PayrollService {

  constructor(private apiService: ApiService) { }
  getEmployeeHoursList(employeeid: number, startDate, enddate, status: Boolean) {
    return this.apiService.get(Constants.getEmployeeHoursList + '?employeeid=' + employeeid + '&startDate=' + startDate + '&enddate=' + enddate + '&IsOnTime=' + status, null).pipe(map(res => {
      return res;
    }));
  }
  getEmployeeHoursDetails(employeeid: number, startDate, enddate, status: Boolean) {
    return this.apiService.get(Constants.getEmployeeHoursDetail + '?employeeid=' + employeeid + '&startDate=' + startDate + '&enddate=' + enddate + '&IsOnTime=' + status, null).pipe(map(res => {
      return res;
    }));
  }
  ApproveHours(data) {
    return this.apiService.post(Constants.approveHours, data, null).pipe(map(res => {
      return res;
    }));
  }

  IncompleteShift(data) {
    return this.apiService.post(Constants.incompleteShift, data, null).pipe(map(res => {
      return res;
    }));
  }

  AddCheckoutDetails(data) {
    return this.apiService.post(Constants.addCheckoutDetails, data, null).pipe(map(res => {
      return res;
    }));
  }
  getEmployeeMyObHoursDetails(employeeid: number, startDate, enddate, status: Boolean) {
    return this.apiService.get(Constants.getEmployeeMyObHoursDetails + '?employeeid=' + employeeid + '&startDate=' + startDate + '&enddate=' + enddate + '&IsOnTime=' + status, null).pipe(map(res => {
      return res;
    }));
  }
}
