import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { LogoutService } from '../../services/logout.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { ClientDetails, EmployeeShiftToDo } from '../../view-models/add-client-details';
import { Paging } from 'projects/viewmodels/paging';
import { PageEvent, MatPaginator } from '@angular/material/paginator';
import { NotificationService, MembershipService } from 'projects/core/src/projects';

@Component({
  selector: 'lib-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.scss']
})

export class LogoutComponent implements OnInit {
  response: ResponseModel = {};
  displayedColumnsRequired: string[] = ['employeeName', 'action'];
  displayedColumnsToDo: string[] = ['dateTime', 'shiftTimeString', 'shiftTypeString', 'action'];
  totalCount: number;
  paging: Paging = {};
  isLogoutDisabled: boolean = true;
  clientShiftModel: ClientDetails[] = [];
  employeeShiftToDoModel: EmployeeShiftToDo[] = [];
  dataSourceRequired = new MatTableDataSource(this.clientShiftModel);
  dataSourceRequiredToDo = new MatTableDataSource(this.employeeShiftToDoModel);
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  orderBy: number;
  orderColumn: number;
  employeeId: number;
  shiftId: number;
  Ismessage: number;
  isCheckoutCompleted: boolean = false;

  delete(elm) {
    this.dataSourceRequired.data = this.dataSourceRequired.data.filter(i => i !== elm)
  }

  @ViewChild(MatSort, { static: true }) sort: MatSort;

  constructor(private logoutService: LogoutService, private route: ActivatedRoute, private router: Router,
    private notificationService: NotificationService, private membershipService: MembershipService) {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe((params: any) => {
      this.shiftId = params.params.id;
    });
    this.employeeId = this.membershipService.getUserDetails('employeeId');
    this.getCurrentShift();
    this.getToDoList();
  }

  getCurrentShift() {
    const data = {
      Id: Number(this.shiftId)
    };
    this.logoutService.getCheckOutClientList(data).subscribe(res => {
      this.response = res;
      this.totalCount = this.response.total;
      switch (this.response.status) {
        case 1:
          this.clientShiftModel = this.response.responseData;
          this.checkIsLogoutDisabled();
          this.dataSourceRequired = new MatTableDataSource(this.clientShiftModel);
          break;
        default:
          break;
      }
    });
  }

  checkIsLogoutDisabled() {
    const index = this.clientShiftModel.findIndex(x => x.isProgressNotesSubmitted == 0);
    const ind = this.clientShiftModel.findIndex(x => x.isToDoListSubmitted == 0);
    this.isCheckoutCompleted = this.clientShiftModel[0].isCheckoutCompleted;
    if (this.isCheckoutCompleted) {
      this.isLogoutDisabled = true;
      this.Ismessage = 4;
    } else {
      if (index > -1 && ind > -1) {
        this.isLogoutDisabled = true;
        this.Ismessage = 1;
      }
      else if (index > -1) {
        this.isLogoutDisabled = true;
        this.Ismessage = 2;
      }
      else if (ind > -1) {
        this.isLogoutDisabled = true;
        this.Ismessage = 3;
      }
      else {
        this.isLogoutDisabled = false;
      }
    }
  }

  showCheckoutNotification() {
    if (this.Ismessage == 1) {
      this.notificationService.Warning({ message: 'Please fill progress notes and to do list before logout!', title: '' });
    }
    else if (this.Ismessage == 2) {
      this.notificationService.Warning({ message: 'Please fill progress notes for all clients before logout!!', title: '' });
    }
    else if (this.Ismessage == 3) {
      this.notificationService.Warning({ message: 'Please fill to do list before logout!', title: '' });
    }
    else if (this.Ismessage == 4) {
      this.notificationService.Warning({ message: 'You have already logout this shift!', title: '' });
    }
  }

  PageIndexEvent(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.getCurrentShift();
  }

  getSortingOrder() {
    const sortColumn = this.sort.active == undefined ? 'createdDate' : this.sort.active;
    this.orderBy = this.sort.direction === 'asc' ? 0 : 1;
    switch (sortColumn) {
      case 'employeename':
        this.orderColumn = 0;
        break;
      case 'createdDate':
        this.orderColumn = 1;
        break;
      default:
        break;
    }
  }

  getToDoList() {
    const data = {
      employeeId: this.employeeId,
      shiftId: Number(this.shiftId)
    }
    this.logoutService.GetToDoList(data).subscribe(res => {
      this.response = res;
      switch (this.response.status) {
        case 1:
          this.employeeShiftToDoModel = this.response.responseData;
          this.dataSourceRequiredToDo = new MatTableDataSource(this.employeeShiftToDoModel);
          break;

        default:
          break;
      }
    });
  }

}
