import { Injectable } from '@angular/core';
import { ApiService } from 'projects/core/src/projects';
import { Constants } from '../config/constants';
import { map } from 'rxjs/operators';
import { DayPilot } from 'daypilot-pro-angular';
import { Observable } from 'rxjs';
import { HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class RosterService {

  // resources: any[] = [
  //   { name: '<h2>John Doe</h2>', id: 'R1' },
  //   { name: '<h2>Petey Cruiser</h2>', id: 'R2' },
  //   { name: '<h2>Anna Sthesia</h2>', id: 'R3' },
  //   { name: '<h2>Paul Molive</h2>', id: 'R4' },
  //   { name: '<h2>Gail Forcewind</h2>', id: 'R5' },
  //   { name: '<h2>John Doe</h2>', id: 'R6' },
  //   { name: '<h2>Petey Cruiser</h2>', id: 'R7' },
  //   { name: '<h2>Anna Sthesia</h2>', id: 'R8' },
  // ]
  resources: any[] = [
    {
      name: 'Section A', id: 'GA', expanded: true, children: [
        { name: 'Room A.1', id: 'R1' },
        { name: 'Room A.2', id: 'R2' },
        { name: 'Room A.3', id: 'R3' },
        { name: 'Room A.4', id: 'R4' }
      ]
    },
    {
      name: 'Section B', id: 'GB', expanded: true, children: [
        { name: 'Room B.5', id: 'R5' },
        { name: 'Room B.6', id: 'R6' },
        { name: 'Room B.7', id: 'R7' },
        { name: 'Room B.8', id: 'R8' }
      ]
    }
  ];

  events: any[] = [
    {
      id: '1',
      resource: 'R1',
      start: '2020-08-21',
      end: '2020-08-21',
      text: 'Meeting 1',
      color: '#e69138'
    },
    {
      id: '2',
      resource: 'R3',
      start: '2020-08-21',
      end: '2020-08-21',
      text: 'Meeting 2',
      color: '#6aa84f'
    },
    {
      id: '3',
      resource: 'R3',
      start: '2020-08-21',
      end: '2020-08-21',
      text: 'Meeting 3',
      color: '#3c78d8'
    }
  ];

  constructor(private apiService: ApiService) { }



  login(data) {
    return this.apiService.post(Constants.testCall, data, null).pipe(map(res => {
      return res;
    }))
  }


  getEvents(from: DayPilot.Date, to: DayPilot.Date): Observable<any[]> {

    // simulating an HTTP request
    return new Observable(observer => {
      setTimeout(() => {
        observer.next(this.events);
      }, 200);
    });

    // return this.http.get("/api/events?from=" + from.toString() + "&to=" + to.toString());
  }

  getResources(): Observable<any[]> {

    // simulating an HTTP request
    return new Observable(observer => {
      setTimeout(() => {
        observer.next(this.resources);
      }, 200);
    });

    // return this.http.get("/api/resources");
  }

  createEvent(data: CreateEventParams): Observable<EventData> {
    let e: EventData = {
      start: data.start,
      end: data.end,
      resource: data.resource,
      id: DayPilot.guid(),
      text: data.text
    };

    // simulating an HTTP request
    return new Observable(observer => {
      setTimeout(() => {
        observer.next(e);
      }, 200);
    });

    //return this.http.post("/api/events/create", data);
  }

  addShift(data) {
    return this.apiService.post(Constants.addShift, data, null).pipe(map(res => {
      return res;
    }));
  }
  GetShiftList(data) {
    return this.apiService.post(Constants.getAllShiftList, data, null).pipe(map(res => {
      return res;
    }));
  }
  GetShiftDetail(Id: number) {
    return this.apiService.get(Constants.getAllShiftInfo + '?Id=' + Id, null).pipe(map(res => {
      return res;
    }));
  }
  UpdateShift(data) {
    return this.apiService.post(Constants.UpdateShiftInfo, data, null).pipe(map(res => {
      return res;
    }));
  }
  GetEmployeeList() {
    return this.apiService.get(Constants.GetEmployeeList).pipe(map(res => {
      return res;
    }));
  }

  GetEmployeeViewCalendar(data) {
    return this.apiService.post(Constants.GetEmployeeViewCalendar, data, null).pipe(map(res => {
      return res;
    }));
  }
  DeleteShiftInfo(id) {
    return this.apiService.post(Constants.DeleteShiftInfo, { Id: id }, null).pipe(map(res => {
      return res;
    }));
  }

  DragDropShift(data: any) {
    return this.apiService.post(Constants.DragDropShift, data, null).pipe(map(res => {
      return res;
    }));
  }

  AddCopyPasteShift(data) {
    return this.apiService.post(Constants.AddCopyPasteShift, data, null).pipe(map(res => {
      return res;
    }));
  }

  AddShiftTemplate(data) {
    return this.apiService.post(Constants.AddShiftTemplate, data, null).pipe(map(res => {
      return res;
    }));
  }

  GetShiftTemplate() {
    return this.apiService.get(Constants.GetShiftTemplate, null).pipe(map(res => {
      return res;
    }));
  }

  LoadShiftTemplate(data) {
    return this.apiService.post(Constants.LoadShiftTemplate, data, null).pipe(map(res => {
      return res;
    }));
  }

  GetShiftHistory(data) {
    return this.apiService.post(Constants.getShiftHistory, data, null).pipe(map(res => {
      return res;
    }));
  }

  GetShiftPopOverInfo(data) {
    return this.apiService.post(Constants.getShiftPopOverInfo, data, null).pipe(map(res => {
      return res;
    }));
  }

  GetCustomHours(data) {
    return this.apiService.post(Constants.GetCustomHours, data, null).pipe(map(res => {
      return res;
    }));
  }
}

export interface CreateEventParams {
  start: string;
  end: string;
  text: string;
  resource: string | number;
}

export interface EventData {
  id: string | number;
  start: string;
  end: string;
  text: string;
  resource: string | number;
}
