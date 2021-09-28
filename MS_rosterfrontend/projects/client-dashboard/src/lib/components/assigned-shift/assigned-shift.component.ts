import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { ShiftInfoViewModel, ShiftDetailViewModel } from '../../view-models/shift-info-view-model';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { ClientService } from '../../services/client.service';
import { ActivatedRoute } from '@angular/router';
import { PageEvent } from '@angular/material/paginator';
import { Paging } from 'projects/viewmodels/paging';
import { FormGroup, Validators, FormBuilder, NgForm } from '@angular/forms';
import { NotificationService, MembershipService } from 'projects/core/src/projects';
import { Shiftviewmodel } from 'projects/employee-roster/src/lib/view-models/shiftviewmodel';

@Component({
  selector: 'lib-assigned-shift',
  templateUrl: './assigned-shift.component.html',
  styleUrls: ['./assigned-shift.component.scss']
})
export class AssignedShiftComponent implements OnInit {
  clientId: number = 0;
  totalCount: number = 0;
  responseModel: ResponseModel = {};
  shiftModel: ShiftInfoViewModel[] = [];
  displayedColumnsRequired: string[] = ['sr', 'employeeName', 'location', 'startDate', 'endDate', 'startTime', 'endTime', 'action'];
  dataSourceRequired = new MatTableDataSource(this.shiftModel);
  paging: Paging = {};
  rForm: FormGroup;
  tempShiftId?: number;
  @ViewChild('btnCancel') cancel: ElementRef;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild('formDirective') formDirective: NgForm;  

  constructor(private clientService: ClientService, private route: ActivatedRoute, private fb: FormBuilder,
    private notificationService: NotificationService, private membershipService: MembershipService) {
    this.route.paramMap.subscribe((params: any) => {
      this.clientId = Number(params.params.id);
    });
  }

  ngOnInit(): void {
    this.dataSourceRequired.sort = this.sort;
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
    this.createForm();
    this.getClientAssignedShiftList();
  }

  createForm() {
    this.rForm = this.fb.group({
      remark: ['', Validators.required],
    });
  }

  get remark() {
    return this.rForm.get('remark');
  }

  getClientAssignedShiftList() {
    const data = {
      id: this.clientId,
      pageNo: this.paging.pageNo,
      pageSize: this.paging.pageSize
    }
    this.clientService.getAssignedShiftList(data).subscribe(res => {
      this.responseModel = res;
      switch (this.responseModel.status) {
        case 1:
          this.shiftModel = this.responseModel.responseData;
          this.totalCount = this.responseModel.total;
          this.dataSourceRequired = new MatTableDataSource(this.shiftModel);
          break;

        default:
          break;
      }
    })
  }

  PageIndexEvent(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.getClientAssignedShiftList();
  }

  showModalRejectShift(shift: Shiftviewmodel) {
    this.tempShiftId = shift.id;
  }

  cancelShift() {
    if (this.rForm.valid) {
      const data = {
        id: this.tempShiftId,
        remark: this.rForm.controls['remark'].value,
        clientId: this.membershipService.getUserDetails('employeeId')
      };
      this.clientService.cancelShift(data).subscribe(res => {
        this.responseModel = res;
        switch (this.responseModel.status) {
          case 1:
            this.notificationService.Success({ message: 'Shift cancelled successfully', title: '' });
            this.getClientAssignedShiftList();
            this.cancel.nativeElement.click();
            this.formDirective.resetForm();
            this.rForm.reset();
            break;

          default:
            break;
        }
      });
    } else {
      this.notificationService.Warning({ message: 'Remark is missing', title: '' });
    }
  }

  cancelForm() {
    this.rForm.reset();
    this.formDirective.resetForm();
  }

}
