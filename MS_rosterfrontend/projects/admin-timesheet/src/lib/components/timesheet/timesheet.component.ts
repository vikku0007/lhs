import { Component, OnInit, ViewChild } from '@angular/core';
import { DayPilotSchedulerComponent, DayPilot } from 'daypilot-pro-angular';
import { TimesheetService } from '../../services/timesheet.service';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FormControl } from '@angular/forms';
import { takeUntil } from 'rxjs/operators';
import { Subject, ReplaySubject } from 'rxjs';

@Component({
  selector: 'lib-timesheet',
  templateUrl: './timesheet.component.html',
  styleUrls: ['./timesheet.component.scss']
})
export class TimesheetComponent implements OnInit {
  @ViewChild('scheduler', { static: false }) scheduler: DayPilotSchedulerComponent;
  events: any[] = [];
  employeesList: any;
  ReportedTosearch: any;
  ServiceData :any;
  response: ResponseModel = {};
  responseModel: ResponseModel = {};
  employees: Employees = { id: '' };
  employeeArray: Employees[] = [];
  tempEmpArray: Employees[] = [];
  timeSheet: TimeSheet = {};
  timeSheetArray: TimeSheet[] = [];
  public searchcontrol: FormControl = new FormControl();
  runningMonth: string;
  monthVal: string;
  private _onDestroy = new Subject<void>();
  public filteredRecords: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
  public filteredRecordsService: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
  month: number;
  year: number;
  show: boolean = false;
  rForm: FormGroup;

  constructor(private timesheetService: TimesheetService, private fb: FormBuilder) { }

  ngOnInit(): void {
    this.month = DayPilot.Date.today().getMonth();
    this.year = DayPilot.Date.today().getYear();
    this.getMonth(this.month);
    this.monthVal = this.monthVal + ' ' + this.year;
    this.runningMonth = this.monthVal;
    this.getEmployees();
    this.getTimeSheet();
    this.createForm();
    this.searchEmployee();
    this.searchservicetype();
  }
  searchservicetype() {
    this.searchcontrol.valueChanges
      .pipe(takeUntil(this._onDestroy))
      .subscribe(() => {
        this.filterServicetype();
      });
  }
  private filterServicetype(){
    if (!this.ServiceData) {
      return;
    }
    let search = this.searchcontrol.value;
    if (!search) {
      this.filteredRecordsService.next(this.ServiceData.slice());
      return;
    } else {
      search = search.toLowerCase();
    }
    if (search.length >= 1) {
      this.filteredRecordsService.next(
        this.ServiceData.filter(ServiceType => ServiceType.supportItemName.toLowerCase().indexOf(search) > -1)
      );
    }

  }
  searchEmployee() {
    this.searchcontrol.valueChanges
      .pipe(takeUntil(this._onDestroy))
      .subscribe(() => {
        this.filterEmployee();
      });
  }
  private filterEmployee(){
    if (!this.ReportedTosearch) {
      return;
    }
    let search = this.searchcontrol.value;
    if (!search) {
      this.filteredRecords.next(this.employeesList.slice());
      return;
    } else {
      search = search.toLowerCase();
    }
    if (search.length >= 1) {
      this.filteredRecords.next(
        this.employeesList.filter(department => department.fullName.toLowerCase().indexOf(search) > -1)
      );
    }
  }

  createForm() {
    this.rForm = this.fb.group({
      employeeId: [null, Validators.nullValidator]
    });
  }

  config: DayPilot.SchedulerConfig = {
    timeHeaders: [{ "groupBy": "Month" }, { "groupBy": "Day", "format": "d" }],
    scale: "Day",
    cellWidth: 50,
    days: DayPilot.Date.today().daysInMonth(),
    startDate: DayPilot.Date.today().firstDayOfMonth(),
    treeEnabled: true,
    rowHeaderHideIconEnabled: true,
    bubble: new DayPilot.Bubble({
      onLoad: args => {
        let event = args.source;
        args.html = "<p><b>Start Date : </b>" + event.data.start.split('T')[0] + "</p>";
        args.html += "<p><b>End Date : </b>" + event.data.end.split('T')[0] + "</p>";
        args.html += "<p><b>Start Time : </b>" + event.data.startTime + "</p>";
        args.html += "<p><b>End Time : </b>" + event.data.endTime + "</p>";
        args.html += "<p><b>Duration : </b>" + event.data.customDuration + "</p>";
      }
    }),
    onBeforeEventRender: args => {
      if (args !== undefined) {
        args.data.cssClass = "shift-default";
      }
    },

  };

  getEmployees() {
    this.timesheetService.getEmployees().subscribe(res => {
      if (res) {
        this.responseModel = res;
        switch (this.responseModel.status) {
          case 1:
            this.employeesList = this.responseModel.responseData;
             //Added By DeepakBisht
        this.ReportedTosearch = this.response.responseData || [];
        this.filteredRecords.next(this.employeesList.slice());
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
    this.timesheetService.getTimeSheet(data).subscribe(res => {
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
            this.timeSheet.text = element.startTimeString + '-' + element.endTimeString;
            this.timeSheet.startTime = element.startTimeString;
            this.timeSheet.endTime = element.endTimeString;
            this.timeSheet.customDuration = element.customDuration;
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

  toggleFilters() {
    this.show = !this.show;
  }

  searchTimesheet() {
    if (this.rForm.valid) {
      this.tempEmpArray = this.employeeArray.filter(x => x.id == this.rForm.controls['employeeId'].value);
      this.config.resources = this.tempEmpArray;
    }
  }

  ClearFilter() {
    this.rForm.reset();
    this.config.resources = this.employeeArray;
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
  customDuration?: string;
}