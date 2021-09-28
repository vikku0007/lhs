import { Component, OnInit, Input, ViewChild, ElementRef, SimpleChanges, OnChanges } from '@angular/core';
import { EmployeeJobProfile } from '../../viewmodel/employee-jobprofile';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { EmpServiceService } from '../../services/emp-service.service';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { ActivatedRoute } from '@angular/router';
import { NotificationService } from 'projects/core/src/lib/services/notification-service/notification.service';
import { CommonService } from 'projects/lhs-service/src/projects';
import * as _ from 'lodash';
import { Subject, ReplaySubject } from 'rxjs';
import { MatSelect } from '@angular/material/select';
import { takeUntil } from 'rxjs/operators';
interface Department {
  id: number;
  codeDescription: string;
}
interface SourceOfHire {
  id: number;
  codeDescription: string;
}

@Component({
  selector: 'lib-employee-jobprofile',
  templateUrl: './employee-jobprofile.component.html',
  styleUrls: ['./employee-jobprofile.component.scss']
})

export class EmployeeJobProfileComponent implements OnInit, OnChanges {
  @Input() empJobProfile: EmployeeJobProfile;
  rForm: FormGroup;
  response: ResponseModel = {};
  @ViewChild('btnJobProfileCancel') cancel: ElementRef;
  getErrorMessage: any;
  departmentList: Department[];
  sourceOfHireList: SourceOfHire[];
  LocationList:any;
  ReportedToList:any;
  LocationTypeList: any;
  list: any;
  selectedType: any;
  isShownOtherLocation: boolean = false ;
  isShownLocatiodropdown: boolean = false ;
  locationnameId: any;
  public control: FormControl = new FormControl();
  public searchcontrol: FormControl = new FormControl();
  private _onDestroy = new Subject<void>();
  public filteredRecords: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
  public filteredRecordsreport: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
  @ViewChild('Select') select: MatSelect;
  constructor(private commonService: CommonService,private fb: FormBuilder, private route: ActivatedRoute, private empService: EmpServiceService, private notificationService: NotificationService) { }

  ngOnInit(): void {
    if (this.empJobProfile.employeeId === 0) {
      this.route.queryParams.subscribe(params => {
        // tslint:disable-next-line: radix
        this.empJobProfile.employeeId = parseInt(params.Id);
      });
    }
    this.createForm();
    this.getLocationType();
     this.getReportedTo();
    // this.getLocation();
     this.getDepartments();
     this.getSourceofHire();
     this.searchdepartmenttype();
     this.searchreportedto();
  }
  ngOnChanges(changes: SimpleChanges): void {
    if (this.empJobProfile.employeeId === 0) {
      this.route.queryParams.subscribe(params => {
        // tslint:disable-next-line: radix
        this.empJobProfile.employeeId = parseInt(params.Id);
      });
  }
}
  createForm() {
    this.rForm = this.fb.group({
      jobDesc: [this.empJobProfile.jobDesc, Validators.required],
     // locationId: [this.empJobProfile.locationId, Validators.nullValidator],
      reportingToId: [this.empJobProfile.reportingToId === 0 ? null : this.empJobProfile.reportingToId , Validators.required],
      departmentId: [this.empJobProfile.departmentId == 0 ? null : this.empJobProfile.departmentId , Validators.required],
      dateOfJoining: [this.empJobProfile.dateOfJoining, Validators.required],
      distanceTravel: [this.empJobProfile.distanceTravel, Validators.compose([Validators.required,
        Validators.max(100),
        
        ])],
      source: [this.empJobProfile.source, Validators.required],
     
      workingHoursWeekly: [this.empJobProfile.workingHoursWeekly, Validators.compose([Validators.required,
        Validators.max(100),
        ])],
        // locationtype: [this.empJobProfile.locationType, Validators.required],
        // otherlocation: ['', Validators.nullValidator],
    });
  }
  searchdepartmenttype(){
    this.control.valueChanges
    .pipe(takeUntil(this._onDestroy))
    .subscribe(() => {
      this.filterdepartmenttype();
    });
  }


 private filterdepartmenttype() {
    if (!this.departmentList) {
      return;
    }
    let search = this.control.value;
    if (!search) {
      this.filteredRecords.next(this.departmentList.slice());
      return;
    } else {
      search = search.toLowerCase();
    }
    if (search.length >= 1) {
     this.filteredRecords.next(
      this.departmentList.filter(department => department.codeDescription.toLowerCase().indexOf(search) > -1)
     );
    }
  }
  get workingHoursWeekly() {
    return this.rForm.get('workingHoursWeekly');
  }
  
  get source() {
    return this.rForm.get('source');
  }
  get jobDesc() {
    return this.rForm.get('jobDesc');
  }

  // get locationId() {
  //   return this.rForm.get('locationId');
  // }

  get reportingToId() {
    return this.rForm.get('reportingToId');
  }

  get departmentId() {
    return this.rForm.get('departmentId');
  }
  get dateOfJoining() {
    return this.rForm.get('dateOfJoining');
  }
  get distanceTravel() {
    return this.rForm.get('distanceTravel');
  }
  // get locationtype() {
  //   return this.rForm.get('locationtype');
  // }
  // get otherlocation() {
  //   return this.rForm.get('otherlocation');
  // }
  UpdateJobProfile() {
  
    if (this.rForm.valid) {
     
      const data = {
        Id : this.empJobProfile.id,
        EmployeeId: this.empJobProfile.employeeId,
        JobDesc: this.jobDesc.value,
        // tslint:disable-next-line: radix
        DepartmentId: parseInt(this.departmentId.value),
        // tslint:disable-next-line: radix
       // LocationId: (this.locationnameId),
        // tslint:disable-next-line: radix
        ReportingToId: parseInt(this.reportingToId.value),
        DateOfJoining: this.dateOfJoining.value,
        Source: Number(this.source.value),
        // tslint:disable-next-line: radix
        WorkingHoursWeekly: parseInt(this.workingHoursWeekly.value),
        // tslint:disable-next-line: radix
        DistanceTravel: parseInt(this.distanceTravel.value),
        
      };
      this.empService.updateEmployeeJobProfile(data).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
            this.empJobProfile = this.response.responseData;
          
            this.empJobProfile.departmentName =  this.departmentList.filter(x=>x.id === Number(this.departmentId.value))[0].codeDescription;
           // this.empJobProfile.locationName = this.empJobProfile.locationName;;
            this.empJobProfile.reportingToName = this.ReportedToList.filter(x=>x.id === Number(this.reportingToId.value))[0].fullName;
            this.empJobProfile.sourceName = this.sourceOfHireList.filter(x=>x.id === Number(this.source.value))[0].codeDescription;
           // this.empJobProfile.locationTypeName=this.empJobProfile.locationTypeName;
            this.cancel.nativeElement.click();
            this.notificationService.Success({ message: 'JobProfile updated successfully', title: null });
            break;

          default:
            break;
        }
      });
    }
  }
  selectChangeHandler(event:any) {
    this.list=this.LocationTypeList;
    this.selectedType = event;
   if(this.selectedType==5){
      this.getLocation()
      this.isShownLocatiodropdown=true;
      this.isShownOtherLocation=false;
      this.rForm.controls['otherlocation'].patchValue("");
    }
    else{
      this.isShownLocatiodropdown=false;
      this.isShownOtherLocation=true;
    }
  }
  searchreportedto(){
    this.searchcontrol.valueChanges
    .pipe(takeUntil(this._onDestroy))
    .subscribe(() => {
      this.filterreportedto();
    });
  }
  
  
  private filterreportedto() {
    if (!this.ReportedToList) {
      return;
    }
    let search = this.searchcontrol.value;
    if (!search) {
      this.filteredRecordsreport.next(this.ReportedToList.slice());
      return;
    } else {
      search = search.toLowerCase();
    }
    if (search.length >= 1) {
     this.filteredRecordsreport.next(
      this.ReportedToList.filter(department => department.fullName.toLowerCase().indexOf(search) > -1)
     );
    }
  }
  getLocationType(){
    this.commonService.getLocationType().subscribe((res=>{
      if(res){
        this.response = res;
        this.LocationTypeList=this.response.responseData||[];
       
      }else{

      }
    }));
  }
  getDepartments(){
    
    this.commonService.getDepartmentList().subscribe(res => {
      this.response = res;
     
      switch (this.response.status) {
        case 1:
         this.departmentList = this.response.responseData || [];
         this.filteredRecords.next(this.departmentList.slice());
          break;

        default:
          break;
      }
    });
  }
  getSourceofHire(){
    
    this.commonService.getSourceOfHire().subscribe(res => {
      this.response = res;
    
      switch (this.response.status) {
        case 1:
         this.sourceOfHireList = this.response.responseData || [];
          break;

        default:
          break;
      }
    });
  }
  getLocation(){
    this.commonService.getLocation().subscribe((res=>{
      if(res){
        this.response = res;
        this.LocationList=this.response.responseData || [];
       
      }else{

      }
    }));
  }
  getReportedTo(){
    this.commonService.getReportedTo().subscribe((res=>{
      if(res){
        this.response = res;
        this.ReportedToList=this.response.responseData || [];
        this.filteredRecordsreport.next(this.ReportedToList.slice());
      }else{

      }
    }));
  }

}
