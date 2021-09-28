import { Injectable } from '@angular/core';
import { ApiService } from 'projects/core/src/projects';
import { Constants } from '../config/constants';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class RosterService {

  constructor(private apiService: ApiService) { }

  getClientCalendarShifts(data) {
    return this.apiService.post(Constants.getClientCalendarShifts, data).pipe(map(res => {
      return res;
    }));
  }
}
