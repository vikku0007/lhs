import { Component, OnInit, ViewChild } from '@angular/core';
import { DayPilot, DayPilotSchedulerComponent } from 'daypilot-pro-angular';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { TimesheetService } from '../../services/timesheet.service';

@Component({
  selector: 'lib-hourly-timesheet',
  templateUrl: './hourly-timesheet.component.html',
  styleUrls: ['./hourly-timesheet.component.scss']
})
export class HourlyTimesheetComponent implements OnInit {
  config: DayPilot.SchedulerConfig = {
    timeHeaders: [{ "groupBy": "Month" }, { "groupBy": "Day", "format": "d" }],
    scale: "Day",
    cellWidth: 50,
    days: DayPilot.Date.today().daysInMonth(),
    startDate: DayPilot.Date.today().firstDayOfMonth(),
    treeEnabled: true,
    rowHeaderHideIconEnabled: true,
    onBeforeEventRender: args => {
      if (args !== undefined) {        
        args.data.cssClass = "shift-default";
      }
    },
  };
  @ViewChild('scheduler', { static: false }) scheduler: DayPilotSchedulerComponent;
  employeesList: any;
  employees: Employees = { id: '' };
  employeeArray: Employees[] = [];
  timeSheet: TimeSheet = {};
  timeSheetArray: TimeSheet[] = [];
  events: any[] = [];
  responseModel: ResponseModel = {};
  runningMonth: string;
  monthVal: string;
  month: number;
  year: number;

  constructor(private timesheetService: TimesheetService) { }

  ngOnInit(): void {
    this.month = DayPilot.Date.today().getMonth();
    this.year = DayPilot.Date.today().getYear();
    this.getMonth(this.month);
    this.monthVal = this.monthVal + ' ' + this.year;
    this.runningMonth = this.monthVal;
    this.getEmployees();
    this.getTimeSheet();
  }

  getEmployees() {
    this.timesheetService.getEmployees().subscribe(res => {
      if (res) {
        this.responseModel = res;
        switch (this.responseModel.status) {
          case 1:
            this.employeesList = this.responseModel.responseData;
            this.employeesList.forEach(element => {
              this.employees = { id: '' };
              this.employees.id = element.id;
              this.employees.name = element.fullName;
              this.employeeArray.push(this.employees);
            });
            this.config.resources = this.employeeArray;
            break;

          default:
            break;
        }
      }
    });
  }

  getTimeSheet() {
    const data = {
      startDate: this.config.startDate,
      endDate: new DayPilot.Date(this.config.startDate).lastDayOfMonth()
    }
    this.timesheetService.getHourlyTimeSheet(data).subscribe(res => {
      this.responseModel = res;
      this.timeSheetArray = [];
      switch (this.responseModel.status) {
        case 1:
          let index = 1;
          this.responseModel.responseData.forEach(element => {
            this.timeSheet = {};
            this.timeSheet.id = index;
            this.timeSheet.start = element.startDate;
            this.timeSheet.end = element.endDate;
            this.timeSheet.shiftId = element.shiftId;
            this.timeSheet.resource = element.employeeId;
            this.timeSheet.duration = element.duration;
            this.timeSheet.text = element.duration;
            this.timeSheet.startTime = element.startTime;
            this.timeSheet.endTime = element.endTime;
            this.timeSheetArray.push(this.timeSheet);
            index++;
          });
          this.events = this.timeSheetArray;
          this.getMonth(this.month);
          this.monthVal = this.monthVal + ' ' + this.year;
          this.runningMonth = this.monthVal;
          break;

        default:
          break;
      }
    });
  }

  getMonth(monthNo: number) {
    switch (monthNo) {
      case 0:
        this.monthVal = 'January';
        break;
      case 1:
        this.monthVal = 'February';
        break;
      case 2:
        this.monthVal = 'March';
        break;
      case 3:
        this.monthVal = 'April';
        break;
      case 4:
        this.monthVal = 'May';
        break;
      case 5:
        this.monthVal = 'June';
        break;
      case 6:
        this.monthVal = 'July';
        break;
      case 7:
        this.monthVal = 'August';
        break;
      case 8:
        this.monthVal = 'September';
        break;
      case 9:
        this.monthVal = 'October';
        break;
      case 10:
        this.monthVal = 'November';
        break;
      case 11:
        this.monthVal = 'December';
        break;
      default:
        break;
    }
  }

  previousMonth() {
    this.config.startDate = new DayPilot.Date(this.config.startDate).addMonths(-1);
    this.config.days = this.config.startDate.daysInMonth();
    if (this.month >= 0) {
      this.month--;
    }
    if (this.month == -1) {
      this.month = 11;
      this.year--;
    }
    this.getTimeSheet()
  }

  thisMonth() {
    this.config.startDate = DayPilot.Date.today().firstDayOfMonth()
    this.config.days = this.config.startDate.daysInMonth();
    this.month = new Date().getMonth();
    this.year = new Date().getFullYear();
    this.getTimeSheet()
  }

  nextMonth() {
    this.config.startDate = new DayPilot.Date(this.config.startDate).addMonths(+1);
    this.config.days = this.config.startDate.daysInMonth()
    if (this.month <= 11) {
      this.month++;
    }
    if (this.month == 12) {
      this.month = 0;
      this.year++;
    }
    this.getTimeSheet()
  }

}

interface Employees {
  id: string | number;
  name?: string;
}

interface TimeSheet {
  id?: string | number;
  shiftId?: string | number;
  text?: string;
  statusId?: number;
  statusName?: string;
  resource?: number;
  start?: Date;
  end?: Date;
  duration?: number;
  startTime?: string;
  endTime?: string;
  isAccepted?: boolean;
  isSleepover?: boolean;
  isHoliday?: boolean;
  isActiveNight?: boolean;
}