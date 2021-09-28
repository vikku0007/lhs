import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { EmpDashboardService } from '../../services/emp-dashboard.service';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { AssignedShift } from '../../viewmodels/assigned-shift';
import { Paging } from 'projects/viewmodels/paging';
import { CurrentshiftComponent } from '../currentshift/currentshift.component';
import { NotificationService } from 'projects/core/src/projects';
import { PageEvent } from '@angular/material/paginator';
import { ShiftInfoViewModel } from '../../viewmodels/shiftlist-viewModel';

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
  shifthoursInfo: ShifthoursInfo = {};
  employeeId: number;
  response: ResponseModel = {};
  assignedModel: AssignedShift[] = [];
  totalCount: number = 0;
  paging: Paging = {};
  @ViewChild(CurrentshiftComponent, { static: false }) childC: CurrentshiftComponent;
  notificationlist: Notificationlist[];
  notificationcount: number;
  tempSelection: string;
  shiftList: ShiftInfoViewModel[];
  dataSource: any;
  shiftCount = 0;
  displayedColumnsRequired: string[] = ['description', 'date', 'time', 'duration', 'client', 'staff', 'location', 'status', 'action'];
  @ViewChild('btnCancel') btnCancel: ElementRef;

  constructor(private route: ActivatedRoute, private empService: EmpDashboardService, private notificationService: NotificationService) {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
    this.route.paramMap.subscribe((params: any) => {
      this.employeeId = params.params.id;
    });
  }

  ngOnInit(): void {
    this.dataSourceMultitabs.sort = this.sort;
    this.getAssignedShifts(0, 10);
    this.getEmployeeShiftHours();
    this.showEmployeeNotification();
  }

  getAssignedShifts(pageNo, pageSize) {
    const data = {
      id: Number(this.employeeId),
      pageSize: pageSize,
      pageNo: pageNo,
      orderBy: 1,
      sortOrder: 0
    };

    this.empService.getAssignedShifts(data).subscribe(res => {
      this.response = res;
      switch (this.response.status) {
        case 1:
          this.assignedModel = this.response.responseData;
          this.totalCount = this.response.total;
          break;

        default:
          break;
      }
    });

  }

  updatePagingAssignedShift(paging: Paging) {
    this.getAssignedShifts(paging.pageNo, paging.pageSize);
  }
  getEmployeeShiftHours() {
    var EmployeeId = Number(this.employeeId)
    this.empService.getEmployeeshifthours(EmployeeId).subscribe(res => {
      this.response = res;
      if (this.response.responseData) {
        this.shifthoursInfo = this.response.responseData;
      }
    });
  }

  updateCurrentShift(data: any) {
    this.childC.getCurrentShift();
    this.getEmployeeShiftHours();
    this.showEmployeeNotification();
  }

  updateAlert(data) {
    this.getEmployeeShiftHours();
    this.showEmployeeNotification();
  }

  updateAssignedShift(data: any) {
    this.getAssignedShifts(0, 10);
  }
  showEmployeeNotification() {
    this.empService.getEmployeeNotification(this.employeeId).subscribe(res => {
      this.response = res;
      if (this.response.responseData) {
        this.notificationlist = this.response.responseData;
        this.notificationcount = this.notificationlist.length > 0 ? this.notificationlist.length : 0;
      }
    });
  }
  ClearAll() {
    const data = {
      IsAdmin: false,
      EmployeeId: Number(this.employeeId)
    }
    this.empService.UpdateNotification(data).subscribe(res => {
      this.response = res;
      switch (this.response.status) {
        case 1:
          this.showEmployeeNotification();
        default:
          this.notificationService.Warning({ message: this.response.message, title: null });
          break;
      }
    });
  }
  getShiftsList(selection: string) {
    this.shiftList = [];
    this.dataSource = new MatTableDataSource(this.shiftList);
    var statusId = 0;
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
    this.tempSelection = selection;
    switch (selection) {
      case 'completedHours':
        statusId = 2324;
        break;
      case 'rejectedHours':
        statusId = 1098;
        break;
      case 'approvedShifts':
        statusId = 1100;
        break;
      case 'pendingShifts':
        statusId = 1095;
        break;
      default:
        statusId = 0;
        break;
    }

    const data = {
      employeeId: Number(this.employeeId),
      orderBy: 3,
      searchTextByManualAddress: null,
      searchTextByStatus: statusId,
      searchTextBylocation: 0,
      sortOrder: 1,
      pageNo: this.paging.pageNo,
      pageSize: this.paging.pageSize
    };
    this.empService.GetEmployeeShiftList(data).subscribe(res => {
      this.response = res;
      this.shiftCount = this.response.total;
      switch (this.response.status) {
        case 1:
          this.shiftList = this.response.responseData;
          this.dataSource = new MatTableDataSource(this.shiftList);
          break;
        case 0:
          // this.notificationService.Warning({ message: this.response.message, title: null });
          break;
        default:
          break;
      }
    });
  }

  PageIndexEvent(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.getShiftsList(this.tempSelection);
  }

  cancelModal() {
    this.btnCancel.nativeElement.click();
  }
}

export interface ShifthoursInfo {
  approvedShifts?: number;
  cancelHours?: number;
  pendingShifts?: number;
  totalHours?: number;
  unallocatedHours?: number;
  completedHours?: number;
}
export interface Notificationlist {
  employeeName?: string;
  description?: string;
  eventName?: Date;
  createdDate?: Date;

}