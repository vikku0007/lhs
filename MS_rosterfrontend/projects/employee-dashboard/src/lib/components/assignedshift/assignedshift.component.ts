import { Component, OnInit, ViewChild, Input, OnChanges, SimpleChanges, ElementRef, createPlatformFactory, Output, EventEmitter } from '@angular/core';
import { MatTableDataSource, MatTable } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { AssignedShift } from '../../viewmodels/assigned-shift';
import { EmpDashboardService } from '../../services/emp-dashboard.service';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { NotificationService } from 'projects/core/src/projects';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { PageEvent } from '@angular/material/paginator';
import { Paging } from 'projects/viewmodels/paging';
import { CommonService } from 'projects/lhs-service/src/projects';
import { CurrentshiftComponent } from '../currentshift/currentshift.component';
import { environment } from 'src/environments/environment';


@Component({
  selector: 'lib-assignedshift',
  templateUrl: './assignedshift.component.html',
  styleUrls: ['./assignedshift.component.scss']
})

export class AssignedshiftComponent implements OnInit, OnChanges {
  shiftModel: AssignedShift[] = [];
  documentcheckList: ClientComplianceDetails[]=[];
  displayedColumnsRequired: string[] = ['sr', 'clientName', 'location', 'startDate', 'endDate', 'startTime', 'endTime', 'action'];
  dataSourceRequired = new MatTableDataSource(this.shiftModel);
  displayedColumnsdocumentdata: string[] = ['documentType','documentName','action'];
  dataSourcedocumentdata = new MatTableDataSource(this.documentcheckList);
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @Input() assignedShiftModel: AssignedShift[];
  @Input() totalCount: number;
  response: ResponseModel = {};
  rForm: FormGroup;
  shiftStatusList: StatusList[];
  getErrorMessage: any;
  tempShift: AssignedShift = {clientInfo : []};
  @ViewChild('btnCancel') cancel: ElementRef;
  @ViewChild('formDirective') formDirective: NgForm;
  paging: Paging = {};
  @Output() pageAssignedShift = new EventEmitter();
  booked: StatusList;
  reject: StatusList;
  @Output() updateCurrentShift = new EventEmitter();
  @Output() updateAlert = new EventEmitter();
  baseUrl : string = environment.baseUrl;
  
  constructor(private empService: EmpDashboardService, private notificationService: NotificationService, private commonService: CommonService,
    private fb: FormBuilder) {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (this.assignedShiftModel !== undefined && this.assignedShiftModel.length > 0) {
      this.dataSourceRequired = new MatTableDataSource(this.assignedShiftModel);
    }
    
  }

  ngOnInit(): void {
    this.dataSourceRequired.sort = this.sort;
    this.createForm();
    this.getShiftStatusList();
  }

  createForm() {
    this.rForm = this.fb.group({
      remark: ['', Validators.required],
    });
  }

  get remark() {
    return this.rForm.get('remark');
  }
  getShiftStatusList() {
    this.commonService.getShiftStatusList().subscribe((res => {
      if (res) {
        this.response = res;
        this.shiftStatusList = this.response.responseData || [];
        this.booked = this.shiftStatusList.filter(x => x.codeDescription === 'Booked')[0];
        this.reject = this.shiftStatusList.filter(x => x.codeDescription === 'Cancelled')[0];
      } else {
        this.notificationService.Error({ message: 'Something went wrong! Shift not found', title: null });
      }
    }));
  }

  acceptShift(shift: AssignedShift) {
    const data =
    {
      id: shift.id,
      StatusId: this.booked.id,
      EmployeeId: shift.employeeId
    };
    this.empService.updateAcceptStatus(data).subscribe(res => {
      this.response = res;
      switch (this.response.status) {
        case 1:
          this.updateCurrentShift.emit();
          this.notificationService.Success({ message: 'Shift accepted successfully', title: '' });
          const index = this.assignedShiftModel.findIndex(x => x.id == this.response.responseData.shiftId);
          if (index > -1) {
            this.assignedShiftModel[index].isAccepted = true;
          }
          this.updateAlert.emit();

          break;

        default:
          break;
      }
    });
  }

  showModalRejectShift(shift: AssignedShift) {
    console.log('shift:' + shift);
    this.tempShift = shift;
  }

  rejectShift() {
    if (this.rForm.valid) {
      const data = {
        id: this.tempShift.id,
        remark: this.rForm.controls['remark'].value,
        StatusId: this.reject.id
      };
      this.empService.updateRejectStatus(data).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
            this.notificationService.Success({ message: 'Shift rejected successfully', title: '' });
            const index = this.assignedShiftModel.findIndex(x => x.id == this.response.responseData.shiftId);
            if (index > -1) {
              this.assignedShiftModel[index].isRejected = true;
            }
            this.cancel.nativeElement.click();
            this.formDirective.resetForm();
            this.rForm.reset();
            this.updateAlert.emit();
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

  PageIndexEvent(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.pageAssignedShift.emit(this.paging);
  }
  getclientuploadeddocument(element){
    const data = {
      ClientId: element,
      PageSize: this.paging.pageSize,
      pageNo: this.paging.pageNo,
  }
    this.empService.getClientUploadedDocument(data).subscribe((res=>{
      this.response = res;
      switch (this.response.status) {
        case 1:
          this.documentcheckList = this.response.responseData;
          this.dataSourcedocumentdata = new MatTableDataSource(this.documentcheckList);
          document.getElementById("openViewdataModalButton").click();
          break;
        default:
          this.notificationService.Warning({ message: 'No record found', title: '' });
          break;
      }
    }));
  }
  
  FileSaver = require('file-saver');
  downloadPdf(docUrl) {
   this.FileSaver.saveAs(docUrl);
 }
}
export interface StatusList {
  id?: number;
  codeDescription?: string;
}
export interface ClientComplianceDetails {
  id?: number
  employeeId?: number;
  documentName?: number;
  documentType?: number;
  documentTypeName?: string;
  issueDate?: Date;
  expiryDate?: Date;
  description?: string;
  hasExpiry?: boolean;
  alert?: boolean;
  isActive?: boolean;
  isDeleted?: boolean;
  deletedById?: string;
  deletedBy?: string;
  deletedDate?: Date;
  createdById?: string;
  createdBy?: string;
  createdDate?: Date;
  updateById?: number;
  updateBy?: string;
  updatedDate?: Date;
  fileName?: string;
  document?:string;
}