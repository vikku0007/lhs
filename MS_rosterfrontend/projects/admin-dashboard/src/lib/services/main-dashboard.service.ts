import { Injectable } from '@angular/core';
import { ApiService } from 'projects/core/src/lib/services/api-service/api.service';
import { Constants } from '../config/constants';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class MainDashboardService {

  constructor(private apiService: ApiService) { }

  getAllClientFundingList(data) {
    return this.apiService.post(Constants.getAllClientFundingList, data).pipe(map(res => {
      return res;
    }))
  }
  getshifthours(StartDate, EndDate) {
    return this.apiService.get(Constants.getshifthours + '?StartDate=' + StartDate + '&EndDate=' + EndDate, null).pipe(map(res => {
      return res;
    }));
  }
  getDashboardShiftTimeStatus(StartDate, EndDate) {
    return this.apiService.get(Constants.getDashboardShiftTimeStatus + '?StartDate=' + StartDate + '&EndDate=' + EndDate, null).pipe(map(res => {
      return res;
    }));
  }

  getSchedulesShiftAdminDashboard() {
    return this.apiService.get(Constants.getSchedulesShiftAdminDashboard).pipe(map(res => {
      return res;
    }))
  }
  getFinishedShiftDetails() {
    return this.apiService.get(Constants.getFinishedShiftDetails).pipe(map(res => {
      return res;
    }))
  }
  getOnShiftDetails() {
    return this.apiService.get(Constants.getOnShiftDetails).pipe(map(res => {
      return res;
    }))
  }
  getAdminNotification(data) {
    return this.apiService.post(Constants.getAdminNotification, data, null).pipe(map(res => {
      return res;
    }))
  }
  UpdateNotification(data) {
    return this.apiService.post(Constants.updateNotification, data, null).pipe(map(res => {
      return res;
    }))
  }
  getShiftList(data) {
    return this.apiService.post(Constants.getShiftList, data, null).pipe(map(res => {
      return res;
    }));
  }
}
