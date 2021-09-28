import { Component, OnInit, ViewChild, Input, OnChanges, SimpleChanges, ElementRef, Output, EventEmitter } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { EmpDashboardService } from '../../services/emp-dashboard.service';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { AssignedShift, CurrentShift } from '../../viewmodels/assigned-shift';
import { FormGroup, Validators, FormBuilder, NgForm } from '@angular/forms';
import { debug } from 'console';
import { NotificationService } from 'projects/core/src/projects';
import { Paging } from 'projects/viewmodels/paging';
import { PageEvent } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { getMatScrollStrategyAlreadyAttachedError } from '@angular/cdk/overlay/scroll/scroll-strategy';
import { element } from 'protractor';

export interface RequiredComplianceElement {
  clientName: string;
  location: string;
  startDate: string;
  endDate: string;
  startTime: string;
  endTime: string;
}

const REQUIRED_COMPLIANCE_DATA: RequiredComplianceElement[] = [
  { clientName: 'Mario Speedwagon', location: 'Sydney', startDate: '20-Sep-2020', endDate: '30-Mar-2020', startTime: '9:00 AM', endTime: '6:00 PM' },
  { clientName: 'Petey Cruiser', location: 'Melbourne', startDate: '19-Jun-2019', endDate: '31-Mar-2020', startTime: '9:00 AM', endTime: '6:00 PM' },
  { clientName: 'Anna Sthesia', location: 'Sydney', startDate: '01-Apr-2019', endDate: '31-Mar-2020', startTime: '9:00 AM', endTime: '6:00 PM' },
  { clientName: 'Paul Molive', location: 'Perth', startDate: '01-Sep-2019', endDate: '31-Mar-2020', startTime: '9:00 AM', endTime: '6:00 PM' },
];

@Component({
  selector: 'lib-currentshift',
  templateUrl: './currentshift.component.html',
  styleUrls: ['./currentshift.component.scss']
})
export class CurrentshiftComponent implements OnInit, OnChanges {
  displayedColumnsRequired: string[] = ['sr', 'clientName', 'location', 'startDate', 'endDate', 'startTime', 'endTime', 'action'];
  displayedColumnsdocument: string[] = ['documentType', 'documentName', 'action'];
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @Input() employeeId: number;
  response: ResponseModel = {};
  currentShiftModel: CurrentShift[] = [];
  documentcheckList: ClientComplianceDetails[] = [];
  dataSourceRequired = new MatTableDataSource(this.currentShiftModel);
  dataSourcedocument = new MatTableDataSource(this.documentcheckList);
  getErrorMessage: any;
  rForm: FormGroup;
  selectedShiftId: number;
  paging: Paging = {};
  //hide : boolean=false;
  mydatehour:any = new Date().getHours();
  mydateminutes:any = new Date().getMinutes();
  mydate = this.mydatehour*60  + this.mydateminutes;
  @ViewChild('btnCancel') cancel: ElementRef;
  @ViewChild('formDirective') formDirective: NgForm;
  @Output() updateAssignedShift = new EventEmitter();
  baseUrl: string = environment.baseUrl;
  constructor(private empService: EmpDashboardService, private fb: FormBuilder, private notificationService: NotificationService) { }

  ngOnChanges(changes: SimpleChanges): void {
    if (this.employeeId !== undefined) {
      this.getCurrentShift();
    }
  }

  ngOnInit(): void {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
    this.dataSourceRequired.sort = this.sort;
    this.createForm();
  }

  createForm() {
    this.rForm = this.fb.group({
      remark: ['', Validators.nullValidator],
    });
  }

  get remark() {
    return this.rForm.get('remark');
  }

  // convertDate(date:any)
  // {
  //   let day= date.getDate();
  //   let month = date.getMonth();
  //   let year = date.getFullYear()
  //  return `${day}-${month}-${year}`
  // }
  getCurrentShift() {
    const data = {
      id: Number(this.employeeId)
    };
    this.empService.getCurrentShifts(data).subscribe(res => {
      this.response = res;
      switch (this.response.status) {
        case 1:
        //   let arr= [];
        //   let date2 = this.convertDate(new Date());
        //   this.response.responseData.forEach(element => { 
        //     let date1 = this.convertDate(new Date(element.startDate))
        //     if(date1 == date2)  {       
        //   var minutes= this.mydate -  element.startTime.totalMinutes;
        //   if(minutes<=15 && minutes>=-15){
        //     element["hide"] = true
        //   }
        //   else{
        //     element["hide"] = false
        //   }
        // }
        // else
        // {
        //   element["hide"] = false 
        // }
        //   arr.push(element)
        //   });
        
          this.currentShiftModel = this.response.responseData;         
          this.dataSourceRequired = new MatTableDataSource(this.currentShiftModel);
          break;
        default:
          this.notificationService.Warning({ message: 'No record found', title: '' });
          break;
      }
    });
  }

  getShiftId(data: CurrentShift) {
    this.selectedShiftId = data.id;
  }
  checkIn() {
    if (this.rForm.valid) {
      const data = {
        employeeId: Number(this.employeeId),
        shiftId: Number(this.selectedShiftId),
        checkInRemark: this.rForm.controls['remark'].value,
        checkInDate: new Date(),
        checkInTime: new Date().toTimeString().split(' ')[0]
      }
      this.empService.addCheckInDetails(data).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
            this.notificationService.Success({ message: 'Check in successfull', title: '' });
            this.cancel.nativeElement.click();
            this.formDirective.resetForm();
            this.rForm.reset();
            this.getCurrentShift();
            this.updateAssignedShift.emit();
            break;
          case 0:
            this.notificationService.Warning({
              message: this.response.message.toLowerCase().indexOf('already') > -1 ? 'Already check in' :
                this.response.message, title: ''
            });
            this.cancel.nativeElement.click();
            this.formDirective.resetForm();
            this.rForm.reset();

          default:
            break;
        }
      });
    } else {
      this.notificationService.Warning({ message: 'Remark missing', title: '' });
    }
  }

  cancelForm() {
    this.rForm.reset();
    this.formDirective.resetForm();
  }
  getclientuploadeddocument(element) {
    debugger
    const data = {
      ClientId: element,
      PageSize: this.paging.pageSize,
      pageNo: this.paging.pageNo,
    }
    this.empService.getClientUploadedDocument(data).subscribe((res => {
      this.response = res;
      switch (this.response.status) {
        case 1:
          this.documentcheckList = this.response.responseData;
          this.dataSourcedocument = new MatTableDataSource(this.documentcheckList);
          document.getElementById("openViewModalButton").click();
          break;
        default:
          this.notificationService.Warning({ message: 'No record found', title: '' });
          break;
      }
    }));
  }
  PageIndexEvent(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    // this.getclientuploadeddocument();
  }
  FileSaver = require('file-saver');
  downloadPdf(docUrl) {
    this.FileSaver.saveAs(docUrl);
  }
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
  document?: string;
}

