import { Component, OnInit, Input, ViewChild, ElementRef, OnChanges, SimpleChanges } from '@angular/core';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { ActivatedRoute } from '@angular/router';
import { NotificationService } from 'projects/core/src/lib/services/notification-service/notification.service';
import * as moment from 'moment';
import { MatSort } from '@angular/material/sort';
import { DashboardService } from 'projects/dashboard/src/projects';
import { MainDashboardService } from '../../services/main-dashboard.service';
import { MatTableDataSource } from '@angular/material/table';
import { Label, MultiDataSet, Color } from 'ng2-charts';
import { ChartType } from 'chart.js';
import { Paging } from 'projects/viewmodels/paging';
import { ShiftInfoViewModel } from 'projects/roster/src/lib/viewmodel/roster-shift-info-viewModel';
import { PageEvent } from '@angular/material/paginator';
import { CommonService } from 'projects/lhs-service/src/projects';

export interface Multitabs {
  name: string;
  shiftId: string;
  location: string;
  date: string;
}

const MULTITABS_DATA: Multitabs[] = [
  { name: 'John Doe', shiftId: '456786', location: 'Sydney', date: 'Mon End - 16:30' },
  { name: 'Kevin A. Miller', shiftId: '789456', location: 'Melbourne', date: 'Tue End - 16:30' },
  { name: 'Lee R. Jordan', shiftId: '874751', location: 'Perth', date: 'Wed End - 16:30' },
];

@Component({
  selector: 'lib-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  displayedColumnsMultitabs: string[] = ['sr', 'name', 'shiftId', 'location', 'date'];
  dataSourceMultitabs = new MatTableDataSource(MULTITABS_DATA);
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild('btnShowAll') ShowAll: ElementRef;
  @ViewChild('btnWeek') Week: ElementRef;
  @ViewChild('btnToday') Today: ElementRef;
  @ViewChild('btnMonthTiming') MonthTiming: ElementRef;
  @ViewChild('btnWeekTiming') WeekTiming: ElementRef;
  @ViewChild('btnTodayTiming') TodayTiming: ElementRef;
  response: ResponseModel = {};
  todayDatemax = new Date();
  StarDate: any;
  shifthoursInfo: ShifthoursInfo = {};
  shifttimingInfo: ShifttimingInfo = {};
  ShiftStatusdata: ShiftStatusInfo[];
  public doughnutChartColors: Color[] = [{
    backgroundColor: ['#CB4335', '#F39C12']
  }];
  doughnutChartLabels: Label[] = ['Staff on time %', 'Staff late %'];
  public doughnutChartData: number[] = [];
  doughnutChartType: ChartType = 'doughnut';
  scheduledlist: ShiftStatusInfo[];
  Finishedlist: ShiftStatusInfo[];
  OnShiftlist: ShiftStatusInfo[];
  notificationlist: Notificationlist[];
  schedulecount: any;
  finishcount: any;
  onshitcount: any;
  isShownChart: boolean = false;
  paging: Paging = {};
  orderBy: number;
  orderColumn: number;
  notificationcount: number;
  defaultSelection: string;
  shiftList: ShiftInfoViewModel[];
  dataSource: any;
  displayedColumnsRequired: string[] = ['description', 'date', 'time', 'duration', 'client', 'staff', 'location', 'status', 'action'];
  totalCount: number = 0;
  tempSelection = null;
  @ViewChild('btnCancel') btnCancel: ElementRef;
  rForm: FormGroup;
  shiftStatusList: [];

  constructor(private route: ActivatedRoute, private dashboardService: MainDashboardService,
    private notificationService: NotificationService, private fb: FormBuilder, private commonService: CommonService) {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
  }

  ngOnInit(): void {
    this.dataSourceMultitabs.sort = this.sort;
    this.showShiftHoursLoad();
    this.ShowTodayTimingLoad();
    this.getScheduledShiftDetails();
    this.showNotification();
    this.getShiftStatusList();
    this.createForm();
  }

  createForm() {
    this.rForm = this.fb.group({
      shiftStatus: [null, Validators.nullValidator],
      startDate: [null, Validators.nullValidator],
      endDate: [null, Validators.nullValidator]
    });
  }

  get shiftStatus() {
    return this.rForm.get('shiftStatus');
  }

  get startDate() {
    return this.rForm.get('startDate');
  }

  get endDate() {
    return this.rForm.get('endDate');
  }

  getShiftStatusList() {
    this.commonService.getShiftStatusList().subscribe((res => {
      if (res) {
        this.response = res;
        this.shiftStatusList = this.response.responseData || [];

      } else {
        this.notificationService.Error({ message: 'Something went wrong! location not found', title: null });
      }
    }));
  }

  showallshifthours() {
    this.defaultSelection = 'all';
    this.ShowAll.nativeElement.className = 'nav-item active';
    this.Week.nativeElement.className = 'nav-item';
    this.Today.nativeElement.className = 'nav-item';
    var thisYear = (new Date()).getFullYear();
    var start = new Date("1/1/" + thisYear);
    var defaultStart = moment(start.valueOf());
    var EndDate = moment(this.todayDatemax).format('YYYY-MM-DD');
    var StartDate = moment(defaultStart).format('YYYY-MM-DD').toString();
    this.dashboardService.getshifthours(StartDate, EndDate).subscribe(res => {
      this.response = res;
      if (this.response.responseData) {
        this.shifthoursInfo = this.response.responseData;
      }
    });
  }
  showweekshifthours() {
    this.defaultSelection = 'week';
    this.ShowAll.nativeElement.className = 'nav-item';
    this.Week.nativeElement.className = 'nav-item active';
    this.Today.nativeElement.className = 'nav-item';
    var EndDate = moment(this.todayDatemax).format('YYYY-MM-DD');
    var StartDate = moment(this.todayDatemax).subtract(7, 'day').format('YYYY-MM-DD').toString();
    this.dashboardService.getshifthours(StartDate, EndDate).subscribe(res => {
      this.response = res;
      if (this.response.responseData) {
        this.shifthoursInfo = this.response.responseData;
      }
    });
  }
  showShiftHourstoday() {
    this.defaultSelection = 'today';
    this.ShowAll.nativeElement.className = 'nav-item';
    this.Week.nativeElement.className = 'nav-item';
    this.Today.nativeElement.className = 'nav-item active';
    var StartDate = moment(this.todayDatemax).format('YYYY-MM-DD');
    this.dashboardService.getshifthours(StartDate, StartDate).subscribe(res => {
      this.response = res;
      if (this.response.responseData) {
        this.shifthoursInfo = this.response.responseData;

      }
    });
  }
  showShiftHoursLoad() {
    var StartDate = moment(this.todayDatemax).format('YYYY-MM-DD');
    this.defaultSelection = 'today';
    this.dashboardService.getshifthours(StartDate, StartDate).subscribe(res => {
      this.response = res;
      if (this.response.responseData) {
        this.shifthoursInfo = this.response.responseData;
      }
    });
  }

  ShowMonthTiming() {
    this.MonthTiming.nativeElement.className = 'nav-item active';
    this.WeekTiming.nativeElement.className = 'nav-item';
    this.TodayTiming.nativeElement.className = 'nav-item';
    var StartDate = moment(this.todayDatemax).format('YYYY-MM-DD');
    var EndDate = moment(this.todayDatemax).format('YYYY-MM-DD');
    var StartDate = moment(this.todayDatemax).subtract(30, 'day').format('YYYY-MM-DD').toString();
    this.dashboardService.getDashboardShiftTimeStatus(StartDate, EndDate).subscribe(res => {
      this.response = res;
      if (this.response.responseData) {
        this.shifttimingInfo = this.response.responseData;
        if (this.shifttimingInfo.shiftOnTimePercentage > 0 || this.shifttimingInfo.shiftLatePercentage > 0) {
          this.doughnutChartData = [this.shifttimingInfo.shiftOnTimePercentage, this.shifttimingInfo.shiftLatePercentage];
          this.isShownChart = true;
        }
        else {
          this.isShownChart = false;
        }
      }
    });
  }
  ShowWeekTiming() {
    this.MonthTiming.nativeElement.className = 'nav-item';
    this.WeekTiming.nativeElement.className = 'nav-item active';
    this.TodayTiming.nativeElement.className = 'nav-item';
    var EndDate = moment(this.todayDatemax).format('YYYY-MM-DD');
    var StartDate = moment(this.todayDatemax).subtract(7, 'day').format('YYYY-MM-DD').toString();
    this.dashboardService.getDashboardShiftTimeStatus(StartDate, EndDate).subscribe(res => {
      this.response = res;
      if (this.response.responseData) {
        this.shifttimingInfo = this.response.responseData;
        if (this.shifttimingInfo.shiftOnTimePercentage > 0 || this.shifttimingInfo.shiftLatePercentage > 0) {
          this.doughnutChartData = [this.shifttimingInfo.shiftOnTimePercentage, this.shifttimingInfo.shiftLatePercentage];
          this.isShownChart = true;
        }
        else {
          this.isShownChart = false;
        }
      }
    });
  }
  ShowTodayTiming() {
    this.MonthTiming.nativeElement.className = 'nav-item';
    this.WeekTiming.nativeElement.className = 'nav-item';
    this.TodayTiming.nativeElement.className = 'nav-item active';
    var StartDate = moment(this.todayDatemax).format('YYYY-MM-DD');
    this.dashboardService.getDashboardShiftTimeStatus(StartDate, StartDate).subscribe(res => {
      this.response = res;
      if (this.response.responseData) {
        this.shifttimingInfo = this.response.responseData;
        if (this.shifttimingInfo.shiftOnTimePercentage > 0 || this.shifttimingInfo.shiftLatePercentage > 0) {
          this.doughnutChartData = [this.shifttimingInfo.shiftOnTimePercentage, this.shifttimingInfo.shiftLatePercentage];
          this.isShownChart = true;
        }
        else {
          this.isShownChart = false;
        }
      }
    });
  }
  ShowTodayTimingLoad() {
    var StartDate = moment(this.todayDatemax).format('YYYY-MM-DD');
    this.dashboardService.getDashboardShiftTimeStatus(StartDate, StartDate).subscribe(res => {
      this.response = res;
      if (this.response.responseData) {
        this.shifttimingInfo = this.response.responseData;
        if (this.shifttimingInfo.shiftOnTimePercentage > 0 || this.shifttimingInfo.shiftLatePercentage > 0) {
          this.doughnutChartData = [this.shifttimingInfo.shiftOnTimePercentage, this.shifttimingInfo.shiftLatePercentage];
          this.isShownChart = true;
        }
        else {
          this.isShownChart = false;
        }
      }
    });
  }
  getScheduledShiftDetails() {
    this.dashboardService.getSchedulesShiftAdminDashboard().subscribe(res => {
      this.response = res;
      if (this.response.responseData) {
        this.scheduledlist = this.response.responseData;
        this.schedulecount = this.scheduledlist.length;
        // make other list empty
        this.OnShiftlist = [];
        this.onshitcount = 0;
        this.finishcount = 0;
        this.Finishedlist = [];
      }

    });
  }
  getFinishedShiftDetails() {
    this.dashboardService.getFinishedShiftDetails().subscribe(res => {
      this.response = res;
      if (this.response.responseData) {
        this.Finishedlist = this.response.responseData;
        this.finishcount = this.Finishedlist.length;
        // Make other list empty
        this.scheduledlist = [];
        this.schedulecount = 0;
        this.onshitcount = 0;
        this.OnShiftlist = [];
      }
    });
  }
  getOnShiftDetails() {
    this.dashboardService.getOnShiftDetails().subscribe(res => {
      this.response = res;
      if (this.response.responseData) {
        this.OnShiftlist = this.response.responseData;
        this.onshitcount = this.OnShiftlist.length;
        // Make other list empty
        this.finishcount = 0;
        this.Finishedlist = [];
        this.schedulecount = 0;
        this.scheduledlist = [];
      }
    });
  }
  showNotification() {
    // this.getSortingOrder();
    debugger;
    const data = {
      pageNo: this.paging.pageNo,
      pageSize: this.paging.pageSize,
      OrderBy: 0,
      SortOrder: 1
    };
    this.dashboardService.getAdminNotification(data).subscribe(res => {
      this.response = res;
      if (this.response.responseData) {
        this.notificationlist = this.response.responseData;
        this.notificationcount = this.notificationlist.length > 0 ? this.notificationlist.length : 0;
      }
    });
  }
  ClearAll() {
    const data = {
      IsAdmin: false
    }
    this.dashboardService.UpdateNotification(data).subscribe(res => {
      this.response = res;
      switch (this.response.status) {
        case 1:
          this.showNotification();
        default:
          this.notificationService.Warning({ message: this.response.message, title: null });
          break;
      }
    });
  }

  cancelModal() {
    this.btnCancel.nativeElement.click();
  }

  getShiftsList(selection: string) {
    debugger;
    // this.ClearFilter();
    this.shiftList = [];
    this.dataSource = new MatTableDataSource(this.shiftList);
    var statusId = 0;
    // this.paging.pageNo = 1;
    // this.paging.pageSize = 10;
    this.tempSelection = selection;
    switch (selection) {
      case 'bookedHours':
        statusId = 1097;
        break;
      case 'cancelShifts':
        statusId = 1098;
        break;
      case 'pendingShifts':
        statusId = 1095;
        break;
      default:
        statusId = 0;
        break;
    }
    var startDate;
    var endDate;
    switch (this.defaultSelection) {
      case 'today':
        startDate = moment(this.todayDatemax).format('YYYY-MM-DD');
        endDate = startDate;
        break;
      case 'week':
        endDate = moment(this.todayDatemax).format('YYYY-MM-DD');
        startDate = moment(this.todayDatemax).subtract(7, 'day').format('YYYY-MM-DD').toString();
        break;
      case 'all':
        startDate = null;
        endDate = null;
        break;
      default:
        break;
    }

    if (this.startDate.value != null || this.endDate.value != null) {
      this.paging.pageNo = 0;
      this.paging.pageSize = 10;
    }

    const data = {
      SearchByClientName: 0,
      SearchByEmpName: 0,
      SearchByStartDate: startDate != null ? startDate : this.startDate.value,
      SearchByEndDate: endDate != null ? endDate : this.endDate.value,
      SearchTextByManualAddress: null,
      SearchTextByShiftType: 0,
      SearchTextByStatus: this.shiftStatus.value ? this.shiftStatus.value : statusId,
      SearchTextBylocation: 0,
      pageNo: this.paging.pageNo,
      pageSize: this.paging.pageSize
    };
    this.dashboardService.getShiftList(data).subscribe(res => {
      this.response = res;
      this.totalCount = this.response.total;
      switch (this.response.status) {
        case 1:
          this.shiftList = this.response.responseData;
          this.dataSource = new MatTableDataSource(this.shiftList);
          break;
        case 0:
          // this.notificationService.Warning({ message: this.response.message, title: null });
          break;
        default:
          // this.notificationService.Error({ message: 'Some error occured while fetching employee listing', title: null });
          break;
      }
    });
  }

  PageIndexEvent(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.getShiftsList(this.tempSelection);
  }

  ClearFilter() {
    this.rForm.reset();
  }
}
export interface ShifthoursInfo {
  bookedHours?: number;
  cancelShifts?: number;
  pendingShifts?: number;
  totalHours?: number;
  unallocatedHours?: number;
}
export interface ShifttimingInfo {
  shiftOnTimePercentage?: number;
  shiftOnTime?: number;
  shiftLatePercentage?: number;
  shiftLate?: number;

}
export interface ShiftStatusInfo {
  employeeName?: string;
  location?: string;
  startDate?: Date;
  endDate?: Date;
  StartTimeString?: string;
  EndTimeString?: string;
}

export interface Notificationlist {
  employeeName?: string;
  description?: string;
  eventName?: Date;
  createdDate?: Date;

}