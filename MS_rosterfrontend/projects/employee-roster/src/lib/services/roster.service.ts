import { Injectable } from '@angular/core';
import { ApiService } from 'projects/core/src/projects';
import { Constants } from '../config/constants';
import { map } from 'rxjs/operators';
import { DayPilot } from 'daypilot-pro-angular';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RosterService {

  events: any[] = [
    {
      id: "1",
      start: "2020-08-14T00:00:00",
      end: "2020-08-14T00:00:00",
      text: "Event 1"
    }
  ];

  constructor(private apiService: ApiService) { }

  getEvents(from: DayPilot.Date, to: DayPilot.Date): Observable<any[]> {

    // simulating an HTTP request
    return new Observable(observer => {
      setTimeout(() => {
        observer.next(this.events);
      }, 200);
    });

    // return this.http.get("/api/events?from=" + from.toString() + "&to=" + to.toString()).map((response:Response) => response.json());
  }

  getEmployeeAssignedShifts(data) {
    return this.apiService.post(Constants.getEmployeeAssignedShifts, data).pipe(map(res => {
      return res;
    }));
  }

  getEmployeeCalendarShifts(data) {
    return this.apiService.post(Constants.getEmployeeCalendarShifts, data).pipe(map(res => {
      return res;
    }));
  }
}
