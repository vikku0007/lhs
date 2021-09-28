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
  selector: 'lib-incident-contactperson',
  templateUrl: './incident-contactperson.component.html',
  styleUrls: ['./incident-contactperson.component.scss']
})

export class IncidentContactpersonComponent implements OnInit {
  responseModel: ResponseModel = {};
  rFormcontact: FormGroup;
  textContact: string;
  todayDatemax = new Date();
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild('btnEditaccidentCancel') editCancel: ElementRef;
  @ViewChild('formDirective') private formDirective: NgForm;
  @ViewChild('fileInput', {static: false}) fileInput: ElementRef;
  clientId: number;
  response: ResponseModel = {};
  shiftId: number;
  employeeId: any;
  constructor(private fb: FormBuilder,private notification:NotificationService, private route: ActivatedRoute,
    private logoutservice:LogoutService,private membershipService: MembershipService,
    private commonservice:CommonService) {
   }
 
   ngOnInit(): void {
    this.route.paramMap.subscribe((params: any) => {
      this.clientId = Number(params.params.id);
      this.shiftId = Number(params.params.shiftId);
    });
     this.createFormContact();
     this.getIncidentPrimaryContact();
     this.employeeId = this.membershipService.getUserDetails('employeeId');
  }
  createFormContact() {
    this.rFormcontact = this.fb.group({
      ContactTitle: ['', Validators.required],
      ContactFirstName: ['', Validators.required],
      ContactMiddleName: ['', Validators.nullValidator],
      ContactLastName: ['', Validators.required],
      ContactPhoneNo: ['',[Validators.maxLength(16),Validators.required]],
      ContactEmail: [null, Validators.compose([Validators.nullValidator, Validators.pattern(/^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/)])],
      ContactMethod: ['', Validators.required],
      ContactProvider: ['', Validators.required],
      
    });
  }  
  getIncidentPrimaryContact() {
      const data = {
       Id : this.clientId,
       ShiftId : this.shiftId
      };
     this.logoutservice.getIncidentPrimaryContact(data).subscribe(res => {
     this.response = res;
     if (this.response.status > 0) {
     if(this.response.responseData.clientAccidentPrimaryContact.clientId>0){
        this.rFormcontact.controls['ContactTitle'].patchValue(this.response.responseData.clientAccidentPrimaryContact.title);
        this.rFormcontact.controls['ContactFirstName'].patchValue(this.response.responseData.clientAccidentPrimaryContact.firstName);
        this.rFormcontact.controls['ContactMiddleName'].patchValue(this.response.responseData.clientAccidentPrimaryContact.middleName);
        this.rFormcontact.controls['ContactLastName'].patchValue(this.response.responseData.clientAccidentPrimaryContact.lastName);
        this.rFormcontact.controls['ContactProvider'].patchValue(this.response.responseData.clientAccidentPrimaryContact.providerPosition);
        this.rFormcontact.controls['ContactPhoneNo'].patchValue(this.response.responseData.clientAccidentPrimaryContact.phoneNo);
        this.rFormcontact.controls['ContactEmail'].patchValue(this.response.responseData.clientAccidentPrimaryContact.email);
        this.rFormcontact.controls['ContactMethod'].patchValue(this.response.responseData.clientAccidentPrimaryContact.contactMetod);
        this.textContact="Update";
     }
     else{
      this.textContact="Submit"
     }
    }
    });
    }
    
    AddcontactInfo() {  
      if (this.rFormcontact.valid) {
        const data = {
          'ClientId': this.clientId,
          'ShiftId': this.shiftId,
          'EmployeeId': this.employeeId,
          'Title': this.rFormcontact.value.ContactTitle,
          'FirstName': this.rFormcontact.value.ContactFirstName,
          'MiddleName': this.rFormcontact.value.ContactMiddleName,
          'LastName': this.rFormcontact.value.ContactLastName,
          'ProviderPosition': this.rFormcontact.value.ContactProvider,
          'PhoneNo': this.rFormcontact.value.ContactPhoneNo,
          'Email': this.rFormcontact.value.ContactEmail,
          'ContactMetod': this.rFormcontact.value.ContactMethod,
        };
         this.logoutservice.AddIncidentContactInfo(data).subscribe(res => {
          this.response = res;
          switch (this.response.status) {
            case 1:
            this.notification.Success({ message: this.response.message, title: null });
            this.rFormcontact.controls['ContactTitle'].patchValue(this.response.responseData.title);
            this.rFormcontact.controls['ContactFirstName'].patchValue(this.response.responseData.firstName);
            this.rFormcontact.controls['ContactMiddleName'].patchValue(this.response.responseData.middleName);
            this.rFormcontact.controls['ContactLastName'].patchValue(this.response.responseData.lastName);
            this.rFormcontact.controls['ContactProvider'].patchValue(this.response.responseData.providerPosition);
            this.rFormcontact.controls['ContactPhoneNo'].patchValue(this.response.responseData.phoneNo);
            this.rFormcontact.controls['ContactEmail'].patchValue(this.response.responseData.email);
            this.rFormcontact.controls['ContactMethod'].patchValue(this.response.responseData.contactMetod);
            this.textContact="Update";
            break;
            default:
            this.notification.Warning({ message: this.response.message, title: null });
            break;
          }
        });
      }
    }
 }
