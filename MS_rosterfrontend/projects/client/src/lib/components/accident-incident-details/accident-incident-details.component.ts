import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
//import { EmpServiceService } from '../../../services/emp-service.service';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { FormGroup, FormBuilder, Validators, NgForm, FormControl } from '@angular/forms';
//import { EmployeeAccidentIncidents } from '../view-model/employee-accident-inc';
import { ActivatedRoute } from '@angular/router';
import { Paging } from 'projects/viewmodels/paging';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { NotificationService } from 'projects/core/src/projects';
import { LoaderService } from 'src/app/domain/services/loader/loader.service';
import { CommonService } from 'projects/lhs-service/src/lib/service/common/common.service';
import { ClientService } from '../../services/client.service';
import { ClientAccidentIncidents, IncidentImpactedPerson, IncidentAllegation, IncidentAttachment } from '../../view-models/client-accident-inc';
import * as moment from 'moment';
import { APP_DATE_FORMATS, AppDateAdapter } from 'projects/lhs-directives/src/lib/directives/date-format.directive';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';
import { environment } from 'src/environments/environment';
import { Subject, ReplaySubject } from 'rxjs';
import { MatSelect } from '@angular/material/select';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'lib-accident-incident-details',
  templateUrl: './accident-incident-details.component.html',
  styleUrls: ['./accident-incident-details.component.scss'],
  providers: [
    {
        provide: DateAdapter, useClass: AppDateAdapter
    },
    {
        provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS
    }
    ]
})
export class AccidentIncidentDetailsComponent implements OnInit {
  responseModel: ResponseModel = {};
  getErrorMessage:'Please Enter Value';
  empAIModel: ClientAccidentIncidents = {};
  ImpactedModel: IncidentImpactedPerson = {};
  AllegationModel: IncidentAllegation = {};
  response: ResponseModel = {};
  rForm: FormGroup;
  rForm1: FormGroup;
  rFormProvider: FormGroup;
  rFormcontact: FormGroup;
  rFormCategory: FormGroup;
  rFormIncidentInfo: FormGroup;
  rFormImpacted: FormGroup;
  rFormAllegation: FormGroup;
  rFormAction: FormGroup;
  rFormRisk: FormGroup;
  rFormDeclaration: FormGroup;
  rFormattach: FormGroup;
  totalCount: number;
  paging: Paging = {};
  searchByName = null;
  searchByType = null;
  employeeAccidentsList: any;
  employeePrimaryInfo: {};
  AccidentInfoModel: AllEmployeeAccidentInfo = {};
  deleteAccidentId : number;
  AccidentData:AllEmployeeAccidentInfo[];
  EditList: ClientAccidentIncidents[] = [];
  selectedReportedTo = 'rtperson-1';
  employeeId = 0;
  clientId=0;
  requiredComplianceModel: IncidentAttachment[] = [];
  requiredCompliancedata: IncidentAttachment[];
  displayedColumnsRequired: string[] = ['documentName', 'action'];
  dataSourceRequired = new MatTableDataSource(this.requiredComplianceModel);
  displayedColumnsAccidentIncident: string[] = ['sr','employeeName', 'eventTypeName','locationtype', 'locationName', 'departmentName', 'reportedbyName', 'accidentDate','action'];
  dataSourceAccidentIncident : any; //= new MatTableDataSource(this.employeeAccidentsList);
  TimeDropdownList : TimeList [] = ELEMENT_DATA;
  EditaccidentId: any;
  EventTypeList: any;
  LocationList:any;
  ReportedToList:any;
  ReportedList:any;
  RaisedList:any;
  RaisedByList:any;
  departmentList: any;
  selecetedId: any;
  EditId: number;
  orderBy: number;
  orderColumn: number;
  LocationTypeList: any;
  list: any;
  selectedType: any;
  StateList: [];
  PrimaryTypeList: any;
  SecondaryList: any;
  AssignedToShow: any;
  Primarylist: any;
  Secondarylist: any;
  otherdisability: any;
  primarydisability: any;
  genderList: any;
  behaviourlist: any;
  communicationlist: any;
  Primarydislist: any;
  otherdislist: any;
  ImpactConcernlist: any;
  ImpactCommunication: any;
  ImpactConcerndata: any;
  PrimaryAllegation: any;
  otherAllegation: any;
  AllegationCommunication: any;
  AllegationConcerndata: any;
  textDeclaration: string;
  textRisk: string;
  textImmediate: string;
  textSubject: string;
  textImpacted: string;
  textIncident: string;
  textCategory: string;
  textContact: string;
  textProvider: string;
  ImageName: any;
  ImageSize: any;
  EditdocId: any;
  isShownBrowse: boolean = true ; 
  isShownUrl: boolean = false ;
  isShownReport: boolean = false ;
  Imageurl: string;
  txtAttachButton: string;
  ReportedBy: any;
  ProviderName: any;
  RegistrationId: any;
  ProviderABN: any;
  OutletName: any;
  RegistrationGroup: any;
  State: any;
  ContactTitle: any;
  ContactFirstName: any;
  ContactMiddleName: any;
  ContactLastName: any;
  ContactProvider: any;
  ContactPhoneNo: any;
  ContactEmail: any;
  ContactMethod: any;
  IsIncidentAnticipatedInfo: string;
  PrimaryCategoryName: any;
  SecondaryCategoryName: any;
  locationName: any;
  otherlocationInfo: any;
  circumstanceIncidentinfo: any;
  locationtypeinfo: any;
  Incidentdateinfo: any;
  Reasoninfo: any;
  NdisProviderTimeinfo: any;
  NdisProviderDateinfo: any;
  DescribeIncidentinfo: any;
  ImpactedPrimaryDisbilityinfo: any;
  ImpactedSecondaryDisbilityinfo: any;
  ImpactedCommunicationinfo: any;
  ImpactedBehaviourinfo: any;
  ImpactedTitleinfo: any;
  ImpactedName: any;
  ImpactedNdisNoinfo: any;
  ImpactedGenderinfo: any;
  ImpactedDOBinfo: any;
  ImpactedPhoneNo: any;
  ImpactedEmail: any;
  OtherDetail: any;
  ImpactedSecondaryDisabilityinfo: any;
  DisablePrimaryDisability: any;
  DisableOtherDisability: any;
  DisableCommunication: any;
  DisableBehaviour: any;
  WorkerTitle: any;
  WorkerName: any;
  WorkerPosition: any;
  WorkerGender: any;
  WorkerDOB: any;
  WorkerPhoneNo: any;
  WorkerEmail: any;
  AllegationOtherDetail: any;
  IsSubjectAllegationInfo: string;
  DeclarationDate: any;
  DeclarationName: any;
  DeclarationPosition: any;
  TobeFinished: any;
  InProgressRisk: any;
  NoRiskAssesmentInfo: any;
  RiskDetails: any;
  RiskAssesmentDate: any;
  IsRiskAssesmentInfo: string;
  DescribeDisability: any;
  WorkerDescribe: any;
  DescribeImmediate: any;
  ChildContacted: any;
  Guardian: any;
  IsUnder18Info: string;
  DisableTitle: any;
  DisableGender: any;
  DisableNdisNo: any;
  DisableName: any;
  DisableDOBirth: any;
  DisablePhoneNo: any;
  DisableEmail: any;
  OtherName: any;
  OtherRelationShip: any;
  OtherGender: any;
  OtherDOB: any;
  OtherPhoneNo: any;
  OtherEmail: any;
  OtherTitle: any;
  IsPoliceInformed: string;
  OfficerName: any;
  PoliceStation: any;
  PoliceNumber: any;
  PoliceNotInform: any;
  IsFamilyAwareInfo: string;
  ReportText: string;
  selectedName: any;
  todayDatemax = new Date();
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild('btnEditaccidentCancel') editCancel: ElementRef;
  @ViewChild('formDirective') private formDirective: NgForm;
  @ViewChild('fileInput', {static: false}) fileInput: ElementRef;
  public controlPrimary: FormControl = new FormControl();
  public controlSecondary: FormControl = new FormControl();
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
  isShownOtherLocation: boolean = false ;
  isShownLocatiodropdown: boolean = false ;
  isShownLocation: boolean = false ;
  baseUrl : string = environment.baseUrl;
  ShiftId: number;
  EmpId: number;
  constructor(private fb: FormBuilder,private notification:NotificationService, private route: ActivatedRoute,private clientservice:ClientService,
    private commonservice:CommonService) {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
   }
   
   ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
    this.clientId = parseInt(params.Id);
    this.ShiftId = Number(params['ShiftId']); 
    this.EmpId = Number(params['EmpId']); 
   });
     this.getEventtype();
     this.getLocationType();
     this.createFormContact();
     this.createFormProvider();
     this.createFormCategory();
     this.createFormIncident();
     this.createFormImpacted();
     this.createSubjectAllegation();
     this.createFormAction();
     this.createFormRisk();
     this.CreateFormDeclaration();
     this.CreateFormAttach();
     this.getState();
     this.getPrimaryCategory();
     this.getSecondaryCategory();
     this.getgender();
     this.getotherdisability();
     this.getPrimarydisability();
     this.getconcernbehaviour();
     this.getcommunication();
     this.setvalue();
     this.GetAllAccidentIncidentInfo();
     this.searchPrimarycategorytype();
     this.searchSecondarycategorytype();
     this.searchcategoryBehaviour();
     this.searchcategoryPrimaryDis();
     this.searchcategoryOtherDis();
     this.searchcategoryCommunication();
  }
  get otherlocation() {
    return this.rFormIncidentInfo.get('otherlocation');
  }
  setvalue(){
    // this.rFormCategory.controls['IsIncidentAnticipated'].setValue("2");
    this.rFormAllegation.controls['IsSubjectAllegation'].setValue("2");
    this.rFormAction.controls['IsFamilyAware'].setValue("2");
    this.rFormAction.controls['IsUnder18'].setValue("2");
    this.rFormAction.controls['IsPoliceInformed'].setValue("2");
    this.rFormRisk.controls['IsRiskAssesment'].setValue("2");
    this.txtAttachButton="Submit";   
  }
  createFormProvider() {
    this.rFormProvider = this.fb.group({
      ReportedBy: ['', Validators.required],
      ProviderName: ['', Validators.required],
      ProviderABN: ['', Validators.required],
      OutletName: ['', Validators.required],
      RegistrationGroup: ['', Validators.nullValidator],
      State: [null, Validators.required], 
      RegistrationId: ['', Validators.required],
      
    });
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
  createFormCategory() {
    this.rFormCategory = this.fb.group({
      PrimaryCategory: ['', Validators.required],
      SecondaryCategory: ['', Validators.required],
      IsIncidentAnticipated: ['', Validators.nullValidator],
     
    });
  }
  createFormIncident() {
    this.rFormIncidentInfo = this.fb.group({
     locationId: ['', Validators.nullValidator],
     locationtype: ['', Validators.required],
     otherlocation: ['', Validators.nullValidator],
     Location: ['', Validators.nullValidator],
     Incidentdate: ['', Validators.required],
     Reason: ['', Validators.nullValidator],
     NdisProviderTime: ['', Validators.required],
     NdisProviderDate: ['', Validators.required],
     DescribeIncident: ['', Validators.required],
     circumstanceIncident: ['', Validators.required],
    });
  }
  createFormAction() {
    this.rFormAction = this.fb.group({
      IsPoliceInformed: ['', Validators.nullValidator],
      OfficerName: ['', Validators.nullValidator],
      PoliceStation: ['', Validators.nullValidator],
      PoliceNumber: ['', Validators.nullValidator],
      PoliceNotInform: [null, Validators.nullValidator],
      IsFamilyAware: ['', Validators.nullValidator],
      Guardian: ['', Validators.required],
      IsUnder18: ['', Validators.nullValidator],
      ChildContacted: ['', Validators.required],
      DescribeImmediate: ['', Validators.required],
      WorkerDescribe: ['', Validators.required],
      DescribeDisability: ['', Validators.required],
    });
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
  createSubjectAllegation() {
    this.rFormAllegation = this.fb.group({
      WorkerTitle: ['', Validators.required],
      WorkerFirstName: ['', Validators.required],
      WorkerLastName: ['', Validators.required],
      WorkerPhoneNo: ['',[Validators.maxLength(16),Validators.required]],
      WorkerEmail: [null, Validators.compose([Validators.nullValidator, Validators.pattern(/^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/)])],
      WorkerGender: ['', Validators.required],
      IsSubjectAllegation: ['', Validators.nullValidator],
      WorkerDOB: ['', Validators.required],
      WorkerPosition: ['', Validators.nullValidator],
      DisableTitle: ['', Validators.required],
      DisableFirstName: ['', Validators.required],
      DisableLastName: ['', Validators.required],
      DisablePhoneNo: ['',[Validators.maxLength(16),Validators.nullValidator]],
      DisableEmail: [null, Validators.compose([Validators.nullValidator, Validators.pattern(/^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/)])],
      DisableGender: ['', Validators.required],
      DisableDOBirth: [null, Validators.required],
      DisableNdisNo: ['', Validators.required],
      DisablePrimaryDisability: ['', Validators.required],
      DisableOtherDisability: ['', Validators.nullValidator],
      DisableBehaviour: ['', Validators.required],
      DisableCommunication: ['', Validators.required],
      AllegationOtherDetail: ['', Validators.nullValidator],
      OtherTitle: ['', Validators.nullValidator],
      OtherFirstName: ['', Validators.nullValidator],
      OtherLastName: ['', Validators.nullValidator],
      OtherPhoneNo: ['',[Validators.maxLength(16),Validators.nullValidator]],
      OtherEmail: [null, Validators.compose([Validators.nullValidator, Validators.pattern(/^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/)])],
      OtherGender: ['', Validators.nullValidator],
      OtherDOB: ['', Validators.nullValidator],
      OtherRelationShip: ['', Validators.nullValidator],
      
    });
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
  CreateFormDeclaration() {
    this.rFormDeclaration = this.fb.group({
      IsDeclaration: ['', Validators.nullValidator],
      DeclarationName: ['', Validators.required],
      DeclarationPosition: ['', Validators.required],
      DeclarationDate: ['', Validators.required],
      
    });
  }
  CreateFormAttach() {
    this.rFormattach = this.fb.group({
      AddDocument: ['', Validators.required],
     });
  }
  getDepartments(selecetedId){
   
    this.commonservice.getDepartmentbyemployee(selecetedId).subscribe(res => {
      this.response = res;
       switch (this.response.status) {
        case 1:
         this.departmentList = this.response.responseData || [];
         this.rForm.get('departmentId').setValue(this.departmentList[0]["id"]);
         this.rForm.get('departmentId').disable();
          break;

        default:
          break;
      }
    });
  }
   getLocationType(){
    this.commonservice.getLocationType().subscribe((res=>{
      if(res){
        this.response = res;
        this.LocationTypeList=this.response.responseData||[];
       }
    }));
  }
   formattedaddress=" "; 
   public handleAddressChange(address: any) {
   this.formattedaddress=address.address1
   this.rFormIncidentInfo.controls['otherlocation'].setValue(this.formattedaddress);
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
     this.filteredRecordsSecondary.next(
      this.SecondaryList.filter(Type => Type.codeDescription.toLowerCase().indexOf(search) > -1)
     );
    }
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
  GetList() {
   const data = {
      pageNo: this.paging.pageNo,
      pageSize: this.paging.pageSize,
      SearchTextByType: this.searchByType,
      ClientId : this.clientId,
      ShiftId : this.ShiftId,
      OrderBy: this.orderColumn,
      SortOrder: this.orderBy
    };
    this.clientservice.GetClientAccidentIncidentList(data).subscribe(res => {
    this.response = res;
    this.totalCount = this.response.total;
    if (this.response.responseData) {
    this.employeeAccidentsList = [];
    this.response.responseData.forEach(element => {
    this.employeeAccidentsList.push({
      'Id': element.id,
      'employeeId': element.employeeId,
      'eventType': element.incidentType,
      'locationId': element.locationId,
      'departmentId': element.departmentId,
      'reportedBy': element.reportedBy,
      'accidentDate': element.accidentDate,
      'incidentCause': element.incidentCause,
      'followUp': element.followUp,
      'eventTypeName': element.eventTypeName,
      'locationName': element.locationName,
      'departmentName': element.departmentName,
      'reportedbyName': element.reportedByName,
      'employeeName':element.employeeName,
      'policeNotified':element.policeNotified,
      'phoneNo':element.phoneNo,
      'locationType':element.locationType,
      'otherLocation':element.otherLocation,
      'locationTypeName':element.locationTypeName,
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

  selectChangeHandler(event:any) {
    this.list=this.LocationTypeList;
    this.selectedType = event;
   if(this.selectedType==5){
      this.getLocation()
      this.isShownLocatiodropdown=true;
      this.isShownOtherLocation=false;
      this.rFormIncidentInfo.controls['otherlocation'].patchValue("");
    }
    else{
      this.isShownLocatiodropdown=false;
      this.isShownOtherLocation=true;
      this.rFormIncidentInfo.controls['locationId'].patchValue(0);
    }
  }

  SelectOtherLocation(event:any) {
    this.list=this.LocationList;
    this.selectedType = event;
    const index = this.list.findIndex(x => x.locationId == this.selectedType);
    this.selectedName= this.list[index].name;
    if(this.selectedName=="Other"){
      this.isShownLocation=true;
     
    }
    else{
      this.isShownLocation=false;
      this.rFormIncidentInfo.controls['Location'].patchValue("");
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
       
      }else{

      }
    }));
   }
   getReportedBy(selecetedId){
    this.commonservice.getReportedbyemployee(selecetedId).subscribe((res=>{
      if(res){
        this.response = res;
        this.ReportedList=this.response.responseData||[];
        this.rForm.get('reportedTo').setValue(this.ReportedList[0]["id"]);
        this.rForm.get('reportedTo').disable();
        this.rForm.get('phoneNo').setValue(this.ReportedList[0]["phoneNo"]);
        this.rForm.get('phoneNo').disable();
      }else{

      }
    }));
   }
  
   selected($event) {
    this.selecetedId= $event.value ;
    this.getDepartments(this.selecetedId);
    this.getReportedBy(this.selecetedId);
   }
   GetAllAccidentIncidentInfo() {
      const data = {
       Id : this.clientId,
       ShiftId:this.ShiftId
     };
     debugger
     this.clientservice.GetAllIncidentDetails(data).subscribe(res => {
     this.response = res;
     if (this.response.status > 0) {
     this.totalCount = this.response.total;
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
    if(this.response.responseData.clientIncidentDetails.clientId>0){
      this.rFormIncidentInfo.controls['locationtype'].patchValue(this.response.responseData.clientIncidentDetails.locationType);
      this.rFormIncidentInfo.controls['Incidentdate'].patchValue(this.response.responseData.clientIncidentDetails.dateTime);
      this.rFormIncidentInfo.controls['Reason'].patchValue(this.response.responseData.clientIncidentDetails.unknowndateReason);
      this.rFormIncidentInfo.controls['NdisProviderTime'].patchValue(this.response.responseData.clientIncidentDetails.startTimeString=="12:00 AM"?"24:00":this.response.responseData.clientIncidentDetails.startTimeString=="12:30 AM"?"00:30":this.response.responseData.clientIncidentDetails.startTimeString=="01:00 AM"?"01:00":
      this.response.responseData.clientIncidentDetails.startTimeString=="02:00 AM"?"02:00":this.response.responseData.clientIncidentDetails.startTimeString=="02:30 AM"?"02:30":this.response.responseData.clientIncidentDetails.startTimeString=="03:00 AM"?"03:00":this.response.responseData.clientIncidentDetails.startTimeString=="03:30 AM"?"03:30":
      this.response.responseData.clientIncidentDetails.startTimeString=="04:00 AM"?"04:00":this.response.responseData.clientIncidentDetails.startTimeString=="04:30 AM"?"04:30":this.response.responseData.clientIncidentDetails.startTimeString=="05:00 AM"?"05:00":this.response.responseData.clientIncidentDetails.startTimeString=="05:30 AM"?"05:30":
      this.response.responseData.clientIncidentDetails.startTimeString=="06:00 AM"?"06:00":this.response.responseData.clientIncidentDetails.startTimeString=="06:30 AM"?"06:30":this.response.responseData.clientIncidentDetails.startTimeString=="07:00 AM"?"07:00":this.response.responseData.clientIncidentDetails.startTimeString=="07:30 AM"?"07:30":
      this.response.responseData.clientIncidentDetails.startTimeString=="08:00 AM"?"08:00":this.response.responseData.clientIncidentDetails.startTimeString=="08:30 AM"?"08:30":this.response.responseData.clientIncidentDetails.startTimeString=="09:00 AM"?"09:00":this.response.responseData.clientIncidentDetails.startTimeString=="09:30 AM"?"09:30":
      this.response.responseData.clientIncidentDetails.startTimeString=="10:00 AM"?"10:00":this.response.responseData.clientIncidentDetails.startTimeString=="10:00 AM"?"10:30":this.response.responseData.clientIncidentDetails.startTimeString=="11:00 AM"?"11:00":this.response.responseData.clientIncidentDetails.startTimeString=="11:30 AM"?"11:30":
      this.response.responseData.clientIncidentDetails.startTimeString=="12:00 PM"?"12:00":this.response.responseData.clientIncidentDetails.startTimeString=="12:30 PM"?"12:30":this.response.responseData.clientIncidentDetails.startTimeString=="01:00 PM"?"13:00":
      this.response.responseData.clientIncidentDetails.startTimeString=="02:00 PM"?"14:00":this.response.responseData.clientIncidentDetails.startTimeString=="02:30 PM"?"14:30":this.response.responseData.clientIncidentDetails.startTimeString=="03:00 PM"?"15:00":this.response.responseData.clientIncidentDetails.startTimeString=="03:30 PM"?"15:30":
      this.response.responseData.clientIncidentDetails.startTimeString=="04:00 PM"?"16:00":this.response.responseData.clientIncidentDetails.startTimeString=="04:30 PM"?"16:30":this.response.responseData.clientIncidentDetails.startTimeString=="05:00 PM"?"17:00":this.response.responseData.clientIncidentDetails.startTimeString=="05:30 PM"?"17:30":
      this.response.responseData.clientIncidentDetails.startTimeString=="06:00 PM"?"18:00":this.response.responseData.clientIncidentDetails.startTimeString=="06:30 PM"?"18:30":this.response.responseData.clientIncidentDetails.startTimeString=="07:00 PM"?"19:00":this.response.responseData.clientIncidentDetails.startTimeString=="07:30 PM"?"19:30":
      this.response.responseData.clientIncidentDetails.startTimeString=="08:00 PM"?"20:00":this.response.responseData.clientIncidentDetails.startTimeString=="08:30 PM"?"20:30":this.response.responseData.clientIncidentDetails.startTimeString=="09:00 PM"?"21:00":this.response.responseData.clientIncidentDetails.startTimeString=="09:30 PM"?"21:30":
      this.response.responseData.clientIncidentDetails.startTimeString=="10:00 PM"?"22:00":this.response.responseData.clientIncidentDetails.startTimeString=="10:00 PM"?"22:30":this.response.responseData.clientIncidentDetails.startTimeString=="11:00 PM"?"23:00":this.response.responseData.clientIncidentDetails.startTimeString=="11:30 PM"?"23:30":"");
      this.rFormIncidentInfo.controls['NdisProviderDate'].patchValue(this.response.responseData.clientIncidentDetails.ndisProviderDate);
      this.rFormIncidentInfo.controls['DescribeIncident'].patchValue(this.response.responseData.clientIncidentDetails.incidentAllegtion);
      this.rFormIncidentInfo.controls['circumstanceIncident'].patchValue(this.response.responseData.clientIncidentDetails.allegtionCircumstances);
      if(this.response.responseData.clientIncidentDetails.otherLocation!="" && this.response.responseData.clientIncidentDetails.otherLocation!=null && this.response.responseData.clientIncidentDetails.otherLocation!=undefined){
        this.rFormIncidentInfo.controls['Location'].setValue(this.response.responseData.clientIncidentDetails.otherLocation);
        this.isShownLocation=true;
        
      }
      if(this.response.responseData.clientIncidentDetails.address!="" && this.response.responseData.clientIncidentDetails.address!=null && this.response.responseData.clientIncidentDetails.address!=undefined){
        this.rFormIncidentInfo.controls['otherlocation'].setValue(this.response.responseData.clientIncidentDetails.address);
        this.isShownOtherLocation=true
        this.isShownLocatiodropdown=false;
      }
       else if (this.response.responseData.clientIncidentDetails.locationId!=null&&this.response.responseData.clientIncidentDetails.locationId>0){
         this.getLocation();
         this.rFormIncidentInfo.controls['locationId'].patchValue(this.response.responseData.clientIncidentDetails.locationId);
        this.isShownOtherLocation=false;
        this.isShownLocatiodropdown=true;
      }
      this.textIncident="Update";
    }
    else{
     this.textIncident="Submit"
    }
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
    if(this.response.responseData.incidentImmediateAction.clientId>0){
      this.rFormAction.controls['IsPoliceInformed'].patchValue(this.response.responseData.incidentImmediateAction.isPoliceInformed==true?"1":"2");
      this.rFormAction.controls['OfficerName'].patchValue(this.response.responseData.incidentImmediateAction.officerName);
      this.rFormAction.controls['PoliceStation'].patchValue(this.response.responseData.incidentImmediateAction.policeStation);
      this.rFormAction.controls['PoliceNumber'].patchValue(this.response.responseData.incidentImmediateAction.policeNo);
      this.rFormAction.controls['PoliceNotInform'].patchValue(this.response.responseData.incidentImmediateAction.providerPosition);
      this.rFormAction.controls['IsFamilyAware'].patchValue(this.response.responseData.incidentImmediateAction.isFamilyAware==1?"1":this.response.responseData.incidentImmediateAction.isFamilyAware==2?"2":this.response.responseData.incidentImmediateAction.isFamilyAware==3?"3":"")
      this.rFormAction.controls['IsUnder18'].patchValue(this.response.responseData.incidentImmediateAction.isUnder18==1?"1":this.response.responseData.incidentImmediateAction.isUnder18==2?"2":this.response.responseData.incidentImmediateAction.isUnder18==3?"3":this.response.responseData.incidentImmediateAction.isUnder18==4?"4":"")
      this.rFormAction.controls['Guardian'].patchValue(this.response.responseData.incidentImmediateAction.contacttoFamily);
      this.rFormAction.controls['ChildContacted'].patchValue(this.response.responseData.incidentImmediateAction.contactChildProtection);
      this.rFormAction.controls['DescribeImmediate'].patchValue(this.response.responseData.incidentImmediateAction.disabilityPerson);
      this.rFormAction.controls['WorkerDescribe'].patchValue(this.response.responseData.incidentImmediateAction.subjectWorkerAllegation);
      this.rFormAction.controls['DescribeDisability'].patchValue(this.response.responseData.incidentImmediateAction.subjectDisabilityPerson);
      this.textImmediate="Update";
    }
    else{
     this.textImmediate="Submit"
    }
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
    this.requiredComplianceModel = this.response.responseData.incidentDocumentDetailModel;
    if(this.requiredComplianceModel!=null){
      this.dataSourceRequired = new MatTableDataSource(this.requiredComplianceModel);
    }
    else{
      this.dataSourceRequired = new MatTableDataSource(this.requiredCompliancedata);
    }
     
    }
    });
    }
    ShowAccidentDetails(){
      const data = {
        Id : this.clientId,
        ShiftId:this.ShiftId
      };
      this.clientservice.GetAllIncidentDetails(data).subscribe(res => {
      this.response = res;
      if (this.response.status > 0) {
      this.ReportedBy=(this.response.responseData.clientAccidentProviderInfo.reportCompletedBy);
      this.ProviderName=(this.response.responseData.clientAccidentProviderInfo.providerName);
      this.RegistrationId=(this.response.responseData.clientAccidentProviderInfo.providerregistrationId);
      this.ProviderABN=(this.response.responseData.clientAccidentProviderInfo.providerABN);
      this.OutletName=(this.response.responseData.clientAccidentProviderInfo.outletName);
      this.RegistrationGroup=(this.response.responseData.clientAccidentProviderInfo.registrationgroup);
      this.State=(this.response.responseData.clientAccidentProviderInfo.stateName);
      this.ContactTitle=(this.response.responseData.clientAccidentPrimaryContact.title);
      this.ContactFirstName=(this.response.responseData.clientAccidentPrimaryContact.fullName);
      this.ContactMiddleName=(this.response.responseData.clientAccidentPrimaryContact.middleName);
      this.ContactLastName=(this.response.responseData.clientAccidentPrimaryContact.lastName);
      this.ContactProvider=(this.response.responseData.clientAccidentPrimaryContact.providerPosition);
      this.ContactPhoneNo=(this.response.responseData.clientAccidentPrimaryContact.phoneNo);
      this.ContactEmail=(this.response.responseData.clientAccidentPrimaryContact.email);
      this.ContactMethod=(this.response.responseData.clientAccidentPrimaryContact.contactMetod);
      this.IsIncidentAnticipatedInfo=this.response.responseData.clientIncidentCategory.isIncidentAnticipated==1?"Yes":this.response.responseData.clientIncidentCategory.isIncidentAnticipated==2?"No":this.response.responseData.clientIncidentCategory.isIncidentAnticipated==3?"Unknown":"";
      this.Primarylist=this.response.responseData.clientPrimaryIncidentCategory;
      this.PrimaryCategoryName=this.Primarylist.map(x=>x.primaryIncidentName);
      this.Secondarylist=this.response.responseData.clientSecondaryIncidentCategory;
      this.SecondaryCategoryName=(this.Secondarylist.map(x=>x.secondaryIncidentName));
      this.locationtypeinfo=(this.response.responseData.clientIncidentDetails.locationTypeName);
      this.Incidentdateinfo=(this.response.responseData.clientIncidentDetails.dateTime);
      this.Reasoninfo=(this.response.responseData.clientIncidentDetails.unknowndateReason);
      this.NdisProviderTimeinfo=(this.response.responseData.clientIncidentDetails.startTimeString);
      this.NdisProviderDateinfo=(this.response.responseData.clientIncidentDetails.ndisProviderDate);
      this.DescribeIncidentinfo=(this.response.responseData.clientIncidentDetails.incidentAllegtion);
      this.circumstanceIncidentinfo=(this.response.responseData.clientIncidentDetails.allegtionCircumstances);
      if(this.response.responseData.clientIncidentDetails.address!="" && this.response.responseData.clientIncidentDetails.address!=null && this.response.responseData.clientIncidentDetails.address!=undefined){
        this.otherlocationInfo=(this.response.responseData.clientIncidentDetails.address);
        
      }
       else if (this.response.responseData.clientIncidentDetails.locationId!=null&&this.response.responseData.clientIncidentDetails.locationId>0){
        
        this.locationName=(this.response.responseData.clientIncidentDetails.locationName);
       
      }
      this.Primarydislist=this.response.responseData.incidentPrimaryDisability;
      this.ImpactedPrimaryDisbilityinfo=(this.Primarydislist.map(x=>x.primaryDisabilityName));
      this.otherdislist=this.response.responseData.incidentOtherDisability;
      this.ImpactedSecondaryDisabilityinfo=(this.otherdislist.map(x=>x.secondaryDisabilityName));
      this.ImpactCommunication=this.response.responseData.clientIncidentCommunication;
      this.ImpactedCommunicationinfo=(this.ImpactCommunication.map(x=>x.communicationName));
      this.ImpactConcerndata=this.response.responseData.incidentConcernBehaviour;
      this.ImpactedBehaviourinfo=(this.ImpactConcerndata.map(x=>x.concernBehaviourName));
      this.ImpactedTitleinfo=(this.response.responseData.incidentImpactedPerson.title);
      this.ImpactedName=(this.response.responseData.incidentImpactedPerson.fullName);
      this.ImpactedNdisNoinfo=(this.response.responseData.incidentImpactedPerson.ndisParticipantNo);
      this.ImpactedGenderinfo=(this.response.responseData.incidentImpactedPerson.genderName);
      this.ImpactedDOBinfo=(this.response.responseData.incidentImpactedPerson.dateOfBirth);
      this.ImpactedPhoneNo=(this.response.responseData.incidentImpactedPerson.phoneNo);
      this.ImpactedEmail=(this.response.responseData.incidentImpactedPerson.email);
      this.OtherDetail=(this.response.responseData.incidentImpactedPerson.otherDetail);
      this.PrimaryAllegation=this.response.responseData.incidentAllegationPrimaryDisability;
      this.DisablePrimaryDisability=(this.PrimaryAllegation.map(x=>x.primaryDisabilityName));
      this.otherAllegation=this.response.responseData.incidentAllegationOtherDisability;
      this.DisableOtherDisability=(this.otherAllegation.map(x=>x.secondaryDisabilityName));
      this.AllegationCommunication=this.response.responseData.incidentAllegationCommunication;
      this.DisableCommunication=(this.AllegationCommunication.map(x=>x.communicationName));
      this.AllegationConcerndata=this.response.responseData.incidentAllegationBehaviour;
      this.DisableBehaviour=(this.AllegationConcerndata.map(x=>x.concernBehaviourName));
      this.WorkerTitle=(this.response.responseData.incidentWorkerAllegation.title);
      this.WorkerName=(this.response.responseData.incidentWorkerAllegation.subjectFullName);
      this.WorkerPosition=(this.response.responseData.incidentWorkerAllegation.position);
      this.WorkerGender=(this.response.responseData.incidentWorkerAllegation.genderName);
      this.WorkerDOB=(this.response.responseData.incidentWorkerAllegation.dateOfBirth);
      this.WorkerPhoneNo=(this.response.responseData.incidentWorkerAllegation.phoneNo);
      this.WorkerEmail=(this.response.responseData.incidentWorkerAllegation.email);
      this.AllegationOtherDetail=(this.response.responseData.incidentDisablePersonAllegation.otherDetail);
      this.IsSubjectAllegationInfo=this.response.responseData.incidentWorkerAllegation.isSubjectAllegation==1?"Yes":this.response.responseData.incidentWorkerAllegation.isSubjectAllegation==2?"No":this.response.responseData.incidentWorkerAllegation.isSubjectAllegation==3?"Unknown":"";
      this.DisableTitle=(this.response.responseData.incidentDisablePersonAllegation.title);
      this.DisableName=(this.response.responseData.incidentDisablePersonAllegation.disableFullName);
      this.DisableNdisNo=(this.response.responseData.incidentDisablePersonAllegation.ndisNumber);
      this.DisableGender=(this.response.responseData.incidentDisablePersonAllegation.genderName);
      this.DisableDOBirth=(this.response.responseData.incidentDisablePersonAllegation.dateOfBirth);
      this.DisablePhoneNo=(this.response.responseData.incidentDisablePersonAllegation.phoneNo);
      this.DisableEmail=(this.response.responseData.incidentDisablePersonAllegation.email);
      this.OtherName=(this.response.responseData.incidentOtherAllegation.otherFullName);
      this.OtherRelationShip=(this.response.responseData.incidentOtherAllegation.relationship);
      this.OtherGender=(this.response.responseData.incidentOtherAllegation.genderName);
      this.OtherDOB=(this.response.responseData.incidentOtherAllegation.dateOfBirth);
      this.OtherPhoneNo=(this.response.responseData.incidentOtherAllegation.phoneNo);
      this.OtherEmail=(this.response.responseData.incidentOtherAllegation.email);
      this.OtherTitle=(this.response.responseData.incidentOtherAllegation.title);
      this.IsPoliceInformed=(this.response.responseData.incidentImmediateAction.isPoliceInformed==true?"Yes":"No");
      this.OfficerName=(this.response.responseData.incidentImmediateAction.officerName);
      this.PoliceStation=(this.response.responseData.incidentImmediateAction.policeStation);
      this.PoliceNumber=(this.response.responseData.incidentImmediateAction.policeNo);
      this.PoliceNotInform=(this.response.responseData.incidentImmediateAction.providerPosition);
      this.IsFamilyAwareInfo=this.response.responseData.incidentImmediateAction.isFamilyAware==1?"Yes":this.response.responseData.incidentImmediateAction.isFamilyAware==2?"No":this.response.responseData.incidentImmediateAction.isFamilyAware==3?"Unsure":"";
      this.IsUnder18Info=this.response.responseData.incidentImmediateAction.isUnder18==1?"Yes":this.response.responseData.incidentImmediateAction.isUnder18==2?"No":this.response.responseData.incidentImmediateAction.isUnder18==3?"Unkown":this.response.responseData.incidentImmediateAction.isUnder18==4?"NotApplicable":"";
      this.Guardian=(this.response.responseData.incidentImmediateAction.contacttoFamily);
      this.ChildContacted=(this.response.responseData.incidentImmediateAction.contactChildProtection);
      this.DescribeImmediate=(this.response.responseData.incidentImmediateAction.disabilityPerson);
      this.WorkerDescribe=(this.response.responseData.incidentImmediateAction.subjectWorkerAllegation);
      this.DescribeDisability=(this.response.responseData.incidentImmediateAction.subjectDisabilityPerson);
      this.IsRiskAssesmentInfo=this.response.responseData.incidentRiskAssesment.isRiskAssesment==1?"Yes":this.response.responseData.incidentRiskAssesment.isRiskAssesment==2?"No":this.response.responseData.incidentRiskAssesment.isRiskAssesment==3?"InProgress":"";
      this.RiskAssesmentDate=(this.response.responseData.incidentRiskAssesment.riskAssesmentDate);
      this.RiskDetails=(this.response.responseData.incidentRiskAssesment.riskDetails);
      this.NoRiskAssesmentInfo=(this.response.responseData.incidentRiskAssesment.noRiskAssesmentInfo);
      this.InProgressRisk=(this.response.responseData.incidentRiskAssesment.inProgressRisk);
      this.TobeFinished=(this.response.responseData.incidentRiskAssesment.tobeFinished);
      this.DeclarationName=(this.response.responseData.incidentDeclaration.name);
      this.DeclarationPosition=(this.response.responseData.incidentDeclaration.positionAtOrganisation);
      this.DeclarationDate=(this.response.responseData.incidentDeclaration.date);
     
        }
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
    AddProviderInfo() {  
      if (this.rFormProvider.valid) {
        const data = {
          'ClientId': this.clientId,
          'ShiftId': this.ShiftId,
          'EmployeeId': this.EmpId,
          'ReportCompletedBy': this.rFormProvider.value.ReportedBy,
          'ProviderName': this.rFormProvider.value.ProviderName,
          'ProviderregistrationId': this.rFormProvider.value.RegistrationId,
          'ProviderABN': this.rFormProvider.value.ProviderABN,
          'OutletName': this.rFormProvider.value.OutletName,
          'Registrationgroup': this.rFormProvider.value.RegistrationGroup,
          'State': this.rFormProvider.value.State,
          
        };
    
        this.clientservice.AddIncidentProviderdetail(data).subscribe(res => {
          this.response = res;
          switch (this.response.status) {
            case 1:
            this.notification.Success({ message: this.response.message, title: null });
            this.rFormProvider.controls['ReportedBy'].patchValue(this.response.responseData.reportCompletedBy);
            this.rFormProvider.controls['ProviderName'].patchValue(this.response.responseData.providerName);
            this.rFormProvider.controls['RegistrationId'].patchValue(this.response.responseData.providerregistrationId);
            this.rFormProvider.controls['ProviderABN'].patchValue(this.response.responseData.providerABN);
            this.rFormProvider.controls['OutletName'].patchValue(this.response.responseData.outletName);
            this.rFormProvider.controls['RegistrationGroup'].patchValue(this.response.responseData.registrationgroup);
            this.rFormProvider.controls['State'].patchValue(this.response.responseData.state);
            this.textProvider="Update"
              break;
    
            default:
              this.notification.Warning({ message: this.response.message, title: null });
              break;
          }
        });
      }
    }
    AddcontactInfo() {  
      if (this.rFormcontact.valid) {
        const data = {
          'ClientId': this.clientId,
          'ShiftId': this.ShiftId,
          'EmployeeId': this.EmpId,
          'Title': this.rFormcontact.value.ContactTitle,
          'FirstName': this.rFormcontact.value.ContactFirstName,
          'MiddleName': this.rFormcontact.value.ContactMiddleName,
          'LastName': this.rFormcontact.value.ContactLastName,
          'ProviderPosition': this.rFormcontact.value.ContactProvider,
          'PhoneNo': this.rFormcontact.value.ContactPhoneNo,
          'Email': this.rFormcontact.value.ContactEmail,
          'ContactMetod': this.rFormcontact.value.ContactMethod,
        };
         this.clientservice.AddIncidentContactInfo(data).subscribe(res => {
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
    AddIncidentCategory() {  
      if (this.rFormCategory.valid) {
       
        this.empAIModel.primaryIncidentId = (this.rFormCategory.value.PrimaryCategory);
        this.empAIModel.secondaryIncidentId = (this.rFormCategory.value.SecondaryCategory);
        const data = {
          'ClientId': this.clientId,
          'ShiftId': this.ShiftId,
          'EmployeeId': this.EmpId,
          'PrimaryIncidentId': this.empAIModel.primaryIncidentId,
          'SecondaryIncidentId': this.empAIModel.secondaryIncidentId,
          'IsIncidentAnticipated': this.rFormCategory.value.IsIncidentAnticipated=="1"?this.rFormCategory.value.IsIncidentAnticipated=1:this.rFormCategory.value.IsIncidentAnticipated=="2"?this.rFormCategory.value.IsIncidentAnticipated=2:this.rFormCategory.value.IsIncidentAnticipated=="3"?this.rFormCategory.value.IsIncidentAnticipated=3:this.rFormCategory.value.IsIncidentAnticipated=0,
        };
         this.clientservice.AddIncidentCategory(data).subscribe(res => {
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
    AddIncidentDetails() {  
      if (this.rFormIncidentInfo.valid) {
        const data = {
          'ClientId': this.clientId,
          'ShiftId': this.ShiftId,
          'EmployeeId': this.EmpId,
          'LocationId':Number(this.rFormIncidentInfo.value.locationId),
          'LocationType': this.rFormIncidentInfo.value.locationtype,
          'Address': this.rFormIncidentInfo.value.otherlocation,
          'OtherLocation': this.rFormIncidentInfo.value.Location,
          'DateTime': moment(this.rFormIncidentInfo.value.Incidentdate).format('YYYY-MM-DD'),
          'UnknowndateReason': this.rFormIncidentInfo.value.Reason,
          'NdisProviderTime': (this.rFormIncidentInfo.value.NdisProviderTime),
          'NdisProviderDate':  moment(this.rFormIncidentInfo.value.NdisProviderDate).format('YYYY-MM-DD'),
          'AllegtionCircumstances':this.rFormIncidentInfo.value.circumstanceIncident,
          'IncidentAllegtion':this.rFormIncidentInfo.value.DescribeIncident,
        };
       
        this.clientservice.AddIncidentDetails(data).subscribe(res => {
          this.response = res;
          switch (this.response.status) {
            case 1:
             this.notification.Success({ message: this.response.message, title: null });
             this.getAccidentIncidentInfo();
              break;
    
            default:
              this.notification.Warning({ message: this.response.message, title: null });
              break;
          }
        });
      }
    }
    AddImpactedPerson() {  
      if (this.rFormImpacted.valid) {
        this.ImpactedModel.PrimaryDisability = (this.rFormImpacted.value.ImpactedPrimaryDisbility);
        this.ImpactedModel.OtherDisability = (this.rFormImpacted.value.ImpactedSecondaryDisbility);
        this.ImpactedModel.CommunicationId = (this.rFormImpacted.value.ImpactedCommunication);
        this.ImpactedModel.ConcerBehaviourId = (this.rFormImpacted.value.ImpactedBehaviour);
        const data = {
          'ClientId': this.clientId,
          'ShiftId': this.ShiftId,
          'EmployeeId': this.EmpId,
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
         this.clientservice.AddIncidentImpactedPerson(data).subscribe(res => {
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

    AddSubjectAllegation() {  
      if (this.rFormAllegation.valid) {
        this.AllegationModel.PrimaryDisability = (this.rFormAllegation.value.DisablePrimaryDisability);
        this.AllegationModel.OtherDisability = (this.rFormAllegation.value.DisableOtherDisability!=""?this.rFormAllegation.value.DisableOtherDisability:[]);
        this.AllegationModel.CommunicationId = (this.rFormAllegation.value.DisableCommunication);
        this.AllegationModel.ConcerBehaviourId = (this.rFormAllegation.value.DisableBehaviour);
       
        const data = {
          'ClientId': this.clientId,
          'ShiftId': this.ShiftId,
          'EmployeeId': this.EmpId,
          'PrimaryDisability': this.AllegationModel.PrimaryDisability,
          'OtherDisability': this.AllegationModel.OtherDisability,
          'CommunicationId': this.AllegationModel.CommunicationId,
          'ConcerBehaviourId': this.AllegationModel.ConcerBehaviourId,
          'Title': this.rFormAllegation.value.WorkerTitle,
          'FirstName': this.rFormAllegation.value.WorkerFirstName,
          'LastName': this.rFormAllegation.value.WorkerLastName,
          'Position': this.rFormAllegation.value.WorkerPosition,
          'PhoneNo': this.rFormAllegation.value.WorkerPhoneNo,
          'Email': this.rFormAllegation.value.WorkerEmail,
          'GenderId': this.rFormAllegation.value.WorkerGender,
          'OtherDetail': this.rFormAllegation.value.AllegationOtherDetail,
          'DateOfBirth': moment(this.rFormAllegation.value.WorkerDOB).format('YYYY-MM-DD'),
          'IsSubjectAllegation': this.rFormAllegation.value.IsSubjectAllegation=="1"?this.rFormAllegation.value.IsSubjectAllegation=1:this.rFormAllegation.value.IsSubjectAllegation=="2"?this.rFormAllegation.value.IsSubjectAllegation=2:this.rFormAllegation.value.IsSubjectAllegation=="3"?this.rFormAllegation.value.IsSubjectAllegation=3:this.rFormAllegation.value.IsSubjectAllegation=0,
          'DisableTitle': this.rFormAllegation.value.DisableTitle,
          'DisableFirstName': this.rFormAllegation.value.DisableFirstName,
          'DisableLastName': this.rFormAllegation.value.WorkerLastName,
          'DisableNdisNumber': this.rFormAllegation.value.DisableNdisNo,
          'DisablePhoneNo': this.rFormAllegation.value.DisablePhoneNo,
          'DisableEmail': this.rFormAllegation.value.DisableEmail,
          'DisableGender': this.rFormAllegation.value.DisableGender,
          'DisableDateOfBirth': moment(this.rFormAllegation.value.DisableDOBirth).format('YYYY-MM-DD'),
          'OtherTitle': this.rFormAllegation.value.OtherTitle,
          'OtherFirstName': this.rFormAllegation.value.OtherFirstName,
          'OtherLastName': this.rFormAllegation.value.OtherLastName,
          'OtherRelationship': this.rFormAllegation.value.OtherRelationShip,
          'OtherPhoneNo': this.rFormAllegation.value.OtherPhoneNo,
          'OtherEmail': this.rFormAllegation.value.OtherEmail,
          'OtherGender': Number(this.rFormAllegation.value.OtherGender),
          'OtherDateOfBirth':moment(this.rFormAllegation.value.OtherDOB).format('YYYY-MM-DD')=="Invalid date"?this.todayDatemax:moment(this.rFormAllegation.value.OtherDOB).format('YYYY-MM-DD'),
        };
         this.clientservice.AddIncidentSubjectAllegation(data).subscribe(res => {
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
    AddActionTaken() {  
      if (this.rFormAction.valid) {
          const data = {
          'ClientId': this.clientId,
          'ShiftId': this.ShiftId,
          'EmployeeId': this.EmpId,
          'IsPoliceInformed': this.rFormAction.value.IsPoliceInformed== "1" ? true :false,
          'OfficerName': this.rFormAction.value.OfficerName,
          'PoliceStation': this.rFormAction.value.PoliceStation,
          'PoliceNo': this.rFormAction.value.PoliceNumber,
          'ProviderPosition': this.rFormAction.value.PoliceNotInform,
          'ContacttoFamily': this.rFormAction.value.Guardian,
          'IsFamilyAware': this.rFormAction.value.IsFamilyAware=="1"?this.rFormAction.value.IsFamilyAware=1:this.rFormAction.value.IsFamilyAware=="2"?this.rFormAction.value.IsFamilyAware=2:this.rFormAction.value.IsFamilyAware=="3"?this.rFormAction.value.IsFamilyAware=3:this.rFormAction.value.IsFamilyAware=0,
          'IsUnder18': this.rFormAction.value.IsUnder18=="1"?this.rFormAction.value.IsUnder18=1:this.rFormAction.value.IsUnder18=="2"?this.rFormAction.value.IsUnder18=2:this.rFormAction.value.IsUnder18=="3"?this.rFormAction.value.IsUnder18=3:this.rFormAction.value.IsUnder18=="4"?this.rFormAction.value.IsUnder18=4:this.rFormAction.value.IsUnder18=0,
          'ContactChildProtection': this.rFormAction.value.ChildContacted,
          'DisabilityPerson': this.rFormAction.value.DescribeImmediate,
          'SubjectWorkerAllegation': this.rFormAction.value.WorkerDescribe,
          'SubjectDisabilityPerson': this.rFormAction.value.DescribeDisability,
          
        };
         this.clientservice.AddIncidentImmediateAction(data).subscribe(res => {
          this.response = res;
          switch (this.response.status) {
            case 1:
            this.notification.Success({ message: this.response.message, title: null });
            this.rFormAction.controls['IsPoliceInformed'].patchValue(this.response.responseData.isPoliceInformed==true?"1":"2");
            this.rFormAction.controls['OfficerName'].patchValue(this.response.responseData.officerName);
            this.rFormAction.controls['PoliceStation'].patchValue(this.response.responseData.policeStation);
            this.rFormAction.controls['PoliceNumber'].patchValue(this.response.responseData.policeNo);
            this.rFormAction.controls['PoliceNotInform'].patchValue(this.response.responseData.providerPosition);
            this.rFormAction.controls['IsFamilyAware'].patchValue(this.response.responseData.incidentImmediateAction.isFamilyAware==1?"1":this.response.responseData.incidentImmediateAction.isFamilyAware==2?"2":this.response.responseData.incidentImmediateAction.isFamilyAware==3?"3":"")
            this.rFormAction.controls['IsUnder18'].patchValue(this.response.responseData.incidentImmediateAction.isUnder18==1?"1":this.response.responseData.incidentImmediateAction.isUnder18==2?"2":this.response.responseData.incidentImmediateAction.isUnder18==3?"3":this.response.responseData.incidentImmediateAction.isUnder18==4?"4":"")
            this.rFormAction.controls['Guardian'].patchValue(this.response.responseData.contacttoFamily);
            this.rFormAction.controls['ChildContacted'].patchValue(this.response.responseData.contactChildProtection);
            this.rFormAction.controls['DescribeImmediate'].patchValue(this.response.responseData.disabilityPerson);
            this.rFormAction.controls['WorkerDescribe'].patchValue(this.response.responseData.subjectWorkerAllegation);
            this.rFormAction.controls['DescribeDisability'].patchValue(this.response.responseData.subjectDisabilityPerson);
            this.textImmediate="Update";
            break;
    
            default:
              this.notification.Warning({ message: this.response.message, title: null });
              break;
          }
        });
      }
    }
    AddRiskAssesment() {  
      if (this.rFormRisk.valid) {
           const data = {
          'ClientId': this.clientId,
          'ShiftId': this.ShiftId,
          'EmployeeId': this.EmpId,
          'IsRiskAssesment': this.rFormRisk.value.IsRiskAssesment=="1"?this.rFormRisk.value.IsRiskAssesment=1:this.rFormRisk.value.IsRiskAssesment=="2"?this.rFormRisk.value.IsRiskAssesment=2:this.rFormRisk.value.IsRiskAssesment=="3"?this.rFormRisk.value.IsRiskAssesment=3:this.rFormRisk.value.IsRiskAssesment=0,
          'RiskAssesmentDate': moment(this.rFormRisk.value.RiskAssesmentDate).format('YYYY-MM-DD'),
          'RiskDetails': this.rFormRisk.value.RiskDetails,
          'NoRiskAssesmentInfo': this.rFormRisk.value.NoRiskAssesmentInfo,
          'InProgressRisk': this.rFormRisk.value.InProgressRisk,
          'TobeFinished': this.rFormRisk.value.TobeFinished,
         };
           this.clientservice.AddIncidentRiskAssesment(data).subscribe(res => {
           this.response = res;
           switch (this.response.status) {
           case 1:
           this.notification.Success({ message: this.response.message, title: null });
           this.rFormRisk.controls['RiskAssesmentDate'].patchValue(this.response.responseData.riskAssesmentDate);
           this.rFormRisk.controls['RiskDetails'].patchValue(this.response.responseData.riskDetails);
           this.rFormRisk.controls['NoRiskAssesmentInfo'].patchValue(this.response.responseData.noRiskAssesmentInfo);
           this.rFormRisk.controls['InProgressRisk'].patchValue(this.response.responseData.inProgressRisk);
           this.rFormRisk.controls['TobeFinished'].patchValue(this.response.responseData.tobeFinished);
           this.rFormRisk.controls['IsRiskAssesment'].patchValue(this.response.responseData.incidentRiskAssesment.isRiskAssesment==1?"1":this.response.responseData.incidentRiskAssesment.isRiskAssesment==2?"2":this.response.responseData.incidentRiskAssesment.isRiskAssesment==3?"3":"")
           this.textRisk="Update";
           break;
           default:
           this.notification.Warning({ message: this.response.message, title: null });
           break;
          }
        });
      }
    }
    getRiskAssesment() {
      const data = {
       Id : this.clientId,
       ShiftId : this.ShiftId
    };
     this.clientservice.getRiskAssesment(data).subscribe(res => {
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
    AddDeclaration() {  
      if (this.rFormDeclaration.valid) {
        if(this.rFormDeclaration.value.IsDeclaration!="1"){
          this.notification.Warning({ message: "Please Select Confirmation box", title: null });
        }
        else{
        const data = {
          'ClientId': this.clientId,
          'ShiftId': this.clientId,
          'EmployeeId': this.EmpId,
          'Name': this.rFormDeclaration.value.DeclarationName,
          'PositionAtOrganisation': this.rFormDeclaration.value.DeclarationPosition,
          'Date': moment(this.rFormDeclaration.value.DeclarationDate).format('YYYY-MM-DD'),
          'IsDeclaration': this.rFormDeclaration.value.IsDeclaration== "1" ? true:false,
        };
         this.clientservice.AddIncidentDeclaration(data).subscribe(res => {
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
    uploadDocument(event: any) {
      let fileExtension = null;
      let extension = null;
      if (event.target.files !== undefined) {
        fileExtension = event.target.files[0].name.split('.');
        extension = fileExtension[fileExtension.length - 1].toLowerCase();
        this.ImageName =  event.target.files[0].name;
        this.ImageSize =  event.target.files[0].length;
       
      }
    }
    BrowseImageDelete(){
      this.ImageName =  '';
      this.ImageSize =  '';
      this.fileInput.nativeElement.value = '';
    }
    AddAttachment() {
      if (this.rFormattach.valid) {
      const fileInput = this.fileInput.nativeElement;
      let fileLength = fileInput.files.length;
      let file = fileInput.files[0];
      const formData = new FormData(); 
      if(fileLength > 0)
      {
        var type =  file.type;
        var name =  file.name;
        formData.append('Files', fileInput.files[0]);
      }
      formData.set('ClientId', this.clientId.toString());
      formData.set('ShiftId', this.ShiftId.toString());
      formData.set('EmployeeId', this.EmpId.toString());
      formData.append('DocumentName', this.rFormattach.get('AddDocument').value);
      this.clientservice.AddIncidentDocuments(formData).subscribe(res => {
      this.responseModel = res;
      switch (this.responseModel.status) {
      case 1:  
      this.notification.Success({ message: this.responseModel.message, title: '' });          
      this.rFormattach.reset();
      this.formDirective.resetForm();
      this.ImageName="";
      this.ImageSize="";
      this.GetAttachDocument();
      break;
      default:
      this.notification.Warning({ message: this.responseModel.message, title: '' });
      break;
    }
    });
    }
    }
    UpdateAttachment() {
      if (this.rFormattach.valid) {
        const formData = new FormData(); 
        if(this.ImageName==undefined||this.ImageName==""){

        }
        else{
          const fileInput = this.fileInput.nativeElement;
          let fileLength = fileInput.files.length;
          let file = fileInput.files[0];
          
          if(fileLength > 0)
          {
            var type =  file.type;
            var name =  file.name;
               
              formData.append('Files', fileInput.files[0]);
          }
        }
     
      formData.set('Id', this.EditdocId.toString());
      formData.append('DocumentName', this.rFormattach.get('AddDocument').value);
        this.clientservice.UpdateIncidentDocuments(formData).subscribe(res => {
          this.responseModel = res;
          switch (this.responseModel.status) {
            case 1:  
            this.notification.Success({ message: this.responseModel.message, title: '' });          
            this.rFormattach.reset();
            this.formDirective.resetForm();
            this.ImageName="";
            this.ImageSize="";
            this.fileInput.nativeElement.value = ''
            this.txtAttachButton="Submit";
            this.isShownBrowse=true;
            this.isShownUrl=false;
            this.GetAttachDocument();
            break;
            default:
            this.notification.Warning({ message: this.responseModel.message, title: '' });
            break;
          }
       });
     }
    }
    SaveAttachment(){
      if(this.txtAttachButton=="Submit"){
        this.AddAttachment();
      }
      else{
        this.UpdateAttachment();
      }
    }
    FileSaver = require('file-saver');
    downloadPdf(docUrl) {
    this.FileSaver.saveAs(docUrl);
   }
    EditCompliance(data: any) {    
       this.EditdocId=data.id;
       this.txtAttachButton="Update";
       this.rFormattach.controls['AddDocument'].patchValue(data.documentName);
       if(data.fileName!="" && data.fileName!=null){
       this.Imageurl = (this.baseUrl + data.fileName);
       this.isShownUrl=true;
       this.isShownBrowse=false;
  }
   else{
       this.isShownUrl=false;
       this.isShownBrowse=true;
   }
     }
   showReport(){
    this.isShownReport=true;
   }
  ImageDelete(){
    this.AccidentInfoModel.Id = this.EditdocId;
    this.clientservice.DeleteIncidentDocument(this.AccidentInfoModel).subscribe((data: any) => {
      if (data.status == 1) {
        this.isShownUrl=false;
        this.isShownBrowse=true;
        this.GetAttachDocument();
      }
      else {
        this.notification.Error({ message: "Some error occured", title: null });
      }

    })
 
  }
  GetIncidentCategory() {
  const data = {
   Id : this.clientId,
   ShiftId: this.ShiftId,
  
 };
 this.clientservice.GetClientIncidentCategory(data).subscribe(res => {
  this.response = res;
  if (this.response.status > 0) {
  this.totalCount = this.response.total;
  this.rFormCategory.controls['IsIncidentAnticipated'].patchValue(this.response.responseData.clientIncidentCategory.isIncidentAnticipated==1?"1":this.response.responseData.clientIncidentCategory.isIncidentAnticipated==2?"2":this.response.responseData.clientIncidentCategory.isIncidentAnticipated==3?"3":"")
  this.Primarylist=this.response.responseData.clientPrimaryIncidentCategory;
  this.rFormCategory.controls['PrimaryCategory'].patchValue(this.Primarylist.map(x=>x.primaryIncidentId));
  this.Secondarylist=this.response.responseData.clientSecondaryIncidentCategory;
  this.rFormCategory.controls['SecondaryCategory'].patchValue(this.Secondarylist.map(x=>x.secondaryIncidentId));
 
}
   else{

   }
});
  }
  GetImpactedPersonInfo() {
  const data = {
   Id : this.clientId,
   ShiftId: this.ShiftId,
 };
 this.clientservice.GetIncidentImpactedPerson(data).subscribe(res => {
  this.response = res;
   if (this.response.status > 0) {
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
   
 
}
  
});
  }
  GetIncidentSubjectAllegation() {
  const data = {
   Id : this.clientId,
   ShiftId: this.ShiftId,
 };
  this.clientservice.GetIncidentSubjectAllegation(data).subscribe(res => {
  this.response = res;
  if (this.response.status > 0) {
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
  this.rFormAllegation.controls['IsSubjectAllegation'].patchValue(this.response.responseData.incidentWorkerAllegation.isSubjectAllegation==1?"1":this.response.responseData.incidentWorkerAllegation.isSubjectAllegation==2?"2":this.response.responseData.incidentWorkerAllegation.isSubjectAllegation==3?"3":"")
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
 
}
   else{

   }
});
  }
  GetAttachDocument() {
  const data = {
   Id : this.clientId,
   ShiftId: this.ShiftId,
 };
 this.clientservice.GetIncidentDocumentDetail(data).subscribe(res => {
  this.response = res;
  if (this.response.status > 0) {
  this.totalCount = this.response.total;
  this.requiredComplianceModel = this.response.responseData.incidentDocumentDetailModel;
      if(this.requiredComplianceModel!=null){
        this.dataSourceRequired = new MatTableDataSource(this.requiredComplianceModel);
      }
      else{
        this.dataSourceRequired = new MatTableDataSource(this.requiredCompliancedata);
      }
 }
   else{

   }
});
  }
  getAccidentIncidentInfo() {
    const data = {
     Id : this.clientId,
     ShiftId: this.ShiftId,
     };
   this.clientservice.getAccidentIncidentInfo(data).subscribe(res => {
    this.response = res;
   if (this.response.status > 0) {
   if(this.response.responseData.clientIncidentDetails.clientId>0){
    this.rFormIncidentInfo.controls['locationtype'].patchValue(this.response.responseData.clientIncidentDetails.locationType);
    this.rFormIncidentInfo.controls['Incidentdate'].patchValue(this.response.responseData.clientIncidentDetails.dateTime);
    this.rFormIncidentInfo.controls['Reason'].patchValue(this.response.responseData.clientIncidentDetails.unknowndateReason);
    this.rFormIncidentInfo.controls['NdisProviderTime'].patchValue(this.response.responseData.clientIncidentDetails.startTimeString=="12:00 AM"?"24:00":this.response.responseData.clientIncidentDetails.startTimeString=="12:30 AM"?"00:30":this.response.responseData.clientIncidentDetails.startTimeString=="01:00 AM"?"01:00":
    this.response.responseData.clientIncidentDetails.startTimeString=="01:30 AM"?"01:30":
    this.response.responseData.clientIncidentDetails.startTimeString=="02:00 AM"?"02:00":this.response.responseData.clientIncidentDetails.startTimeString=="02:30 AM"?"02:30":this.response.responseData.clientIncidentDetails.startTimeString=="03:00 AM"?"03:00":this.response.responseData.clientIncidentDetails.startTimeString=="03:30 AM"?"03:30":
    this.response.responseData.clientIncidentDetails.startTimeString=="04:00 AM"?"04:00":this.response.responseData.clientIncidentDetails.startTimeString=="04:30 AM"?"04:30":this.response.responseData.clientIncidentDetails.startTimeString=="05:00 AM"?"05:00":this.response.responseData.clientIncidentDetails.startTimeString=="05:30 AM"?"05:30":
    this.response.responseData.clientIncidentDetails.startTimeString=="06:00 AM"?"06:00":this.response.responseData.clientIncidentDetails.startTimeString=="06:30 AM"?"06:30":this.response.responseData.clientIncidentDetails.startTimeString=="07:00 AM"?"07:00":this.response.responseData.clientIncidentDetails.startTimeString=="07:30 AM"?"07:30":
    this.response.responseData.clientIncidentDetails.startTimeString=="08:00 AM"?"08:00":this.response.responseData.clientIncidentDetails.startTimeString=="08:30 AM"?"08:30":this.response.responseData.clientIncidentDetails.startTimeString=="09:00 AM"?"09:00":this.response.responseData.clientIncidentDetails.startTimeString=="09:30 AM"?"09:30":
    this.response.responseData.clientIncidentDetails.startTimeString=="10:00 AM"?"10:00":this.response.responseData.clientIncidentDetails.startTimeString=="10:00 AM"?"10:30":this.response.responseData.clientIncidentDetails.startTimeString=="11:00 AM"?"11:00":this.response.responseData.clientIncidentDetails.startTimeString=="11:30 AM"?"11:30":
    this.response.responseData.clientIncidentDetails.startTimeString=="12:00 PM"?"12:00":this.response.responseData.clientIncidentDetails.startTimeString=="12:30 PM"?"12:30":this.response.responseData.clientIncidentDetails.startTimeString=="01:00 PM"?"13:00":
    this.response.responseData.clientIncidentDetails.startTimeString=="02:00 PM"?"14:00":this.response.responseData.clientIncidentDetails.startTimeString=="02:30 PM"?"14:30":this.response.responseData.clientIncidentDetails.startTimeString=="03:00 PM"?"15:00":this.response.responseData.clientIncidentDetails.startTimeString=="03:30 PM"?"15:30":
    this.response.responseData.clientIncidentDetails.startTimeString=="04:00 PM"?"16:00":this.response.responseData.clientIncidentDetails.startTimeString=="04:30 PM"?"16:30":this.response.responseData.clientIncidentDetails.startTimeString=="05:00 PM"?"17:00":this.response.responseData.clientIncidentDetails.startTimeString=="05:30 PM"?"17:30":
    this.response.responseData.clientIncidentDetails.startTimeString=="06:00 PM"?"18:00":this.response.responseData.clientIncidentDetails.startTimeString=="06:30 PM"?"18:30":this.response.responseData.clientIncidentDetails.startTimeString=="07:00 PM"?"19:00":this.response.responseData.clientIncidentDetails.startTimeString=="07:30 PM"?"19:30":
    this.response.responseData.clientIncidentDetails.startTimeString=="08:00 PM"?"20:00":this.response.responseData.clientIncidentDetails.startTimeString=="08:30 PM"?"20:30":this.response.responseData.clientIncidentDetails.startTimeString=="09:00 PM"?"21:00":this.response.responseData.clientIncidentDetails.startTimeString=="09:30 PM"?"21:30":
    this.response.responseData.clientIncidentDetails.startTimeString=="10:00 PM"?"22:00":this.response.responseData.clientIncidentDetails.startTimeString=="10:00 PM"?"22:30":this.response.responseData.clientIncidentDetails.startTimeString=="11:00 PM"?"23:00":this.response.responseData.clientIncidentDetails.startTimeString=="11:30 PM"?"23:30":"");
    this.rFormIncidentInfo.controls['NdisProviderDate'].patchValue(this.response.responseData.clientIncidentDetails.ndisProviderDate);
    this.rFormIncidentInfo.controls['DescribeIncident'].patchValue(this.response.responseData.clientIncidentDetails.incidentAllegtion);
    this.rFormIncidentInfo.controls['circumstanceIncident'].patchValue(this.response.responseData.clientIncidentDetails.allegtionCircumstances);
    if(this.response.responseData.clientIncidentDetails.otherLocation!="" && this.response.responseData.clientIncidentDetails.otherLocation!=null && this.response.responseData.clientIncidentDetails.otherLocation!=undefined){
      this.rFormIncidentInfo.controls['Location'].setValue(this.response.responseData.clientIncidentDetails.otherLocation);
      this.isShownLocation=true;
      
    }
    if(this.response.responseData.clientIncidentDetails.address!="" && this.response.responseData.clientIncidentDetails.address!=null && this.response.responseData.clientIncidentDetails.address!=undefined){
      this.rFormIncidentInfo.controls['otherlocation'].setValue(this.response.responseData.clientIncidentDetails.address);
      this.isShownOtherLocation=true
      this.isShownLocatiodropdown=false;
    }
     else if (this.response.responseData.clientIncidentDetails.locationId!=null&&this.response.responseData.clientIncidentDetails.locationId>0){
       this.getLocation();
       this.rFormIncidentInfo.controls['locationId'].patchValue(this.response.responseData.clientIncidentDetails.locationId);
      this.isShownOtherLocation=false;
      this.isShownLocatiodropdown=true;
    }
    this.textIncident="Update";
  }
  else{
   this.textIncident="Submit"
  }
   }
  });
  }
}
export interface TimeList {
  id?: string;
  text?: string;
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