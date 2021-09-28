import { Injectable } from '@angular/core';
import { ApiService } from 'projects/core/src/projects';
import { Constants } from '../config/constants';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ClientService {

  constructor(private apiService: ApiService) { }

  getCurrentShiftList(data) {
    return this.apiService.post(Constants.getCurrentShifts, data).pipe(map(res => {
      return res;
    }))
  }

  getAssignedShiftList(data) {
    return this.apiService.post(Constants.getAssignedShifts, data).pipe(map(res => {
      return res;
    }))
  }

  getShiftDetail(data) {
    return this.apiService.get(Constants.getShiftDetails + '?Id=' + data.id).pipe(map(res => {
      return res;
    }))
  }

  cancelShift(data) {
    return this.apiService.post(Constants.cancelShift, data).pipe(map(res => {
      return res;
    }))
  }
}
