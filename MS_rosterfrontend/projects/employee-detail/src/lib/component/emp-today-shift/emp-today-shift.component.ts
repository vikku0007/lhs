import { Component, OnInit, Input, ViewChild, ElementRef, OnChanges, SimpleChanges } from '@angular/core';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { ActivatedRoute } from '@angular/router';
import { NotificationService } from 'projects/core/src/lib/services/notification-service/notification.service';
import { CommonService } from 'projects/lhs-service/src/lib/service/common/common.service';
import { ShiftInfoViewModel } from 'projects/roster/src/lib/viewmodel/roster-shift-info-viewModel';
import { EmpDetailService } from '../../services/emp-detail.service';
import { Paging } from 'projects/viewmodels/paging';
import { PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'lib-emp-today-shift',
  templateUrl: './emp-today-shift.component.html',
  styleUrls: ['./emp-today-shift.component.scss']
})
export class EmpTodayShiftComponent implements OnInit {
  employeeId: number;
  response: ResponseModel = {};
  shiftList: ShiftInfoViewModel [];
  startDate: string;
  endDate: string;
  duration: number;
  LocationName: string;
  clientList: any;
  startTime: string;
  endTime: string;
  paging: Paging = {};
  displayedColumnsshift: string[] = ['startDate',  'endDate', 'location','duration', 'client'];
  totalCount: number;
  dataSource: any;
 
  constructor(private route: ActivatedRoute,private commonService: CommonService, 
    private fb: FormBuilder, private empService: EmpDetailService, 
    private notificationService: NotificationService) { 
      this.paging.pageNo = 1;
      this.paging.pageSize = 10;
    }

  ngOnInit(): void {
    this.route.paramMap.subscribe((params: any) => {
      this.employeeId = Number(params.params.id);
    });
    this.GetEmployeeShiftInfo();
  }
  GetEmployeeShiftInfo() {
    const data = {
      EmployeeId:this.employeeId,
      
    };
     this.empService.GetEmployeeTodayShift(data).subscribe(res => {
      this.response = res;
      this.totalCount = this.response.total;
      switch (this.response.status) {
        case 1:
          this.shiftList = this.response.responseData;
          this.dataSource = new MatTableDataSource(this.shiftList);
          break;
        case 0:
          this.notificationService.Warning({ message: this.response.message, title: null });
          this.dataSource = new MatTableDataSource([]);
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
    this.GetEmployeeShiftInfo();
  }
}
