import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { EmpServiceService } from '../../../services/emp-service.service';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { FormGroup, FormBuilder, Validators, NgForm, FormControl } from '@angular/forms';
import { EmployeeAccidentIncidents } from '../view-model/employee-accident-inc';
import { ActivatedRoute } from '@angular/router';
import { Paging } from 'projects/viewmodels/paging';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { NotificationService } from 'projects/core/src/projects';
import { LoaderService } from 'src/app/domain/services/loader/loader.service';
import { CommonService } from 'projects/lhs-service/src/lib/service/common/common.service';
import * as moment from 'moment';
import { merge, Subject, ReplaySubject } from 'rxjs';
import { tap, takeUntil } from 'rxjs/operators';
import { AppDateAdapter } from 'projects/lhs-directives/src/projects';
import { APP_DATE_FORMATS } from 'projects/lhs-directives/src/lib/directives/date-format.directive';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';
import { MatSelect } from '@angular/material/select';


@Component({
  selector: 'emp-accidents-incidents-details',
  templateUrl: './accidents-incidents-details.component.html',
  styleUrls: ['./accidents-incidents-details.component.scss'],
  providers: [
    {
        provide: DateAdapter, useClass: AppDateAdapter
    },
    {
        provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS
    }
    ]
})
export class AccidentsIncidentsDetailsComponent implements OnInit {
  responseModel: ResponseModel = {};
  getErrorMessage:'Please Enter Value';
  empAIModel: EmployeeAccidentIncidents = {};
  response: ResponseModel = {};
  rForm: FormGroup;
  rForm1: FormGroup;
  totalCount: number;
  paging: Paging = {};
  searchByName = null;
  searchByType = null;
  employeeAccidentsList: any;
  employeePrimaryInfo: {};
  AccidentInfoModel: AllEmployeeAccidentInfo = {};
  deleteAccidentId : number;
  AccidentData:AllEmployeeAccidentInfo[];
  EditList: EmployeeAccidentIncidents[] = [];
 
  employeeId = 0;
  //For Discussion
  displayedColumnsAccidentIncident: string[] = ['sr', 'eventTypeName','accidentDate','locationtype', 'locationName', 'raisedByName', 'reportedToName', 'briefDescription','actionTaken','incidentTimeName','createdDate','action'];
  dataSourceAccidentIncident : any; //= new MatTableDataSource(this.employeeAccidentsList);
  EditaccidentId: any;
  EventTypeList: any;
  LocationList:any;
  ReportedToList:any;
  RaisedList:any;
  RaisedByList:any;
  EditId: number;
  LocationTypeList: any;
  list: any;
  selectedType: any;
  seletctedvalue: any;
  applyFilter(event: Event) {    
    this.searchByType = (event.target as HTMLInputElement).value;
    this.GetList();
  }
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild('btnEditaccidentCancel') editCancel: ElementRef;
  @ViewChild('formDirective') private formDirective: NgForm;
  isShownOtherLocation: boolean = false ;
  isShownLocatiodropdown: boolean = false ;
  orderBy: number;
  orderColumn: number;
  TimeDropdownList : TimeList [] = ELEMENT_DATA;
  public control: FormControl = new FormControl();
  public searchcontrol: FormControl = new FormControl();
  private _onDestroy = new Subject<void>();
  public filteredRecords: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
  @ViewChild('Select') select: MatSelect;
  constructor(private fb: FormBuilder,private notification:NotificationService,private loaderService: LoaderService, private route: ActivatedRoute, private empService: EmpServiceService,
    private commonservice:CommonService) {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
   }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      // tslint:disable-next-line: radix
      this.employeeId = parseInt(params.Id);
      this.EditId = parseInt(params.EId);
    });
     this.createForm();
     this.createFormEdit();
     this.GetList();
     this.getEventtype();
    // this.getLocation();
     this.getReportedTo();
     this.getRaisedBy();
     this.getLocationType();
     this.searchlocationlist();
     this.searchlocationEdit();
     if(this.EditId>0){
      this.getaccidentById();
     }
  }
  ngAfterViewInit(): void {
   setTimeout(() => {
      this.dataSourceAccidentIncident !== undefined ? this.dataSourceAccidentIncident.sort = this.sort : this.dataSourceAccidentIncident;
      this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
      merge(this.sort.sortChange, this.paginator.page)
        .pipe(
          tap(() => this.GetList())
        )
        .subscribe();
    }, 2000);

  }
  getSortingOrder() {
    const sortColumn = this.sort.active == undefined ? 'createdDate' : this.sort.active;
    this.orderBy = this.sort.direction === 'asc' ? 0: 1;
   switch (sortColumn) {
      case 'eventTypeName':
        this.orderColumn = 0;
        break;
      case 'accidentDate':
        this.orderColumn = 1;
        break;
        case 'locationtype':
          this.orderColumn = 2;
          break;
        case 'locationName':
          this.orderColumn = 3;
          break;
        case 'raisedByName':
          this.orderColumn = 5;
          break;
        case 'reportedToName':
          this.orderColumn = 4;
          break;
        case 'briefDescription':
          this.orderColumn = 6;
          break;
        case 'actionTaken':
          this.orderColumn = 7;
          break;
        case 'incidentTimeName':
          this.orderColumn = 8;
          break;
        case 'createdDate':
          this.orderColumn = 9;
          break;
        default:
        break;
    }
  }
  getaccidentById(){
    const data = {
      Id: Number(this.EditId),
     
    };
    this.empService.getaccidentById(data).subscribe(res => {
      this.response = res;
         this.EditList = this.response.responseData;
         document.getElementById("openEditModalButton").click();
         this.EditaccidentId=this.EditList[0]['id'];
         this.rForm1.controls['accidentDateEdit'].setValue(this.EditList[0]['accidentDate']);
         this.rForm1.controls['eventTypeEdit'].setValue(this.EditList[0]['eventType']);
         this.rForm1.controls['raisedByEdit'].setValue(this.EditList[0]['raisedBy']);
         this.rForm1.controls['reportedToEdit'].setValue(this.EditList[0]['reportedTo']);
         this.rForm1.controls['briefDescriptionEdit'].setValue(this.EditList[0]['briefDescription']);
         this.rForm1.controls['detailedDescriptionEdit'].setValue(this.EditList[0]['detailedDescription']);
         this.rForm1.controls['locationtypeEdit'].setValue(this.EditList[0]['locationType']);
         this.rForm1.controls['ActionTakenEdit'].setValue(this.EditList[0]['actionTaken']);
         this.rForm1.controls['raisedByEdit'].disable();
         this.rForm1.controls['incidentTimeEdit'].setValue(this.EditList[0]['incidentTimeName']=="12:00 AM"?"24:00":this.EditList[0]['incidentTimeName']=="12:30 AM"?"00:30":this.EditList[0]['incidentTimeName']=="01:00 AM"?"01:00":this.EditList[0]['incidentTimeName']=="01:30 AM"?"01:30":
         this.EditList[0]['incidentTimeName']=="02:00 AM"?"02:00":this.EditList[0]['incidentTimeName']=="02:30 AM"?"02:30":this.EditList[0]['incidentTimeName']=="03:00 AM"?"03:00":this.EditList[0]['incidentTimeName']=="03:30 AM"?"03:30":
         this.EditList[0]['incidentTimeName']=="04:00 AM"?"04:00":this.EditList[0]['incidentTimeName']=="04:30 AM"?"04:30":this.EditList[0]['incidentTimeName']=="05:00 AM"?"05:00":this.EditList[0]['incidentTimeName']=="05:30 AM"?"05:30":
         this.EditList[0]['incidentTimeName']=="06:00 AM"?"06:00":this.EditList[0]['incidentTimeName']=="06:30 AM"?"06:30":this.EditList[0]['incidentTimeName']=="07:00 AM"?"07:00":this.EditList[0]['incidentTimeName']=="07:30 AM"?"07:30":
         this.EditList[0]['incidentTimeName']=="08:00 AM"?"08:00":this.EditList[0]['incidentTimeName']=="08:30 AM"?"08:30":this.EditList[0]['incidentTimeName']=="09:00 AM"?"09:00":this.EditList[0]['incidentTimeName']=="09:30 AM"?"09:30":
         this.EditList[0]['incidentTimeName']=="10:00 AM"?"10:00":this.EditList[0]['incidentTimeName']=="10:00 AM"?"10:30":this.EditList[0]['incidentTimeName']=="11:00 AM"?"11:00":this.EditList[0]['incidentTimeName']=="11:30 AM"?"11:30":
         this.EditList[0]['incidentTimeName']=="12:00 PM"?"12:00":this.EditList[0]['incidentTimeName']=="12:30 PM"?"12:30":this.EditList[0]['incidentTimeName']=="01:00 PM"?"13:00":
         this.EditList[0]['incidentTimeName']=="02:00 PM"?"14:00":this.EditList[0]['incidentTimeName']=="02:30 PM"?"14:30":this.EditList[0]['incidentTimeName']=="03:00 PM"?"15:00":this.EditList[0]['incidentTimeName']=="03:30 PM"?"15:30":
         this.EditList[0]['incidentTimeName']=="04:00 PM"?"16:00":this.EditList[0]['incidentTimeName']=="04:30 PM"?"16:30":this.EditList[0]['incidentTimeName']=="05:00 PM"?"17:00":this.EditList[0]['incidentTimeName']=="05:30 PM"?"17:30":
         this.EditList[0]['incidentTimeName']=="06:00 PM"?"18:00":this.EditList[0]['incidentTimeName']=="06:30 PM"?"18:30":this.EditList[0]['incidentTimeName']=="07:00 PM"?"19:00":this.EditList[0]['incidentTimeName']=="07:30 PM"?"19:30":
         this.EditList[0]['incidentTimeName']=="08:00 PM"?"20:00":this.EditList[0]['incidentTimeName']=="08:30 PM"?"20:30":this.EditList[0]['incidentTimeName']=="09:00 PM"?"21:00":this.EditList[0]['incidentTimeName']=="09:30 PM"?"21:30":
         this.EditList[0]['incidentTimeName']=="10:00 PM"?"22:00":this.EditList[0]['incidentTimeName']=="10:00 PM"?"22:30":this.EditList[0]['incidentTimeName']=="11:00 PM"?"23:00":this.EditList[0]['incidentTimeName']=="11:30 PM"?"23:30":"");
         if(this.EditList[0]['otherLocation']!=""){
          this.rForm1.controls['otherlocationEdit'].setValue(this.EditList[0]['otherLocation']);
          this.isShownOtherLocation=true;
          this.isShownLocatiodropdown=false;
        }
        else if(this.EditList[0]['locationId']!=null){
          this.getLocation();
          this.rForm1.controls['locationIdEdit'].setValue(this.EditList[0]['locationId']);
          this.isShownOtherLocation=false;
          this.isShownLocatiodropdown=true;
        }
        })
  }
  createForm() {
    this.rForm = this.fb.group({
      accidentDate: ['', Validators.required],
      eventType: ['', Validators.required],
      locationId: [null, Validators.nullValidator],
      raisedBy: ['', Validators.required],
      reportedTo: ['', Validators.required],
      briefDescription: ['', Validators.required],
      detailedDescription : ['', Validators.required],
      ActionTaken : ['', Validators.required],
      locationtype: ['', Validators.required],
      otherlocation: ['', Validators.nullValidator],
      incidentTime:['', Validators.required],
    });
  }
  createFormEdit() {
    this.rForm1 = this.fb.group({
    accidentDateEdit: ['', Validators.required],
    eventTypeEdit: ['', Validators.required],
    locationIdEdit: ['', Validators.required],
     raisedByEdit: ['', Validators.required],
     reportedToEdit: ['', Validators.required],
     briefDescriptionEdit: ['', Validators.required],
     detailedDescriptionEdit : ['', Validators.required],
     locationtypeEdit: ['', Validators.required],
     otherlocationEdit: ['', Validators.nullValidator],
     ActionTakenEdit: ['', Validators.nullValidator],
     incidentTimeEdit: ['', Validators.required],
    });
  }
  searchlocationlist(){
    this.control.valueChanges
    .pipe(takeUntil(this._onDestroy))
    .subscribe(() => {
      this.filterlocationlist();
    });
  }


 private filterlocationlist() {
    if (!this.LocationList) {
      return;
    }
    let search = this.control.value;
    if (!search) {
      this.filteredRecords.next(this.LocationList.slice());
      return;
    } else {
      search = search.toLowerCase();
    }
    if (search.length >= 1) {
     this.filteredRecords.next(
      this.LocationList.filter(ServiceType => ServiceType.name.toLowerCase().indexOf(search) > -1)
     );
    }
  }
  searchlocationEdit(){
    this.searchcontrol.valueChanges
    .pipe(takeUntil(this._onDestroy))
    .subscribe(() => {
      this.filterlocationEdit();
    });
  }
 
  private filterlocationEdit() {
    if (!this.LocationList) {
      return;
    }
    let search = this.searchcontrol.value;
    if (!search) {
      this.filteredRecords.next(this.LocationList.slice());
      return;
    } else {
      search = search.toLowerCase();
    }
    if (search.length >= 1) {
     this.filteredRecords.next(
      this.LocationList.filter(ServiceType => ServiceType.name.toLowerCase().indexOf(search) > -1)
     );
    }
  }
  get accidentDate() {
    return this.rForm.get('accidentDate');
  }
  get eventType() {
    return this.rForm.get('eventType');
  }
  get locationId() {
    return this.rForm.get('locationId');
  }
  get raisedBy() {
    return this.rForm.get('raisedBy');
  }

  get reportedTo() {
    return this.rForm.get('reportedTo');
  }

  get briefDescription() {
    return this.rForm.get('briefDescription');
  }

  get detailedDescription() {
    return this.rForm.get('detailedDescription');
  }
  get locationtype() {
    return this.rForm.get('locationtype');
  }
  get otherlocation() {
    return this.rForm.get('otherlocation');
  }
  
  get ActionTaken() {
    return this.rForm.get('ActionTaken');
  }
  get incidentTime() {
    return this.rForm.get('incidentTime');
  }
  get otherlocationEdit() {
    return this.rForm1.get('otherlocationEdit');
  }
  GetList() {
    this.getSortingOrder()
    const data = {
      pageNo: this.paging.pageNo,
      pageSize: this.paging.pageSize,
      SearchTextByType: this.searchByType,
      EmployeeId : this.employeeId,
      OrderBy: this.orderColumn,
      SortOrder: this.orderBy
    };
    this.empService.getAccidentIncidentList(data).subscribe(res => {
      console.log("res accident", res);
      this.response = res;
      this.totalCount = this.response.total;
      if (this.response.responseData) {
        this.employeeAccidentsList = [];
        this.response.responseData.forEach(element => {
          this.employeeAccidentsList.push({
            'Id': element.id,
            'employeeId': element.employeeId,
            'eventType': element.eventType,
            'locationId': element.locationId,
            'raisedBy': element.raisedBy,
            'reportedTo': element.reportedTo,
            'accidentDate': element.accidentDate,
            'briefDescription': element.briefDescription,
            'detailedDescription': element.detailedDescription,
            'eventTypeName': element.eventTypeName,
            'locationName': element.locationName,
            'reportedToName': element.reportedToName,
            'raisedByName': element.raisedByName,
            'createdDate':element.createdDate,
            'locationType':element.locationType,
            'otherLocation':element.otherLocation,
            'locationTypeName':element.locationTypeName,
            'actionTaken':element.actionTaken,
            'incidentTimeName':element.incidentTimeName,
            'incidentTime':element.incidentTime,
            'incidentTimeTake':element.incidentTimeTake
          });
          });
        
         this.dataSourceAccidentIncident = new MatTableDataSource(this.employeeAccidentsList);
         
      }
      else{
        this.dataSourceAccidentIncident = new MatTableDataSource(this.AccidentData);
      }
    });
}
PageIndexEvent(event: PageEvent) {
  this.paging.pageNo = event.pageIndex + 1;
  this.paging.pageSize = event.pageSize;
  this.GetList();
}
AddAccidentIncidentInfo() {  
  if (this.rForm.valid) {
    const data = {
      'EmployeeId': this.employeeId,
      'EventType': parseInt(this.eventType.value),
      'LocationId': parseInt(this.locationId.value),
      'RaisedBy': this.raisedBy.value,
      'ReportedTo': this.reportedTo.value,
      'AccidentDate': moment(this.accidentDate.value).format('YYYY-MM-DD').toString(),
      'BriefDescription': this.briefDescription.value,
      'DetailedDescription': this.detailedDescription.value,
      'LocationType': this.locationtype.value,
      'OtherLocation': this.otherlocation.value,
      'ActionTaken':this.ActionTaken.value,
      'IncidentTime':this.incidentTime.value,
    };
      this.empService.AddAccidentIncidentList(data).subscribe(res => {
      this.response = res;
      switch (this.response.status) {
        case 1:
        this.notification.Success({ message: this.response.message, title: null });
         this.GetList();
          this.rForm.reset();
          this.formDirective.resetForm();
          this.getRaisedBy();
          this.isShownLocatiodropdown=false;
          this.isShownOtherLocation=false;
          break;
          default:
          this.notification.Warning({ message: this.response.message, title: null });
          break;
      }
    });
  }
}

DeleteModal(accidentID,_e)
  {
    this.deleteAccidentId = accidentID;
  }
  DeleteAccidentInfo(event){
    this.AccidentInfoModel.Id=this.deleteAccidentId;
    this.empService.DeleteAccidentDetails(this.AccidentInfoModel).subscribe((data: any) => {
      if (data.status == 1) {
        this.GetList();
        this.notification.Success({ message: data.message, title: null });
        
      }
      else {
        this.notification.Error({ message: data.message, title: null });
      }

    })
  }
  openEditDetails(elem) {
    document.getElementById("openEditModalButton").click();
    this.EditaccidentId=elem.Id;
    this.rForm1.controls['accidentDateEdit'].setValue(elem.accidentDate);
    this.rForm1.controls['eventTypeEdit'].setValue(elem.eventType);
    this.rForm1.controls['raisedByEdit'].setValue(elem.raisedBy);
    this.rForm1.controls['reportedToEdit'].setValue(elem.reportedTo);
    this.rForm1.controls['briefDescriptionEdit'].setValue(elem.briefDescription);
    this.rForm1.controls['detailedDescriptionEdit'].setValue(elem.detailedDescription);
    this.rForm1.controls['raisedByEdit'].disable();
    this.rForm1.controls['locationtypeEdit'].setValue(elem.locationType);
    this.rForm1.controls['ActionTakenEdit'].setValue(elem.actionTaken);
    this.rForm1.controls['incidentTimeEdit'].setValue(elem.incidentTimeName=="12:00 AM"?"24:00":elem.incidentTimeName=="12:30 AM"?"00:30":elem.incidentTimeName=="01:00 AM"?"01:00":
    elem.incidentTimeName=="01:30 AM"?"01:30":
    elem.incidentTimeName=="02:00 AM"?"02:00":elem.incidentTimeName=="02:30 AM"?"02:30":elem.incidentTimeName=="03:00 AM"?"03:00":elem.incidentTimeName=="03:30 AM"?"03:30":
    elem.incidentTimeName=="04:00 AM"?"04:00":elem.incidentTimeName=="04:30 AM"?"04:30":elem.incidentTimeName=="05:00 AM"?"05:00":elem.incidentTimeName=="05:30 AM"?"05:30":
    elem.incidentTimeName=="06:00 AM"?"06:00":elem.incidentTimeName=="06:30 AM"?"06:30":elem.incidentTimeName=="07:00 AM"?"07:00":elem.incidentTimeName=="07:30 AM"?"07:30":
    elem.incidentTimeName=="08:00 AM"?"08:00":elem.incidentTimeName=="08:30 AM"?"08:30":elem.incidentTimeName=="09:00 AM"?"09:00":elem.incidentTimeName=="09:30 AM"?"09:30":
    elem.incidentTimeName=="10:00 AM"?"10:00":elem.incidentTimeName=="10:00 AM"?"10:30":elem.incidentTimeName=="11:00 AM"?"11:00":elem.incidentTimeName=="11:30 AM"?"11:30":
    elem.incidentTimeName=="12:00 PM"?"12:00":elem.incidentTimeName=="12:30 PM"?"12:30":elem.incidentTimeName=="01:00 PM"?"13:00":
    elem.incidentTimeName=="02:00 PM"?"14:00":elem.incidentTimeName=="02:30 PM"?"14:30":elem.incidentTimeName=="03:00 PM"?"15:00":elem.incidentTimeName=="03:30 PM"?"15:30":
    elem.incidentTimeName=="04:00 PM"?"16:00":elem.incidentTimeName=="04:30 PM"?"16:30":elem.incidentTimeName=="05:00 PM"?"17:00":elem.incidentTimeName=="05:30 PM"?"17:30":
    elem.incidentTimeName=="06:00 PM"?"18:00":elem.incidentTimeName=="06:30 PM"?"18:30":elem.incidentTimeName=="07:00 PM"?"19:00":elem.incidentTimeName=="07:30 PM"?"19:30":
    elem.incidentTimeName=="08:00 PM"?"20:00":elem.incidentTimeName=="08:30 PM"?"20:30":elem.incidentTimeName=="09:00 PM"?"21:00":elem.incidentTimeName=="09:30 PM"?"21:30":
    elem.incidentTimeName=="10:00 PM"?"22:00":elem.incidentTimeName=="10:00 PM"?"22:30":elem.incidentTimeName=="11:00 PM"?"23:00":elem.incidentTimeName=="11:30 PM"?"23:30":"");
     if(elem.otherLocation!=""){
      this.rForm1.controls['otherlocationEdit'].setValue(elem.otherLocation);
      this.isShownOtherLocation=true;
      this.isShownLocatiodropdown=false;
    }
    else if(elem.locationId!=null){
      this.getLocation();
      this.rForm1.controls['locationIdEdit'].setValue(elem.locationId);
      this.isShownOtherLocation=false;
      this.isShownLocatiodropdown=true;
    }
  }
  
  updateaccidentinfo() {
    if (this.rForm1.valid) {
      const data = {
        'Id': this.EditaccidentId,
        'EventType': parseInt(this.rForm1.get('eventTypeEdit').value),
        'LocationId':parseInt(this.rForm1.get('locationIdEdit').value), 
        'RaisedBy': this.rForm1.get('raisedByEdit').value,
        'ReportedTo': this.rForm1.get('reportedToEdit').value,
        'AccidentDate': moment(this.rForm1.get('accidentDateEdit').value).format('YYYY-MM-DD').toString(),
        'BriefDescription': this.rForm1.get('briefDescriptionEdit').value,
        'DetailedDescription': this.rForm1.get('detailedDescriptionEdit').value,
        'LocationType': this.rForm1.get('locationtypeEdit').value,
         'OtherLocation': this.rForm1.get('otherlocationEdit').value,
         'ActionTaken': this.rForm1.get('ActionTakenEdit').value,
         'IncidentTime': this.rForm1.get('incidentTimeEdit').value,
      };
       this.empService.EditAccidentIncidentList(data).subscribe(res => {
       this.response = res;
          switch (this.response.status) {
          case 1:
          this.GetList();
          this.notification.Success({ message: this.response.message, title: null });
           this.employeeAccidentsList.push(this.response.responseData);
           this.editCancel.nativeElement.click();
            break;
            default:
            break;
        }
      });
    }
  }
  getEventtype(){
    this.commonservice.getEventType().subscribe((res=>{
      if(res){
        this.response = res;
        this.EventTypeList=this.response.responseData||[];
       
      }else{

      }
    }));
  }
  getLocation(){
    this.commonservice.getLocation().subscribe((res=>{
      if(res){
        this.response = res;
        this.LocationList=this.response.responseData||[];
        this.filteredRecords.next(this.LocationList.slice());
      }else{

      }
    }));
  }
  getLocationType(){
    this.commonservice.getLocationType().subscribe((res=>{
      if(res){
        this.response = res;
        this.LocationTypeList=this.response.responseData||[];
       
      }else{

      }
    }));
  }
  getReportedTo(){
    this.commonservice.getReportedTo().subscribe((res=>{
      if(res){
        this.response = res;
        this.ReportedToList=this.response.responseData||[];
       
      }else{

      }
    }));
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
      this.rForm.controls['locationId'].patchValue(0);
    }
  }
  selectChangeHandlerEdit(event:any) {
    this.list=this.LocationTypeList;
    this.selectedType = event;
   if(this.selectedType==5){
      this.getLocation()
      this.isShownLocatiodropdown=true;
      this.isShownOtherLocation=false;
      this.rForm1.controls['otherlocationEdit'].patchValue("");
    }
    else{
      this.isShownLocatiodropdown=false;
      this.isShownOtherLocation=true;
      this.rForm1.controls['locationIdEdit'].patchValue(0);
    }
  }
  getRaisedBy(){
    const data = {
      EmployeeId: (this.employeeId),
    }
    this.commonservice.getRaisedBy(data).subscribe((res=>{
      if(res){
        this.response = res;
        this.RaisedByList=this.response.responseData||[];
        this.rForm.get('raisedBy').setValue(this.RaisedByList[0]["id"]);
        this.rForm.get('raisedBy').disable();
      }else{

      }
    }));
  }
  formattedaddress=" "; 
  public handleAddressChange(addressObj: any) {
    this.formattedaddress=addressObj.address1
    this.rForm.controls['otherlocation'].setValue(this.formattedaddress);
   
  }
  formattedaddress1=" "; 
  public handleAddressChangeEdit(addressObj: any) {
    this.formattedaddress1=addressObj.address1
    this.rForm1.controls['otherlocationEdit'].setValue(this.formattedaddress1);
   
  }
}
export interface AllEmployeeAccidentInfo {
  Id?: number;
}
const ELEMENT_DATA: TimeList[] = [
  {id: '24:00', text: '12:00 AM '},
  {id: '00:30', text: '12:30 AM '},
  {id: '01:00', text: '1:00 AM '},
  {id: '01:30', text: '1:30 AM '},
  {id: '02:00', text: '2:00 AM '},
  {id: '02:30', text: '2:30 AM '},
  {id: '03:00', text: '3:00 AM '},
  {id: '03:30', text: '3:30 AM '},
  {id: '04:00', text: '4:00 AM '},
  {id: '04:30', text: '4:30 AM '},
  {id: '05:00', text: '5:00 AM '},
  {id: '05:30', text: '5:30 AM '},
  {id: '06:00', text: '6:00 AM '},
  {id: '06:30', text: '6:30 AM '},
  {id: '07:00', text: '7:00 AM '},
  {id: '07:30', text: '7:30 AM '},
  {id: '08:00', text: '8:00 AM '},
  {id: '08:30', text: '8:30 AM '},
  {id: '09:00', text: '9:00 AM '},
  {id: '09:30', text: '9:30 AM '},
  {id: '10:00', text: '10:00 AM '},
  {id: '10:30', text: '10:30 AM '},
  {id: '11:00', text: '11:00 AM '},
  {id: '11:30', text: '11:30 AM '},
  {id: '12:00', text: '12:00 PM '},
  {id: '12:30', text: '12:30 PM '},
  {id: '13:00', text: '1:00 PM '},
  {id: '13:30', text: '1:30 PM '},
  {id: '14:00', text: '2:00 PM '},
  {id: '14:30', text: '2:30 PM '},
  {id: '15:00', text: '3:00 PM '},
  {id: '15:30', text: '3:30 PM '},
  {id: '16:00', text: '4:00 PM '},
  {id: '16:30', text: '4:30 PM '},
  {id: '17:00', text: '5:00 PM '},
  {id: '17:30', text: '5:30 PM '},
  {id: '18:00', text: '6:00 PM '},
  {id: '18:30', text: '6:30 PM '},
  {id: '19:00', text: '7:00 PM '},
  {id: '19:30', text: '7:30 PM '},
  {id: '20:00', text: '8:00 PM '},
  {id: '20:30', text: '8:30 PM '},
  {id: '21:00', text: '9:00 PM '},
  {id: '21:30', text: '9:30 PM '},
  {id: '22:00', text: '10:00 PM '},
  {id: '22:30', text: '10:30 PM '},
  {id: '23:00', text: '11:00 PM '},
  {id: '23:30', text: '11:30 PM '},
];
export interface TimeList {
  id?: string;
  text?: string;
}

