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
import { IncidentAllegation } from '../../view-models/accident-incident';
import { Subject, ReplaySubject } from 'rxjs';
import { MatSelect } from '@angular/material/select';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'lib-incident-subjectallegation',
  templateUrl: './incident-subjectallegation.component.html',
  styleUrls: ['./incident-subjectallegation.component.scss'],
  providers: [
    {
        provide: DateAdapter, useClass: AppDateAdapter
    },
    {
        provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS
    }
    ]
})

export class IncidentSubjectallegationComponent implements OnInit {
  response: ResponseModel = {};
  getErrorMessage:'Please Enter Value';
  AllegationModel: IncidentAllegation = {};
  rFormAllegation: FormGroup;
  PrimaryAllegation: any;
  otherAllegation: any;
  AllegationCommunication: any;
  AllegationConcerndata: any;
  textSubject: string;
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
  behaviourlist: any;
  communicationlist: any;
  otherdisability: any;
  primarydisability: any;
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
     this.createSubjectAllegation();
     this.getotherdisability();
     this.getPrimarydisability();
     this.getconcernbehaviour();
     this.getcommunication();
     this.getgender();
     this.rFormAllegation.controls['IsSubjectAllegation'].setValue("2");
     this.GetIncidentSubjectAllegation();
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
  getgender(){
    this.commonservice.getGenderList().subscribe((res=>{
      if(res){
        this.response = res;
        this.genderList=this.response.responseData||[];
       
      }else{

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
  createSubjectAllegation() {
    this.rFormAllegation = this.fb.group({
      WorkerTitle: [null, Validators.required],
      WorkerFirstName: [null, Validators.required],
      WorkerLastName: [null, Validators.required],
      WorkerPhoneNo: [null,[Validators.maxLength(16),Validators.required]],
      WorkerEmail: [null, Validators.compose([Validators.nullValidator, Validators.pattern(/^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/)])],
      WorkerGender: [null, Validators.required],
      IsSubjectAllegation: [null, Validators.nullValidator],
      WorkerDOB: [null, Validators.required],
      WorkerPosition: [null, Validators.nullValidator],
      DisableTitle: [null, Validators.required],
      DisableFirstName: [null, Validators.required],
      DisableLastName: [null, Validators.required],
      DisablePhoneNo: [null,[Validators.maxLength(16),Validators.nullValidator]],
      DisableEmail: [null, Validators.compose([Validators.nullValidator, Validators.pattern(/^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/)])],
      DisableGender: [null, Validators.required],
      DisableDOBirth: [null, Validators.required],
      DisableNdisNo: [null, Validators.required],
      DisablePrimaryDisability: [null, Validators.required],
      DisableOtherDisability: [null, Validators.nullValidator],
      DisableBehaviour: [null, Validators.required],
      DisableCommunication: [null, Validators.required],
      AllegationOtherDetail: [null, Validators.nullValidator],
      OtherTitle: [null, Validators.nullValidator],
      OtherFirstName: [null, Validators.nullValidator],
      OtherLastName: [null, Validators.nullValidator],
      OtherPhoneNo: [null,[Validators.maxLength(16),Validators.nullValidator]],
      OtherEmail: [null, Validators.compose([Validators.nullValidator, Validators.pattern(/^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/)])],
      OtherGender: [null, Validators.nullValidator],
      OtherDOB: [null, Validators.nullValidator],
      OtherRelationShip: [null, Validators.nullValidator],
    });
  } 
  AddSubjectAllegation() {  
    if (this.rFormAllegation.valid) {
      this.AllegationModel.PrimaryDisability = (this.rFormAllegation.value.DisablePrimaryDisability);
      this.AllegationModel.OtherDisability = (this.rFormAllegation.value.DisableOtherDisability!=null?this.rFormAllegation.value.DisableOtherDisability:[]);
      this.AllegationModel.CommunicationId = (this.rFormAllegation.value.DisableCommunication);
      this.AllegationModel.ConcerBehaviourId = (this.rFormAllegation.value.DisableBehaviour);
     debugger
      const data = {
        'ClientId': this.clientId,
        'ShiftId': this.shiftId,
        'EmployeeId': this.employeeId,
        'PrimaryDisability': this.AllegationModel.PrimaryDisability,
        'OtherDisability': this.AllegationModel.OtherDisability,
        'CommunicationId': this.AllegationModel.CommunicationId,
        'ConcerBehaviourId': this.AllegationModel.ConcerBehaviourId,
        'Title': this.rFormAllegation.value.WorkerTitle,
        'FirstName': this.rFormAllegation.value.WorkerFirstName,
        'LastName': this.rFormAllegation.value.WorkerLastName,
        'Position': this.rFormAllegation.value.WorkerPosition,
        'PhoneNo': String(this.rFormAllegation.value.WorkerPhoneNo),
        'Email': this.rFormAllegation.value.WorkerEmail,
        'GenderId': this.rFormAllegation.value.WorkerGender,
        'OtherDetail': this.rFormAllegation.value.AllegationOtherDetail,
        'DateOfBirth': moment(this.rFormAllegation.value.WorkerDOB).format('YYYY-MM-DD'),
        'IsSubjectAllegation': this.rFormAllegation.value.IsSubjectAllegation=="1"?this.rFormAllegation.value.IsSubjectAllegation=1:this.rFormAllegation.value.IsSubjectAllegation=="2"?this.rFormAllegation.value.IsSubjectAllegation=2:this.rFormAllegation.value.IsSubjectAllegation=="3"?this.rFormAllegation.value.IsSubjectAllegation=3:this.rFormAllegation.value.IsSubjectAllegation=0,
        'DisableTitle': this.rFormAllegation.value.DisableTitle,
        'DisableFirstName': this.rFormAllegation.value.DisableFirstName,
        'DisableLastName': this.rFormAllegation.value.WorkerLastName,
        'DisableNdisNumber': this.rFormAllegation.value.DisableNdisNo,
        'DisablePhoneNo': String(this.rFormAllegation.value.DisablePhoneNo),
        'DisableEmail': this.rFormAllegation.value.DisableEmail,
        'DisableGender': this.rFormAllegation.value.DisableGender,
        'DisableDateOfBirth': moment(this.rFormAllegation.value.DisableDOBirth).format('YYYY-MM-DD'),
        'OtherTitle': this.rFormAllegation.value.OtherTitle,
        'OtherFirstName': this.rFormAllegation.value.OtherFirstName,
        'OtherLastName': this.rFormAllegation.value.OtherLastName,
        'OtherRelationship': this.rFormAllegation.value.OtherRelationShip,
        'OtherPhoneNo': String(this.rFormAllegation.value.OtherPhoneNo),
        'OtherEmail': this.rFormAllegation.value.OtherEmail,
        'OtherGender': Number(this.rFormAllegation.value.OtherGender),
        'OtherDateOfBirth':moment(this.rFormAllegation.value.OtherDOB).format('YYYY-MM-DD')=="Invalid date"?this.todayDatemax:moment(this.rFormAllegation.value.OtherDOB).format('YYYY-MM-DD'),
      };

       this.logoutService.AddIncidentSubjectAllegation(data).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
          this.notification.Success({ message: this.response.message, title: null });
           this.GetIncidentSubjectAllegation();
           this.textSubject="Update";
            break;
           default:
            this.notification.Warning({ message: this.response.message, title: null });
            break;
        }
      });
    }
  }
  GetIncidentSubjectAllegation() {
  const data = {
   Id : this.clientId
  };
  this.logoutService.GetIncidentSubjectAllegation(data).subscribe(res => {
   this.response = res;
   if (this.response.status > 0) {
   if(this.response.responseData.incidentWorkerAllegation.clientId>0){
    this.PrimaryAllegation=this.response.responseData.incidentAllegationPrimaryDisability;
    this.rFormAllegation.controls['DisablePrimaryDisability'].patchValue(this.PrimaryAllegation.map(x=>x.primaryDisability));
    this.otherAllegation=this.response.responseData.incidentAllegationOtherDisability;
    this.rFormAllegation.controls['DisableOtherDisability'].patchValue(this.otherAllegation.map(x=>x.otherDisability));
    this.AllegationCommunication=this.response.responseData.incidentAllegationCommunication;
    this.rFormAllegation.controls['DisableCommunication'].patchValue(this.AllegationCommunication.map(x=>x.communicationId));
    this.AllegationConcerndata=this.response.responseData.incidentAllegationBehaviour;
    this.rFormAllegation.controls['DisableBehaviour'].patchValue(this.AllegationConcerndata.map(x=>x.concerBehaviourId));
    this.rFormAllegation.controls['WorkerTitle'].patchValue(this.response.responseData.incidentWorkerAllegation.title);
    this.rFormAllegation.controls['WorkerFirstName'].patchValue(this.response.responseData.incidentWorkerAllegation.firstName);
    this.rFormAllegation.controls['WorkerLastName'].patchValue(this.response.responseData.incidentWorkerAllegation.lastName);
    this.rFormAllegation.controls['WorkerPosition'].patchValue(this.response.responseData.incidentWorkerAllegation.position);
    this.rFormAllegation.controls['WorkerGender'].patchValue(this.response.responseData.incidentWorkerAllegation.gender);
    this.rFormAllegation.controls['WorkerDOB'].patchValue(this.response.responseData.incidentWorkerAllegation.dateOfBirth);
    this.rFormAllegation.controls['WorkerPhoneNo'].patchValue(this.response.responseData.incidentWorkerAllegation.phoneNo);
    this.rFormAllegation.controls['WorkerEmail'].patchValue(this.response.responseData.incidentWorkerAllegation.email);
    this.rFormAllegation.controls['AllegationOtherDetail'].patchValue(this.response.responseData.incidentDisablePersonAllegation.otherDetail);
    this.rFormAllegation.controls['IsSubjectAllegation'].patchValue(this.response.responseData.incidentWorkerAllegation.isSubjectAllegation==1?"1":this.response.responseData.incidentWorkerAllegation.isSubjectAllegation==2?"2":this.response.responseData.incidentWorkerAllegation.isSubjectAllegation==3?"3":"");
    this.rFormAllegation.controls['DisableTitle'].patchValue(this.response.responseData.incidentDisablePersonAllegation.title);
    this.rFormAllegation.controls['DisableFirstName'].patchValue(this.response.responseData.incidentDisablePersonAllegation.firstName);
    this.rFormAllegation.controls['DisableLastName'].patchValue(this.response.responseData.incidentDisablePersonAllegation.lastName);
    this.rFormAllegation.controls['DisableNdisNo'].patchValue(this.response.responseData.incidentDisablePersonAllegation.ndisNumber);
    this.rFormAllegation.controls['DisableGender'].patchValue(this.response.responseData.incidentDisablePersonAllegation.gender);
    this.rFormAllegation.controls['DisableDOBirth'].patchValue(this.response.responseData.incidentDisablePersonAllegation.dateOfBirth);
    this.rFormAllegation.controls['DisablePhoneNo'].patchValue(this.response.responseData.incidentDisablePersonAllegation.phoneNo);
    this.rFormAllegation.controls['DisableEmail'].patchValue(this.response.responseData.incidentDisablePersonAllegation.email);
    this.rFormAllegation.controls['OtherFirstName'].patchValue(this.response.responseData.incidentOtherAllegation.firstName);
    this.rFormAllegation.controls['OtherLastName'].patchValue(this.response.responseData.incidentOtherAllegation.lastName);
    this.rFormAllegation.controls['OtherRelationShip'].patchValue(this.response.responseData.incidentOtherAllegation.relationship);
    this.rFormAllegation.controls['OtherGender'].patchValue(this.response.responseData.incidentOtherAllegation.gender);
    this.rFormAllegation.controls['OtherDOB'].patchValue(this.response.responseData.incidentOtherAllegation.dateOfBirth);
    this.rFormAllegation.controls['OtherPhoneNo'].patchValue(this.response.responseData.incidentOtherAllegation.phoneNo);
    this.rFormAllegation.controls['OtherEmail'].patchValue(this.response.responseData.incidentOtherAllegation.email);
    this.rFormAllegation.controls['OtherTitle'].patchValue(this.response.responseData.incidentOtherAllegation.title);
    this.textSubject="Update";
  }
  else{
   this.textSubject="Submit"
  }
 }
  
});
}

}
