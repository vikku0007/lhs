import { Injectable } from '@angular/core';
import { ApiService } from 'projects/core/src/projects';
import { Constants } from '../config/constants';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class TimesheetService {

  constructor(private apiService: ApiService) { }

  getEmployees() {
    return this.apiService.get(Constants.getEmployees).pipe(map(res => {
      return res;
    }))
  }

  getTimeSheet(data) {
    return this.apiService.get(Constants.getEmployeeTimesheet + '?startDate=' + data.startDate + '&endDate=' + data.endDate, data).pipe(map(res => {
      return res;
    }))
  }

  getHourlyTimeSheet(data) {
    return this.apiService.get(Constants.getHourlyTimeSheet + '?startDate=' + data.startDate + '&endDate=' + data.endDate, data).pipe(map(res => {
      return res;
    }))
  }
}
