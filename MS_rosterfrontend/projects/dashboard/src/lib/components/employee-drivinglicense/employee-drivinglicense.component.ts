import { Component, OnInit, Input, ViewChild, ElementRef } from '@angular/core';
import { EmployeeDlModel } from '../../viewmodel/employee-dl-model';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { EmpServiceService } from '../../services/emp-service.service';
import { NotificationService } from 'projects/core/src/projects';
import { ActivatedRoute } from '@angular/router';
import * as moment from 'moment';
import { CommonService } from 'projects/lhs-service/src/projects';
import { AppDateAdapter } from 'projects/lhs-directives/src/projects';
import { APP_DATE_FORMATS } from 'projects/lhs-directives/src/lib/directives/date-format.directive';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';
import { MatTableDataSource } from '@angular/material/table';
import { Paging } from 'projects/viewmodels/paging';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { merge, from } from 'rxjs';
import { tap } from 'rxjs/operators'
@Component({
  selector: 'lib-employee-drivinglicense',
  templateUrl: './employee-drivinglicense.component.html',
  styleUrls: ['./employee-drivinglicense.component.scss'],
  providers: [
    {
        provide: DateAdapter, useClass: AppDateAdapter
    },
    {
        provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS
    }
    ]
})
export class EmployeeDrivinglicenseComponent implements OnInit {
  @Input() employeeDrivingLicenseInfo: EmployeeDlModel
  @Input() employeeDrivingLicensemodel: EmployeeDlModel[];
  isShownState: boolean = false ;
  rForm: FormGroup;
  rFormAdd: FormGroup;
  response: ResponseModel = {};
  @ViewChild('btnLicenseCancel') cancel: ElementRef;
  @ViewChild('btnEditLicenseCancel') cancelEdit: ElementRef;
  @ViewChild('formDirective') private formDirective: NgForm;
  getErrorMessage: any;
  employeeId: number;
  LicenseTypeList: any;
  StateList: any;
  LicenseList: any;
  dataSourcelicense: any;
  paging: Paging = {};
  displayedColumnslicense: string[] = ['drivingLicense','licenseOrigin', 'licenseNo', 'carRegNo', 'licenseExpiryDate', 'carRegExpiryDate','licenseTypeName','licenseStateName','insuranceExpiryDate','action'];
  totalCount: number;
  deletedriverId: any;
  EditId: any;
  list: any;
  selectedType: any;
  selectedorigin: any;
  constructor(private fb: FormBuilder, private empService: EmpServiceService,private commonservice:CommonService, private notificationService: NotificationService,
    private route: ActivatedRoute) {
      this.paging.pageNo = 1;
      this.paging.pageSize = 10;
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.employeeId = Number(params['Id']);
    });
   this.createForm();
   this.createFormAdd();
   this.getLicensetype();
   this.getState();
   this.getLicenseorigin();
   this.GetDriverList();
   this.checkbutton();
  }
  checkbutton(){
    this.rFormAdd.controls['AdddrivingLicense'].setValue("false");
    this.rFormAdd.controls['AddcarInsurance'].setValue("false");
    
  }
  createForm() {
    this.rForm = this.fb.group({
      drivingLicense: [null, Validators.required],
      licenseOrigin: [null, Validators.required],
      licenseNo: [null, Validators.compose([Validators.required, Validators.pattern(/^[a-z0-9_-]{8,15}$/), Validators.maxLength(25)])],
      carRegNo: [null, Validators.compose([Validators.required, Validators.pattern(/^[a-z0-9_-]{8,15}$/), Validators.maxLength(25)])],
      licenseExpiryDate: [null, Validators.required],
      carRegExpiryDate: [null, Validators.required],
      carInsurance: [null, Validators.required],
      licenseType: [null, Validators.required],
      licenseState: [null, Validators.nullValidator],
      insuranceExpiryDate: [null, Validators.required],
    });
 
  }
  createFormAdd() {
    this.rFormAdd = this.fb.group({
      id: [this.employeeDrivingLicenseInfo.id, null],
      AdddrivingLicense: [null, Validators.required],
      AddlicenseOrigin: [null, Validators.required],
      AddlicenseNo: [null, Validators.compose([Validators.required, Validators.pattern(/^[a-z0-9_-]{8,15}$/), Validators.maxLength(25)])],
      AddcarRegNo: [null, Validators.compose([Validators.required, Validators.pattern(/^[a-z0-9_-]{8,15}$/), Validators.maxLength(25)])],
      AddlicenseExpiryDate: [null, Validators.required],
      AddcarRegExpiryDate: [null, Validators.required],
      AddcarInsurance: [null, Validators.required],
      AddlicenseType: [null, Validators.required],
      AddlicenseState: [null, Validators.nullValidator],
      AddinsuranceExpiryDate: [null, Validators.required],
    });
  }
  UpdateLicenseDetails() {
    if (this.rForm.valid) {
      const data = {
        id: this.EditId,
        employeeId: this.employeeId,
        drivingLicense: this.rForm.get('drivingLicense').value == 'true' ? true : false,
        LicenseOrigin: this.rForm.get('licenseOrigin').value,
        licenseNo: this.rForm.get('licenseNo').value,
        carRegNo: this.rForm.get('carRegNo').value,
        licenseExpiryDate: this.rForm.get('licenseExpiryDate').value,
        carInsurance: this.rForm.get('carInsurance').value == 'true' ? true : false,
        licenseType: this.rForm.get('licenseType').value,
        licenseState: this.rForm.get('licenseState').value,
        carRegExpiryDate: this.rForm.get('carRegExpiryDate').value,
        insuranceExpiryDate: moment(this.rForm.get('insuranceExpiryDate').value).format('YYYY-MM-DD').toString(),
      }
      this.empService.updateLicenseInfo(data).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
            this.employeeDrivingLicenseInfo = this.response.responseData;            
            this.cancelEdit.nativeElement.click();
            this.GetDriverList();
            this.notificationService.Success({ message: 'Details updated successfully', title: '' });
            break;

          default:
            break;
        }
      });
    }
  }
  AddLicenseDetails() {
    if (this.rFormAdd.valid) {
      const data = {
        employeeId: this.employeeId,
        drivingLicense: this.rFormAdd.get('AdddrivingLicense').value == 'true' ? true : false,
        LicenseOrigin: this.rFormAdd.get('AddlicenseOrigin').value,
        licenseNo: this.rFormAdd.get('AddlicenseNo').value,
        carRegNo: this.rFormAdd.get('AddcarRegNo').value,
        licenseExpiryDate: this.rFormAdd.get('AddlicenseExpiryDate').value,
        carInsurance: this.rFormAdd.get('AddcarInsurance').value == 'true' ? true : false,
        licenseType: this.rFormAdd.get('AddlicenseType').value,
        licenseState: this.rFormAdd.get('AddlicenseState').value,
        carRegExpiryDate: this.rFormAdd.get('AddcarRegExpiryDate').value,
        insuranceExpiryDate: moment(this.rFormAdd.get('AddinsuranceExpiryDate').value).format('YYYY-MM-DD').toString(),
      }
      this.empService.updateLicenseInfo(data).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
            this.employeeDrivingLicenseInfo = this.response.responseData; 
            this.cancel.nativeElement.click();
            this.checkbutton();
            this.GetDriverList(); 
            this.notificationService.Success({ message: 'Details updated successfully', title: '' });
            break;

          default:
            break;
        }
      });
    }
  }
  openEditModal(elem) {
    document.getElementById("openEditlicenseModalButton").click();
    this.EditId = elem.id;
    this.rForm.controls['drivingLicense'].patchValue(elem.drivingLicense== true ? 'true' : 'false');
    this.rForm.controls['licenseOrigin'].patchValue(elem.licenseOrigin);
    this.rForm.controls['licenseNo'].patchValue(elem.licenseNo);
    this.rForm.controls['carRegNo'].patchValue(elem.carRegNo);
    this.rForm.controls['licenseExpiryDate'].patchValue(elem.licenseExpiryDate);
    this.rForm.controls['carRegExpiryDate'].patchValue(elem.carRegExpiryDate);
    this.rForm.controls['carInsurance'].patchValue(elem.carInsurance== true ? 'true' : 'false');
    this.rForm.controls['licenseType'].patchValue(elem.licenseType);
    if(elem.licenseState>0 && elem.licenseState!=null){
      this.rForm.controls['licenseState'].patchValue(elem.licenseState);
      this.rForm.controls['licenseState'].enable();
    }
    else{
      this.rForm.controls['licenseState'].disable();
    }
    
    this.rForm.controls['insuranceExpiryDate'].patchValue(elem.insuranceExpiryDate);
  
  }
  disablestate(event) { 
    this.list=this.LicenseList;
    this.selectedType = event;
    const index = this.list.findIndex(x => x.id == this.selectedType);
    this.selectedorigin= this.list[index].codeDescription;
     if(this.selectedorigin=="International License"){ 
      this.rFormAdd.controls['AddlicenseState'].disable();
      this.rFormAdd.controls['AddlicenseState'].patchValue(0);
    }
    else{ 
      this.rFormAdd.controls['AddlicenseState'].enable();
      
    }
  }
  disablestateEdit(event) { 
    this.list=this.LicenseList;
    this.selectedType = event;
    const index = this.list.findIndex(x => x.id == this.selectedType);
    this.selectedorigin= this.list[index].codeDescription;
     if(this.selectedorigin=="International License"){ 
      this.rForm.controls['licenseState'].patchValue(0);
      this.rForm.controls['licenseState'].disable();
    }
    else{ 
      this.rForm.controls['licenseState'].enable();
    }
  }
  getLicenseorigin(){
    this.commonservice.getdriverlicense().subscribe((res=>{
      if(res){
        this.response = res;
        this.LicenseList=this.response.responseData||[];
      }else{
    }
    }));
  }
  getLicensetype(){
    this.commonservice.getLicenseType().subscribe((res=>{
      if(res){
        this.response = res;
        this.LicenseTypeList=this.response.responseData||[];
       
      }else{

      }
    }));
  }
  getState() {
    this.commonservice.getstate().subscribe((res => {
      if (res) {
        this.response = res;
        this.StateList = this.response.responseData;
      }
    }));
  }
  GetDriverList() {
     const data = {
      employeeId:this.employeeId,
      pageNo: this.paging.pageNo,
      pageSize: this.paging.pageSize,
    };
    this.empService.GetEmployeeDriverList(data).subscribe(res => {
      this.response = res;
      this.totalCount = this.response.total;
       if (this.response.responseData) {
        this.employeeDrivingLicenseInfo = this.response.responseData;
        this.employeeDrivingLicensemodel = this.response.responseData;
        this.dataSourcelicense = new MatTableDataSource(this.employeeDrivingLicensemodel);
         }
      else {
        this.dataSourcelicense = new MatTableDataSource(this.employeeDrivingLicensemodel);
      }

    });
  }
  PageIndexEvent(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.GetDriverList();
  }
  cancelModal() {
    this.rFormAdd.reset();
    this.formDirective.resetForm();
  }
  DeleteModal(driverID,_e)
  {
   this.deletedriverId = driverID;
  }

  deletedriverInfo(event) {
    const data = {
      Id: this.deletedriverId
    }
    this.empService.DeleteEmployeeDrivingLicense(data).subscribe(res => {
      this.response = res;
      switch (this.response.status) {
        case 1:
         this.notificationService.Success({ message: 'License details deleted successfully', title: '' })
         this.GetDriverList();
          break;
        case 0:
          this.notificationService.Error({ message: this.response.message, title: '' });
        default:
          break;
      }
    });
  }
}
