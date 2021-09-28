import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Paging } from 'projects/viewmodels/paging';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { NotificationService, MembershipService } from 'projects/core/src/projects';
import { CommonService } from 'projects/lhs-service/src/lib/service/common/common.service';
import * as moment from 'moment';
import { APP_DATE_FORMATS, AppDateAdapter } from 'projects/lhs-directives/src/lib/directives/date-format.directive';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';
import { environment } from 'src/environments/environment';
import { LogoutService } from '../../services/logout.service';

@Component({
  selector: 'lib-incident-providerdetail',
  templateUrl: './incident-providerdetail.component.html',
  styleUrls: ['./incident-providerdetail.component.scss']
})

export class IncidentProviderdetailComponent implements OnInit {
  responseModel: ResponseModel = {};
  getErrorMessage:'Please Enter Value';
  response: ResponseModel = {};
  rFormProvider: FormGroup;
  todayDatemax = new Date();
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild('btnEditaccidentCancel') editCancel: ElementRef;
  @ViewChild('formDirective') private formDirective: NgForm;
  @ViewChild('fileInput', {static: false}) fileInput: ElementRef;
  clientId: number;
  textProvider: string;
  StateList: any;
  shiftId: number;
  employeeId: any;
  constructor(private fb: FormBuilder,private membershipService: MembershipService,private notification:NotificationService, private route: ActivatedRoute,private logoutService:LogoutService,
    private commonservice:CommonService) {
      this.route.paramMap.subscribe((params: any) => {
        this.clientId = Number(params.params.id);
        this.shiftId = Number(params.params.shiftId);
      });
      this.employeeId = this.membershipService.getUserDetails('employeeId');
   }
   ngOnInit(): void {
    this.createFormProvider();
    this.getState();
    this.getIncidentProviderInfo();
  }
   createFormProvider() {
    this.rFormProvider = this.fb.group({
      ReportedBy: ['',Validators.required],
      ProviderName: ['',Validators.required],
      ProviderABN: ['',Validators.required],
      OutletName: ['',Validators.required],
      RegistrationGroup: ['',Validators.nullValidator],
      State: [null,Validators.required], 
      RegistrationId: ['',Validators.required],
    });
  }
  getState() {
    this.commonservice.getstate().subscribe((res => {
      if (res) {
        this.response = res;
        this.StateList = this.response.responseData;
      }
    }));
  }
   getIncidentProviderInfo() {
      const data = {
       Id : this.clientId,
       ShiftId : this.shiftId
     };
     this.logoutService.getIncidentProviderInfo(data).subscribe(res => {
      this.response = res;
      if (this.response.status > 0) {
        if(this.response.responseData.clientAccidentProviderInfo.clientId>0){
        this.rFormProvider.controls['ReportedBy'].patchValue(this.response.responseData.clientAccidentProviderInfo.reportCompletedBy);
        this.rFormProvider.controls['ProviderName'].patchValue(this.response.responseData.clientAccidentProviderInfo.providerName);
        this.rFormProvider.controls['RegistrationId'].patchValue(this.response.responseData.clientAccidentProviderInfo.providerregistrationId);
        this.rFormProvider.controls['ProviderABN'].patchValue(this.response.responseData.clientAccidentProviderInfo.providerABN);
        this.rFormProvider.controls['OutletName'].patchValue(this.response.responseData.clientAccidentProviderInfo.outletName);
        this.rFormProvider.controls['RegistrationGroup'].patchValue(this.response.responseData.clientAccidentProviderInfo.registrationgroup);
        this.rFormProvider.controls['State'].patchValue(this.response.responseData.clientAccidentProviderInfo.state);
        this.textProvider="Update";
       }
      else{
        this.rFormProvider.controls['ProviderName'].patchValue("Life Health Services");
        this.rFormProvider.controls['RegistrationId'].patchValue("4-433C-2205");
        this.rFormProvider.controls['ProviderABN'].patchValue("72 623 159 446");
       this.textProvider="Submit"
      }
    }
    });
    }
    AddProviderInfo() {  
      if (this.rFormProvider.valid) {
        const data = {
          'ClientId': this.clientId,
          'ShiftId': this.shiftId,
          'EmployeeId': this.employeeId,
          'ReportCompletedBy': this.rFormProvider.value.ReportedBy,
          'ProviderName': this.rFormProvider.value.ProviderName,
          'ProviderregistrationId': this.rFormProvider.value.RegistrationId,
          'ProviderABN': this.rFormProvider.value.ProviderABN,
          'OutletName': this.rFormProvider.value.OutletName,
          'Registrationgroup': this.rFormProvider.value.RegistrationGroup,
          'State': this.rFormProvider.value.State
        };
          this.logoutService.AddIncidentProviderdetail(data).subscribe(res => {
          this.response = res;
          switch (this.response.status) {
            case 1:
            this.notification.Success({ message: this.response.message, title: null });
            this.getIncidentProviderInfo();
            break;
            default:
            this.notification.Warning({ message: this.response.message, title: null });
            break;
          }
        });
      }
    }
 }
