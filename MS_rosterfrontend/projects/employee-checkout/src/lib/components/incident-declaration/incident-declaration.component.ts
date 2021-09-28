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
  selector: 'lib-incident-declaration',
  templateUrl: './incident-declaration.component.html',
  styleUrls: ['./incident-declaration.component.scss'],
  providers: [
    {
        provide: DateAdapter, useClass: AppDateAdapter
    },
    {
        provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS
    }
    ]
})

export class IncidentDeclarationComponent implements OnInit {
  response: ResponseModel = {};
  getErrorMessage:'Please Enter Value';
  rFormDeclaration:FormGroup;
  todayDatemax = new Date();
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild('btnEditaccidentCancel') editCancel: ElementRef;
  @ViewChild('formDirective') private formDirective: NgForm;
  @ViewChild('fileInput', {static: false}) fileInput: ElementRef;
  clientId: number;
  textDeclaration: string;
  shiftId: number;
  employeeId: any;
  constructor(private fb: FormBuilder,private membershipService: MembershipService,private notification:NotificationService,private route: ActivatedRoute,private logoutService:LogoutService,private commonservice:CommonService) {
   }
   
   ngOnInit(): void {
    this.route.paramMap.subscribe((params: any) => {
      this.clientId = Number(params.params.id);
      this.shiftId = Number(params.params.shiftId);
    });
     this.CreateFormDeclaration();
     this.getIncidentDeclaration();
     this.employeeId = this.membershipService.getUserDetails('employeeId');
    }
 
  CreateFormDeclaration() {
    this.rFormDeclaration = this.fb.group({
      IsDeclaration: ['', Validators.nullValidator],
      DeclarationName: ['', Validators.required],
      DeclarationPosition: ['', Validators.required],
      DeclarationDate: ['', Validators.required],
     });
  }
 
 
  getIncidentDeclaration() {
      const data = {
       Id : this.clientId,
       ShiftId : this.shiftId
 };
     this.logoutService.getIncidentDeclaration(data).subscribe(res => {
      this.response = res;
     if (this.response.status > 0) {
     if(this.response.responseData.incidentDeclaration.clientId>0){
      this.rFormDeclaration.controls['DeclarationName'].patchValue(this.response.responseData.incidentDeclaration.name);
      this.rFormDeclaration.controls['DeclarationPosition'].patchValue(this.response.responseData.incidentDeclaration.positionAtOrganisation);
      this.rFormDeclaration.controls['DeclarationDate'].patchValue(this.response.responseData.incidentDeclaration.date);
      this.rFormDeclaration.controls['IsDeclaration'].patchValue(this.response.responseData.incidentDeclaration.isDeclaration==true?"1":"");
      this.textDeclaration="Update";
    }
    else{
     this.textDeclaration="Submit"
    }
  
    }
    });
    }
 
    AddDeclaration() {  
      if (this.rFormDeclaration.valid) {
        if(this.rFormDeclaration.value.IsDeclaration!="1"){
          this.notification.Warning({ message: "Please Select Confirmation box", title: null });
        }
        else{
        const data = {
          'ClientId': this.clientId,
          'ShiftId': this.shiftId,
          'EmployeeId': this.employeeId,
          'Name': this.rFormDeclaration.value.DeclarationName,
          'PositionAtOrganisation': this.rFormDeclaration.value.DeclarationPosition,
          'Date': moment(this.rFormDeclaration.value.DeclarationDate).format('YYYY-MM-DD'),
          'IsDeclaration': this.rFormDeclaration.value.IsDeclaration== "1" ? true:false,
        };
         this.logoutService.AddIncidentDeclaration(data).subscribe(res => {
          this.response = res;
          switch (this.response.status) {
            case 1:
            this.notification.Success({ message: this.response.message, title: null });
            this.rFormDeclaration.controls['DeclarationName'].patchValue(this.response.responseData.name);
            this.rFormDeclaration.controls['DeclarationPosition'].patchValue(this.response.responseData.positionAtOrganisation);
            this.rFormDeclaration.controls['DeclarationDate'].patchValue(this.response.responseData.date);
            this.rFormDeclaration.controls['IsDeclaration'].patchValue(this.response.responseData.isDeclaration==true?"1":"");
            this.textDeclaration="Update";
            break;
            default:
            this.notification.Warning({ message: this.response.message, title: null });
            break;
          }
        });
      }
    }
    }
   
}
