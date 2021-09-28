import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { FormGroup, FormBuilder, Validators, NgForm, FormControl } from '@angular/forms';
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
import { IncidentImpactedPerson } from '../../view-models/accident-incident';
import { Subject, ReplaySubject } from 'rxjs';
import { MatSelect } from '@angular/material/select';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'lib-incident-impactedperson',
  templateUrl: './incident-impactedperson.component.html',
  styleUrls: ['./incident-impactedperson.component.scss'],
  providers: [
    {
        provide: DateAdapter, useClass: AppDateAdapter
    },
    {
        provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS
    }
    ]
})

export class IncidentImpactedpersonComponent implements OnInit {
  getErrorMessage:'Please Enter Value';
 ImpactedModel: IncidentImpactedPerson = {};
  response: ResponseModel = {};
  rFormImpacted: FormGroup;
  todayDatemax = new Date();
  @ViewChild('btnEditaccidentCancel') editCancel: ElementRef;
  @ViewChild('formDirective') private formDirective: NgForm;
  @ViewChild('fileInput', {static: false}) fileInput: ElementRef;
  public controlPrimarydis: FormControl = new FormControl();
  public controlotherdis: FormControl = new FormControl();
  public controlBehaviourconcern: FormControl = new FormControl();
  public controlCommunication: FormControl = new FormControl();
  private _onDestroy = new Subject<void>();
  public filteredRecords: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
  public filteredbehaviourRecords: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
  public filteredcommunicationRecords: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
  public filteredotherdisRecords: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
  public filteredPrimarydisRecords: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
  public filteredRecordsSecondary: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
  @ViewChild('Select') select: MatSelect;
  clientId: number;
  textImpacted: string;
  Primarydislist: any;
  otherdislist: any;
  ImpactCommunication: any;
  ImpactConcerndata: any;
  communicationlist: any;
  behaviourlist: any;
  primarydisability: any;
  otherdisability: any;
  genderList: any;
  shiftId: number;
  employeeId: any;
  constructor(private fb: FormBuilder,private membershipService: MembershipService,private notification:NotificationService,private route: ActivatedRoute,private logoutService:LogoutService,private commonservice:CommonService) {
    this.route.paramMap.subscribe((params: any) => {
      this.clientId = Number(params.params.id);
      this.shiftId = Number(params.params.shiftId);
    });
    this.employeeId = this.membershipService.getUserDetails('employeeId');
  }
  
   ngOnInit(): void {
     this.createFormImpacted();
     this.getPrimarydisability();
     this.getotherdisability();
     this.getcommunication();
     this.getconcernbehaviour();
     this.getgender();
     this.GetImpactedPersonInfo();
     this.searchcategoryBehaviour();
     this.searchcategoryPrimaryDis();
     this.searchcategoryOtherDis();
     this.searchcategoryCommunication();
  }
 
  searchcategoryPrimaryDis(){
    this.controlPrimarydis.valueChanges
    .pipe(takeUntil(this._onDestroy))
    .subscribe(() => {
      this.filterPrimarydisability();
    });
  }

 private filterPrimarydisability() {
    if (!this.primarydisability) {
      return;
    }
   let search = this.controlPrimarydis.value;
    if (!search) {
      this.filteredPrimarydisRecords.next(this.primarydisability.slice());
      return;
    } else {
      search = search.toLowerCase();
    }
    if (search.length >= 1) {
     this.filteredPrimarydisRecords.next(
      this.primarydisability.filter(Type => Type.codeDescription.toLowerCase().indexOf(search) > -1)
     );
    }
  }

 searchcategoryOtherDis(){
    this.controlotherdis.valueChanges
    .pipe(takeUntil(this._onDestroy))
    .subscribe(() => {
      this.filterOtherdisability();
    });
  }

private filterOtherdisability() {
    if (!this.otherdisability) {
      return;
    }
   let search = this.controlotherdis.value;
    if (!search) {
      this.filteredotherdisRecords.next(this.otherdisability.slice());
      return;
    } else {
      search = search.toLowerCase();
    }
    if (search.length >= 1) {
     this.filteredotherdisRecords.next(
      this.otherdisability.filter(Type => Type.codeDescription.toLowerCase().indexOf(search) > -1)
     );
    }
  }
  searchcategoryCommunication(){
    this.controlCommunication.valueChanges
    .pipe(takeUntil(this._onDestroy))
    .subscribe(() => {
      this.filterdCommunication();
    });
  }

 private filterdCommunication() {
    if (!this.communicationlist) {
      return;
    }
   let search = this.controlCommunication.value;
    if (!search) {
      this.filteredcommunicationRecords.next(this.communicationlist.slice());
      return;
    } else {
      search = search.toLowerCase();
    }
    if (search.length >= 1) {
     this.filteredcommunicationRecords.next(
      this.communicationlist.filter(Type => Type.codeDescription.toLowerCase().indexOf(search) > -1)
     );
    }
  }
  searchcategoryBehaviour(){
    this.controlBehaviourconcern.valueChanges
    .pipe(takeUntil(this._onDestroy))
    .subscribe(() => {
      this.filterdBehaviour();
    });
  }

 private filterdBehaviour() {
    if (!this.behaviourlist) {
      return;
    }
   let search = this.controlBehaviourconcern.value;
    if (!search) {
      this.filteredbehaviourRecords.next(this.behaviourlist.slice());
      return;
    } else {
      search = search.toLowerCase();
    }
    if (search.length >= 1) {
     this.filteredbehaviourRecords.next(
      this.behaviourlist.filter(Type => Type.codeDescription.toLowerCase().indexOf(search) > -1)
     );
    }
  }
  createFormImpacted() {
    this.rFormImpacted = this.fb.group({
      ImpactedTitle: ['', Validators.required],
      ImpactedFirstName: ['', Validators.required],
      ImpactedMiddleName: ['', Validators.nullValidator],
      ImpactedLastName: ['', Validators.required],
      ImpactedPhoneNo: ['',[Validators.maxLength(16),Validators.nullValidator]],
      ImpactedEmail: [null, Validators.compose([Validators.nullValidator, Validators.pattern(/^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/)])],
      ImpactedGender: ['', Validators.required],
      ImpactedNdisNo: ['', Validators.nullValidator],
      ImpactedDOB: ['', Validators.required],
      ImpactedPrimaryDisbility: ['', Validators.required],
      ImpactedSecondaryDisbility: ['', Validators.nullValidator],
      ImpactedBehaviour: ['', Validators.required],
      ImpactedCommunication: ['', Validators.required],
      OtherDetail: ['', Validators.nullValidator],
     });
  } 
  GetImpactedPersonInfo() {
  const data = {
   Id : this.clientId,
   ShiftId : this.shiftId
  };
 this.logoutService.GetIncidentImpactedPerson(data).subscribe(res => {
   this.response = res;
    if (this.response.status > 0) {
    if(this.response.responseData.incidentImpactedPerson.clientId>0){
    this.Primarydislist=this.response.responseData.incidentPrimaryDisability;
    this.rFormImpacted.controls['ImpactedPrimaryDisbility'].patchValue(this.Primarydislist.map(x=>x.primaryDisability));
    this.otherdislist=this.response.responseData.incidentOtherDisability;
    this.rFormImpacted.controls['ImpactedSecondaryDisbility'].patchValue(this.otherdislist.map(x=>x.otherDisabilityId));
    this.ImpactCommunication=this.response.responseData.clientIncidentCommunication;
    this.rFormImpacted.controls['ImpactedCommunication'].patchValue(this.ImpactCommunication.map(x=>x.communicationId));
    this.ImpactConcerndata=this.response.responseData.incidentConcernBehaviour;
    this.rFormImpacted.controls['ImpactedBehaviour'].patchValue(this.ImpactConcerndata.map(x=>x.concerBehaviourId));
    this.rFormImpacted.controls['ImpactedTitle'].patchValue(this.response.responseData.incidentImpactedPerson.title);
    this.rFormImpacted.controls['ImpactedFirstName'].patchValue(this.response.responseData.incidentImpactedPerson.firstName);
    this.rFormImpacted.controls['ImpactedMiddleName'].patchValue(this.response.responseData.incidentImpactedPerson.middleName);
    this.rFormImpacted.controls['ImpactedLastName'].patchValue(this.response.responseData.incidentImpactedPerson.lastName);
    this.rFormImpacted.controls['ImpactedNdisNo'].patchValue(this.response.responseData.incidentImpactedPerson.ndisParticipantNo);
    this.rFormImpacted.controls['ImpactedGender'].patchValue(this.response.responseData.incidentImpactedPerson.genderId);
    this.rFormImpacted.controls['ImpactedDOB'].patchValue(this.response.responseData.incidentImpactedPerson.dateOfBirth);
    this.rFormImpacted.controls['ImpactedPhoneNo'].patchValue(this.response.responseData.incidentImpactedPerson.phoneNo);
    this.rFormImpacted.controls['ImpactedEmail'].patchValue(this.response.responseData.incidentImpactedPerson.email);
    this.rFormImpacted.controls['OtherDetail'].patchValue(this.response.responseData.incidentImpactedPerson.otherDetail);
    this.textImpacted="Update";
    }
    else{
   this.textImpacted="Submit"
   }
   }
  });
  }
  getgender(){
    this.commonservice.getGenderList().subscribe((res=>{
      if(res){
        this.response = res;
        this.genderList=this.response.responseData||[];
     }
     else{
     }
    }));
  }
  getPrimarydisability(){
    this.commonservice.GetPrimaryDisability().subscribe((res=>{
      if(res){
        this.response = res;
        this.primarydisability=this.response.responseData||[];
        this.filteredPrimarydisRecords.next(this.primarydisability.slice());
      }else{

      }
    }));
  }
  getotherdisability(){
    this.commonservice.GetSecondaryDisability().subscribe((res=>{
      if(res){
        this.response = res;
        this.otherdisability=this.response.responseData||[];
        this.filteredotherdisRecords.next(this.otherdisability.slice());
      }else{

      }
    }));
  }
  getconcernbehaviour(){
    this.commonservice.GetConcernBehaviour().subscribe((res=>{
      if(res){
        this.response = res;
        this.behaviourlist=this.response.responseData||[];
        this.filteredbehaviourRecords.next(this.behaviourlist.slice());
       }else{
      }
    }));
  }
  getcommunication(){
    this.commonservice.GetCommunicationType().subscribe((res=>{
      if(res){
        this.response = res;
        this.communicationlist=this.response.responseData||[];
        this.filteredcommunicationRecords.next(this.communicationlist.slice());
      }else{
   }
    }));
  }
  AddImpactedPerson() {  
    if (this.rFormImpacted.valid) {
      this.ImpactedModel.PrimaryDisability = (this.rFormImpacted.value.ImpactedPrimaryDisbility);
      this.ImpactedModel.OtherDisability = (this.rFormImpacted.value.ImpactedSecondaryDisbility);
      this.ImpactedModel.CommunicationId = (this.rFormImpacted.value.ImpactedCommunication);
      this.ImpactedModel.ConcerBehaviourId = (this.rFormImpacted.value.ImpactedBehaviour);
      const data = {
        'ClientId': this.clientId,
        'ShiftId': this.shiftId,
        'EmployeeId': this.employeeId,
        'PrimaryDisability': this.ImpactedModel.PrimaryDisability,
        'OtherDisability': this.ImpactedModel.OtherDisability,
        'CommunicationId': this.ImpactedModel.CommunicationId,
        'ConcerBehaviourId': this.ImpactedModel.ConcerBehaviourId,
        'Title': this.rFormImpacted.value.ImpactedTitle,
        'FirstName': this.rFormImpacted.value.ImpactedFirstName,
        'MiddleName': this.rFormImpacted.value.ImpactedMiddleName,
        'LastName': this.rFormImpacted.value.ImpactedLastName,
        'NdisParticipantNo': this.rFormImpacted.value.ImpactedNdisNo,
        'PhoneNo': this.rFormImpacted.value.ImpactedPhoneNo,
        'Email': this.rFormImpacted.value.ImpactedEmail,
        'GenderId': this.rFormImpacted.value.ImpactedGender,
        'OtherDetail': this.rFormImpacted.value.OtherDetail,
        'DateOfBirth': moment(this.rFormImpacted.value.ImpactedDOB).format('YYYY-MM-DD'),
       };
       this.logoutService.AddIncidentImpactedPerson(data).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
          this.notification.Success({ message: this.response.message, title: null });
          this.GetImpactedPersonInfo();
          this.textImpacted="Update";
          break;
        default:
          this.notification.Warning({ message: this.response.message, title: null });
          break;
        }
      });
    }
  }
}
