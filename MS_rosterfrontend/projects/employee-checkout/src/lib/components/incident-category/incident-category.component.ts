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
import { AccidentIncidents } from '../../view-models/accident-incident';
import { MatSelect } from '@angular/material/select';
import { ReplaySubject, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'lib-incident-category',
  templateUrl: './incident-category.component.html',
  styleUrls: ['./incident-category.component.scss']
})

export class IncidentCategoryComponent implements OnInit {
  response: ResponseModel = {};
  rFormCategory: FormGroup;
  clientId=0;
  @ViewChild('btnEditaccidentCancel') editCancel: ElementRef;
  @ViewChild('formDirective') private formDirective: NgForm;
  @ViewChild('fileInput', {static: false}) fileInput: ElementRef;
  public controlPrimary: FormControl = new FormControl();
  public controlSecondary: FormControl = new FormControl();
  private _onDestroy = new Subject<void>();
  public filteredRecords: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
  public filteredRecordsSecondary: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
  @ViewChild('Select') select: MatSelect;
  Primarylist: any;
  Secondarylist: any;
  textCategory: string;
  PrimaryTypeList: any;
  SecondaryList: any;
  IncidentModel: AccidentIncidents = {};
  shiftId: number;
  employeeId: any;
  constructor(private fb: FormBuilder,private membershipService: MembershipService,private notification:NotificationService,private route: ActivatedRoute,private logoutService:LogoutService, private commonservice:CommonService) {
   }
   
   ngOnInit(): void {
    this.route.paramMap.subscribe((params: any) => {
      this.clientId = Number(params.params.id);
      this.shiftId = Number(params.params.shiftId);
    });
    this.employeeId = this.membershipService.getUserDetails('employeeId');
     this.createFormCategory();
     this.getPrimaryCategory();
     this.getSecondaryCategory();
     this.GetIncidentCategory();
     this.searchPrimarycategorytype();
     this.searchSecondarycategorytype();
  }
  
  createFormCategory() {
    this.rFormCategory = this.fb.group({
      PrimaryCategory: ['', Validators.required],
      SecondaryCategory: ['', Validators.required],
      IsIncidentAnticipated: ['', Validators.nullValidator],
     
    });
  }
  searchPrimarycategorytype(){
    this.controlPrimary.valueChanges
    .pipe(takeUntil(this._onDestroy))
    .subscribe(() => {
      this.filterPrimarycategory();
    });
  }

 private filterPrimarycategory() {
    if (!this.PrimaryTypeList) {
      return;
    }
    let search = this.controlPrimary.value;
    if (!search) {
      this.filteredRecords.next(this.PrimaryTypeList.slice());
      return;
    } else {
      search = search.toLowerCase();
    }
    if (search.length >= 1) {
     this.filteredRecords.next(
      this.PrimaryTypeList.filter(ServiceType => ServiceType.codeDescription.toLowerCase().indexOf(search) > -1)
     );
    }
  }
  searchSecondarycategorytype(){
    this.controlSecondary.valueChanges
    .pipe(takeUntil(this._onDestroy))
    .subscribe(() => {
      this.filterSecondarycategory();
    });
  }

 private filterSecondarycategory() {
    if (!this.SecondaryList) {
      return;
    }
    let search = this.controlSecondary.value;
    if (!search) {
      this.filteredRecordsSecondary.next(this.SecondaryList.slice());
      return;
    } else {
      search = search.toLowerCase();
    }
    if (search.length >= 1) {
     this.filteredRecords.next(
      this.SecondaryList.filter(ServiceType => ServiceType.codeDescription.toLowerCase().indexOf(search) > -1)
     );
    }
  }

   GetAllAccidentIncidentInfo() {
      const data = {
       Id : this.clientId,
       ShiftId : this.shiftId,
       
     };
     this.logoutService.GetAllIncidentDetails(data).subscribe(res => {
     if (this.response.status > 0) {
     this.response = res;
     if(this.response.responseData.clientIncidentCategory.clientId>0){
      this.rFormCategory.controls['IsIncidentAnticipated'].patchValue(this.response.responseData.clientIncidentCategory.isIncidentAnticipated==1?"1":this.response.responseData.clientIncidentCategory.isIncidentAnticipated==2?"2":this.response.responseData.clientIncidentCategory.isIncidentAnticipated==3?"3":"")
      this.Primarylist=this.response.responseData.clientPrimaryIncidentCategory;
      this.rFormCategory.controls['PrimaryCategory'].patchValue(this.Primarylist.map(x=>x.primaryIncidentId));
      this.Secondarylist=this.response.responseData.clientSecondaryIncidentCategory;
      this.rFormCategory.controls['SecondaryCategory'].patchValue(this.Secondarylist.map(x=>x.secondaryIncidentId));
      this.textCategory="Update";
    }
     else{
      this.textCategory="Submit"
    }
   }
    });
    }
    getPrimaryCategory(){
      this.commonservice.GetPrimaryCategory().subscribe((res=>{
        if(res){
          this.response = res;
          this.PrimaryTypeList=this.response.responseData||[];
          this.filteredRecords.next(this.PrimaryTypeList.slice());
        }else{
  
        }
      }));
    }
    getSecondaryCategory(){
      this.commonservice.GetSecondaryCategory().subscribe((res=>{
        if(res){
          this.response = res;
          this.SecondaryList=this.response.responseData||[];
          this.filteredRecordsSecondary.next(this.SecondaryList.slice());
        }else{
  
        }
      }));
    }
    GetIncidentCategory() {
      const data = {
       Id : this.clientId,
       ShiftId : this.shiftId
      };
     this.logoutService.GetClientIncidentCategory(data).subscribe(res => {
      this.response = res;
      if (this.response.status > 0) {
        if(this.response.responseData.clientIncidentCategory.clientId>0){
          this.rFormCategory.controls['IsIncidentAnticipated'].patchValue(this.response.responseData.clientIncidentCategory.isIncidentAnticipated==1?"1":this.response.responseData.clientIncidentCategory.isIncidentAnticipated==2?"2":this.response.responseData.clientIncidentCategory.isIncidentAnticipated==3?"3":"")
          this.Primarylist=this.response.responseData.clientPrimaryIncidentCategory;
          this.rFormCategory.controls['PrimaryCategory'].patchValue(this.Primarylist.map(x=>x.primaryIncidentId));
          this.Secondarylist=this.response.responseData.clientSecondaryIncidentCategory;
          this.rFormCategory.controls['SecondaryCategory'].patchValue(this.Secondarylist.map(x=>x.secondaryIncidentId));
          this.textCategory="Update";
        }
        else{
         this.textCategory="Submit"
        }
     
    }
       else{
    
       }
    });
      }
    AddIncidentCategory() {  
      if (this.rFormCategory.valid) {
        this.IncidentModel.primaryIncidentId = (this.rFormCategory.value.PrimaryCategory);
        this.IncidentModel.secondaryIncidentId = (this.rFormCategory.value.SecondaryCategory);
        const data = {
          'ClientId': this.clientId,
          'ShiftId': this.shiftId,
          'EmployeeId': this.employeeId,
          'PrimaryIncidentId': this.IncidentModel.primaryIncidentId,
          'SecondaryIncidentId': this.IncidentModel.secondaryIncidentId,
          'IsIncidentAnticipated': this.rFormCategory.value.IsIncidentAnticipated=="1"?this.rFormCategory.value.IsIncidentAnticipated=1:this.rFormCategory.value.IsIncidentAnticipated=="2"?this.rFormCategory.value.IsIncidentAnticipated=2:this.rFormCategory.value.IsIncidentAnticipated=="3"?this.rFormCategory.value.IsIncidentAnticipated=3:this.rFormCategory.value.IsIncidentAnticipated=0,
        };
         this.logoutService.AddIncidentCategory(data).subscribe(res => {
          this.response = res;
          switch (this.response.status) {
            case 1:
            this.notification.Success({ message: this.response.message, title: null });
            this.textCategory="Update";
            this.GetIncidentCategory();
            this.textCategory="Update";
              break;
            default:
              this.notification.Warning({ message: this.response.message, title: null });
              break;
          }
        });
      }
    }
   
}

