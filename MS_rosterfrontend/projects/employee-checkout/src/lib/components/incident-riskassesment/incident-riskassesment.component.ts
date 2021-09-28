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
  selector: 'lib-incident-riskassesment',
  templateUrl: './incident-riskassesment.component.html',
  styleUrls: ['./incident-riskassesment.component.scss'],
  providers: [
    {
        provide: DateAdapter, useClass: AppDateAdapter
    },
    {
        provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS
    }
    ]
})

export class IncidentRiskassesmentComponent implements OnInit {
  response: ResponseModel = {};
  rFormRisk: FormGroup;
  getErrorMessage:'Please Enter Value';
  todayDatemax = new Date();
  @ViewChild('btnEditaccidentCancel') editCancel: ElementRef;
  @ViewChild('formDirective') private formDirective: NgForm;
  @ViewChild('fileInput', {static: false}) fileInput: ElementRef;
  clientId: number;
  textRisk: string;
  shiftId: number;
  employeeId: any;
  constructor(private fb: FormBuilder,private notification:NotificationService,private membershipService: MembershipService, private route: ActivatedRoute,private logoutService:LogoutService,private commonservice:CommonService) {
    this.route.paramMap.subscribe((params: any) => {
      this.clientId = Number(params.params.id);
      this.shiftId = Number(params.params.shiftId);
    });
    this.employeeId = this.membershipService.getUserDetails('employeeId');

   }
   ngOnInit(): void {
     this.createFormRisk();
     this.rFormRisk.controls['IsRiskAssesment'].setValue("2");
     this.getRiskAssesment();
   }
 
  createFormRisk() {
    this.rFormRisk = this.fb.group({
      IsRiskAssesment: ['', Validators.nullValidator],
      RiskAssesmentDate: ['', Validators.nullValidator],
      RiskDetails: ['', Validators.nullValidator],
      NoRiskAssesmentInfo: ['', Validators.nullValidator],
      InProgressRisk: ['', Validators.nullValidator],
      TobeFinished: [null, Validators.nullValidator],
     });
  }
  
    getRiskAssesment() {
      const data = {
       Id : this.clientId,
       ShiftId : this.shiftId
    };
     this.logoutService.getRiskAssesment(data).subscribe(res => {
     this.response = res; 
     if (this.response.status > 0) {
     if(this.response.responseData.incidentRiskAssesment.clientId>0){
      this.rFormRisk.controls['IsRiskAssesment'].patchValue(this.response.responseData.incidentRiskAssesment.isRiskAssesment==1?"1":this.response.responseData.incidentRiskAssesment.isRiskAssesment==2?"2":this.response.responseData.incidentRiskAssesment.isRiskAssesment==3?"3":"")
      this.rFormRisk.controls['RiskAssesmentDate'].patchValue(this.response.responseData.incidentRiskAssesment.riskAssesmentDate);
      this.rFormRisk.controls['RiskDetails'].patchValue(this.response.responseData.incidentRiskAssesment.riskDetails);
      this.rFormRisk.controls['NoRiskAssesmentInfo'].patchValue(this.response.responseData.incidentRiskAssesment.noRiskAssesmentInfo);
      this.rFormRisk.controls['InProgressRisk'].patchValue(this.response.responseData.incidentRiskAssesment.inProgressRisk);
      this.rFormRisk.controls['TobeFinished'].patchValue(this.response.responseData.incidentRiskAssesment.tobeFinished);
      this.textRisk="Update";
    }
    else{
     this.textRisk="Submit"
    }
  }
  });
}
    AddRiskAssesment() {  
      if (this.rFormRisk.valid) {
        const data = {
          'ClientId': this.clientId,
          'ShiftId': this.shiftId,
          'EmployeeId': this.employeeId,
          'IsRiskAssesment': this.rFormRisk.value.IsRiskAssesment=="1"?this.rFormRisk.value.IsRiskAssesment=1:this.rFormRisk.value.IsRiskAssesment=="2"?this.rFormRisk.value.IsRiskAssesment=2:this.rFormRisk.value.IsRiskAssesment=="3"?this.rFormRisk.value.IsRiskAssesment=3:this.rFormRisk.value.IsRiskAssesment=0,
          'RiskAssesmentDate': moment(this.rFormRisk.value.RiskAssesmentDate).format('YYYY-MM-DD'),
          'RiskDetails': this.rFormRisk.value.RiskDetails,
          'NoRiskAssesmentInfo': this.rFormRisk.value.NoRiskAssesmentInfo,
          'InProgressRisk': this.rFormRisk.value.InProgressRisk,
          'TobeFinished': this.rFormRisk.value.TobeFinished,
          };
         this.logoutService.AddIncidentRiskAssesment(data).subscribe(res => {
          this.response = res;
          switch (this.response.status) {
            case 1:
            this.notification.Success({ message: this.response.message, title: null });
            this.getRiskAssesment();
            break;
            default:
            this.notification.Warning({ message: this.response.message, title: null });
            break;
          }
        });
      }
    }
   
}
