import { Component, OnInit, Input, ViewChild, ElementRef, AfterViewInit, OnChanges, SimpleChanges, Output ,EventEmitter, NgZone} from '@angular/core';
import { EmployeeDetails } from '../../viewmodel/employee-details';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { EmpServiceService } from '../../services/emp-service.service';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { ActivatedRoute } from '@angular/router';
import { NotificationService } from 'projects/core/src/lib/services/notification-service/notification.service';
import { CommonService } from 'projects/lhs-service/src/lib/service/common/common.service';
import { Salutation, Language, EmpType, MaritalStatus, EmpLevel } from 'projects/lhs-service/src/lib/viewmodels/gender';
import * as moment from 'moment';
import { environment } from 'src/environments/environment';
import { GooglePlaceDirective } from 'ngx-google-places-autocomplete';
import { MapsAPILoader } from '@agm/core';
import { types } from 'util';
import PlaceResult = google.maps.places.PlaceResult;
import { timingSafeEqual } from 'crypto';
import { Subject, ReplaySubject } from 'rxjs';
import { MatSelect } from '@angular/material/select';
import { takeUntil } from 'rxjs/operators';
@Component({
  selector: 'lib-employee-primaryinfo',
  templateUrl: './employee-primaryinfo.component.html',
  styleUrls: ['./employee-primaryinfo.component.scss']
})
export class EmployeePrimaryInfoComponent implements OnInit, OnChanges {
  @Output() onAddressChange = new EventEmitter<any>();
  @Input() empPrimaryInfo: EmployeeDetails;
  empBasicInfoForm: FormGroup;
  response: ResponseModel = {};
  @ViewChild('btnCancel') cancel: ElementRef;
  @ViewChild('btnImageCancel') btnImageCancel: ElementRef;
  @ViewChild('fileInput', {static: false}) fileInput: ElementRef;
  public control: FormControl = new FormControl();
  public searchcontrol: FormControl = new FormControl();
  private _onDestroy = new Subject<void>();
  public filteredRecords: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
  @ViewChild('Select') select: MatSelect;
  ImageName:string ='';
  ImageSize: string = '';
  RoleList: any;
  salutationList: Salutation[] = [];
  languageList: Language[] = [];
  empType: EmpType[] = [];
  maritalStatusList: MaritalStatus[] = [];
  empLevelList: EmpLevel[] = [];
  emppicForm: FormGroup;
  visaTypeList: any;
  baseUrl : string = environment.baseUrl;
  todayDate: any;
  todayDatemax = new Date();
  CountryList: any;
  StateList: any;
  isShownVisa: boolean = false ;
  isShownPassportNumber: boolean = false ;
  isShownAustralian: boolean = false ;
  religionList: any;
  hobbiesList: [];
  hobbieslist: any;
  HobbiesId: any;
  isShownOtherReligion: boolean = false ;
  isShownOtherLanguage: boolean = false ;
  list: any;
  selectedType: any;
  selectedother: any;
   constructor(private route: ActivatedRoute, private fb: FormBuilder, private empService: EmpServiceService,
    private notificationService: NotificationService, private commonservice: CommonService,
    ) { }

  ngOnChanges(changes: SimpleChanges): void {
    this.createForm();    
    console.log('primar component' + this.empPrimaryInfo)
  }

  ngOnInit(): void {
    this.GetRole();
    this.getSalutation();
    this.getLanguage();
    this.getEmpType();
    this.getMaritalStatus();
    this.getEmpLevel();
    this.getVisaType();
    this.getCountry();
    this.getState();
    this.getReligion();
    this.getHobbies();
    this.checkOthervalue();
    this.Checkbutton();
    this.searchroletype();
    if (this.empPrimaryInfo.id == 0) {
      this.route.queryParams.subscribe(params => {
        this.empPrimaryInfo.id = parseInt(params['Id']);
      });
    } 
    this.hobbieslist=this.empPrimaryInfo.employeeHobbiesModel;
    this.HobbiesId=this.hobbieslist.map(x=>x.hobbies);
    this.empBasicInfoForm.controls['hobbies'].setValue(this.HobbiesId);
  }
  Checkbutton(){
    if(this.empPrimaryInfo.hasVisa== true){
      this.isShownVisa=false;
      this.isShownPassportNumber=true;
    }
    else{
      this.isShownVisa=true;
      this.isShownPassportNumber=true;
    }
    if(this.empPrimaryInfo.isAustralian== true){
      this.isShownVisa=false;
      this.isShownPassportNumber=true;
     
    }
    else{
      this.isShownVisa=true;
      this.isShownPassportNumber=true;
     }
  }
  radioChange() {
   if(this.empBasicInfoForm.value.hasVisa=="2"){
    this.isShownVisa=true;
    this.isShownPassportNumber=true;
    }
    else if(this.empBasicInfoForm.value.hasVisa=="1"){
      this.empBasicInfoForm.controls['visaNumber'].setValue("");
      this.empBasicInfoForm.controls['visaType'].setValue("");
      this.empBasicInfoForm.controls['visaExpiryDate'].setValue("");
      this.isShownVisa=false;
      this.isShownPassportNumber=true;
      this.empBasicInfoForm.controls['australiancitizen'].setValue("1");
      }
  }
  AustralianChange() {
  if(this.empBasicInfoForm.value.australiancitizen=="2"){
   this.isShownVisa=true;
   this.isShownPassportNumber=true;
   }
   else if(this.empBasicInfoForm.value.hasVisa=="1"){
    this.isShownVisa=false;
    this.isShownPassportNumber=true;
    this.empBasicInfoForm.controls['visaNumber'].setValue("");
    this.empBasicInfoForm.controls['visaType'].setValue("");
    this.empBasicInfoForm.controls['visaExpiryDate'].setValue("");
   
     }
  }
  searchroletype(){
    this.control.valueChanges
    .pipe(takeUntil(this._onDestroy))
    .subscribe(() => {
      this.filterroletype();
    });
  }
  private filterroletype() {
    if (!this.RoleList) {
      return;
    }
   
    let search = this.control.value;
    if (!search) {
      this.filteredRecords.next(this.RoleList.slice());
      return;
    } else {
      search = search.toLowerCase();
    }
    if (search.length >= 1) {
     this.filteredRecords.next(
      this.RoleList.filter(role => role.codeDescription.toLowerCase().indexOf(search) > -1)
     );
    }
  }
  ImageDelete(){
    this.ImageName =  '';
    this.ImageSize =  '';
    this.fileInput.nativeElement.value = '';
  }
  ImageChanges(){
    const fileInput = this.fileInput.nativeElement;
    let fileLength = fileInput.files.length;
    let file = fileInput.files[0];
    if(fileLength > 0)
    {
     
      if(!this.isImageType(file.type,name)){
        this.notificationService.Error({ message: 'Image should be .jpg, .png', title: null });
        this.fileInput.nativeElement.value = '';
        return;
      }
      this.ImageName =  file.name;
      this.ImageSize =  file.length;
    }
  }
    isImageType(type,name){
    return (type.match('image.*') || name.match(/\.(gif|png|jpe?g)$/i)) ? true : false;
};
  AddImage(){
    const fileInput = this.fileInput.nativeElement;
    let fileLength = fileInput.files.length;
    let file = fileInput.files[0];
    if(fileLength > 0)
    {
      var type =  file.type;
      var name =  file.name;
      if(this.isImageType(type,name)){
        const formData = new FormData();  
        formData.append('Files', fileInput.files[0]);
        formData.append('EmployeeId', this.empPrimaryInfo.id.toString()); 
        this.empService.UpdateEmployeeProfileImage(formData).subscribe(res => {
        
          this.response = res;
          switch (this.response.status) {
            case 1:
              this.empPrimaryInfo.imageUrl = this.response.responseData.path;
              this.btnImageCancel.nativeElement.click();
              this.notificationService.Success({ message: 'Employee Image Updated Successfully', title: null });
  
              break;
              case 0:
                this.notificationService.Error({ message: 'Something went wrong '+ this.response.responseData.message, title: null });
                break;
  
            default:
              break;
          }
        });
      }
      else
      {
        this.notificationService.Error({ message: 'Image should be .jpg, .png', title: null });
      }
      
    }
    else
    {
      this.notificationService.Warning({ message: 'Please select image', title: null });
    }
   // fileInput.click();  
  }
  getAge(value) {
    let newDate = new Date(value);
    let timeDiff = Math.abs(Date.now() - newDate.getTime());
    let age = Math.floor((timeDiff / (1000 * 3600 * 24)) / 365.25);
    return age;
  }
 
  createForm() {
    this.empBasicInfoForm = this.fb.group({
      salutation: [this.empPrimaryInfo.saluation, Validators.required],
      firstname: [this.empPrimaryInfo.firstName, Validators.required],
      middleName: [this.empPrimaryInfo.middleName, Validators.nullValidator],
      lastName: [this.empPrimaryInfo.lastName, Validators.required],
      emailId: [this.empPrimaryInfo.emailId, Validators.required],
     // mobileNo: [this.empPrimaryInfo.mobileNo,Validators.pattern(/^\(\d{3}\)\s\d{3}-\d{4}$/), Validators.required],
      mobileNo: [this.empPrimaryInfo.mobileNo,[Validators.maxLength(16),Validators.required]],
      employeeLevel: [this.empPrimaryInfo.employeeLevel, Validators.required],
      address1: [this.empPrimaryInfo.address1, Validators.required],
      status: [this.empPrimaryInfo.status == true ? '1' : '0', Validators.required],
      role: [this.empPrimaryInfo.role==0?null:this.empPrimaryInfo.role, Validators.required],
      language: [this.empPrimaryInfo.language, Validators.required],
      empType: [this.empPrimaryInfo.empType, Validators.required],
      country: [this.empPrimaryInfo.country, Validators.required],
      city: [this.empPrimaryInfo.city, Validators.required],
      id: [this.empPrimaryInfo.id, Validators.required],
      state: [this.empPrimaryInfo.state, Validators.required],
      code: [this.empPrimaryInfo.code, Validators.required],
      maritalStatus: [this.empPrimaryInfo.maritalStatus == 0? null: this.empPrimaryInfo.maritalStatus, Validators.required],
      employeeId: [this.empPrimaryInfo.employeeId, Validators.nullValidator],
      dateOfBirth: [this.empPrimaryInfo.dateOfBirth, Validators.required],
      hasVisa: [this.empPrimaryInfo.hasVisa== true ? '1' : '2', Validators.required],
      visaNumber: [this.empPrimaryInfo.visaNumber, Validators.nullValidator],
      passportNumber: [this.empPrimaryInfo.passportNumber, Validators.nullValidator],
      visaExpiryDate: [this.empPrimaryInfo.visaExpiryDate, Validators.nullValidator],
      visaType: [this.empPrimaryInfo.visaType, Validators.nullValidator],
      religion:[this.empPrimaryInfo.religion, Validators.required],
      hobbies:['', Validators.nullValidator],
      otherhobbies:[this.empPrimaryInfo.otherHobbies, Validators.nullValidator],
      otherreligion:[this.empPrimaryInfo.otherReligion, Validators.nullValidator],
      otherLanguage:[this.empPrimaryInfo.otherLanguage, Validators.nullValidator],
      australiancitizen: [this.empPrimaryInfo.isAustralian== true ? '1' : '2', Validators.required],
    });
  }
  checkOthervalue()
  {
   
    if(this.empPrimaryInfo.otherReligion!="" &&this.empPrimaryInfo.otherReligion!=null){
      this.isShownOtherReligion=true;
    }
    else{
      this.isShownOtherReligion=false;
    }
    if(this.empPrimaryInfo.otherLanguage!="" && this.empPrimaryInfo.otherLanguage!=null){
      this.isShownOtherLanguage=true;
    }
    else{
      this.isShownOtherLanguage=false;
    }
  }
  get salutation() {
    return this.empBasicInfoForm.get('salutation');
  }

  get firstName() {
    return this.empBasicInfoForm.get('firstname');
  }

  get middleName() {
    return this.empBasicInfoForm.get('middleName');
  }

  get lastName() {
    return this.empBasicInfoForm.get('lastName');
  }

  get email() {
    return this.empBasicInfoForm.get('emailId');
  }

  get role() {
    return this.empBasicInfoForm.get('role');
  }
  get employeeId() {
    return this.empBasicInfoForm.get('employeeId');
  }

  get mobileNo() {
    return this.empBasicInfoForm.get('mobileNo');
  }

  get employeeType() {
    return this.empBasicInfoForm.get('empType');
  }
  get language() {
    return this.empBasicInfoForm.get('language');
  }

  get status() {
    return this.empBasicInfoForm.get('status');
  }
  get id() {
    return this.empBasicInfoForm.get('id');
  }


  get address1() {
    return this.empBasicInfoForm.get('address1');
  }
 
  get employeeLevel() {
    return this.empBasicInfoForm.get('employeeLevel');
  }
  get country() {
    return this.empBasicInfoForm.get('country');
  }
  get state() {
    return this.empBasicInfoForm.get('state');
  }
  get city() {
    return this.empBasicInfoForm.get('city');
  }
  get maritalStatus() {
    return this.empBasicInfoForm.get('maritalStatus');
  }
  get code() {
    return this.empBasicInfoForm.get('code');
  }
  get hasVisa() {
    return this.empBasicInfoForm.get('hasVisa');
  }
  get visaExpiryDate() {
    return this.empBasicInfoForm.get('visaExpiryDate');
  }
  get passportNumber() {
    return this.empBasicInfoForm.get('passportNumber');
  }
  get visaNumber() {
    return this.empBasicInfoForm.get('visaNumber');
  }
  get visaType() {
    return this.empBasicInfoForm.get('visaType');
  }
  get religion() {
    return this.empBasicInfoForm.get('religion');
  }
  get hobbies() {
    return this.empBasicInfoForm.get('hobbies');
  }
  get otherhobbies() {
    return this.empBasicInfoForm.get('otherhobbies');
  }
  get otherreligion() {
    return this.empBasicInfoForm.get('otherreligion');
  }
  get otherLanguage() {
    return this.empBasicInfoForm.get('otherLanguage');
  }
  get australiancitizen() {
    return this.empBasicInfoForm.get('australiancitizen');
  }
  EditEmployee() {
    if (this.empBasicInfoForm.valid) {
      var empInfoModel: EmployeeDetails = {};
      empInfoModel = this.empBasicInfoForm.value;
      
      if(this.hasVisa.value=="2"){
        empInfoModel.visaExpiryDate=moment(this.empBasicInfoForm.get('visaExpiryDate').value).format("YYYY-MM-DD");
        empInfoModel.visaType=this.visaType.value;
      }
      else{
        empInfoModel.visaExpiryDate=null;
        empInfoModel.visaType=null;
      }
      empInfoModel.mobileNo = String(this.mobileNo.value);
      empInfoModel.mobileNo = empInfoModel.mobileNo;//.replace(/^(\+[0-9]\d{0,1})/,'').replace(/\D/g, '');
      empInfoModel.saluation=this.salutation.value;
      empInfoModel.status = this.status.value === '1' ? true : false;
      empInfoModel.hasVisa= Boolean(this.hasVisa.value == '1' ? true : false);
      empInfoModel.isAustralian= Boolean(this.australiancitizen.value == '1' ? true : false);
      empInfoModel.code = Number(this.code.value);
      empInfoModel.hobbies = (this.hobbies.value);
       this.empService.EditEmployeeDetails(empInfoModel).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
            this.empPrimaryInfo = this.response.responseData;
            this.hobbieslist=this.empPrimaryInfo.employeeHobbiesModel;
            this.HobbiesId=this.hobbieslist.map(x=>x.hobbies);
            this.empBasicInfoForm.controls['hobbies'].setValue(this.HobbiesId);
            this.cancel.nativeElement.click();
            this.notificationService.Success({ message: 'Employee Updated successfully', title: null });
            break;
            default:
            break;
        }
      });

    }
  }
  GetRole() {
    this.commonservice.getRoles().subscribe((res => {
      if (res) {
        this.response = res;
        this.RoleList = this.response.responseData || [];
        this.filteredRecords.next(this.RoleList.slice());
      } else {

      }
    }));
  }
  getSalutation() {
    this.commonservice.getSalutation().subscribe((res => {
      if (res) {
        this.response = res;
        this.salutationList = this.response.responseData;
      }
    }));
  }
  getLanguage() {
    this.commonservice.getLanguageList().subscribe((res => {
      if (res) {
        this.response = res;
        this.languageList = this.response.responseData;
      }
    }));
  }
  getEmpType() {
    this.commonservice.getEmployeeType().subscribe((res => {
      if (res) {
        this.response = res;
        this.empType = this.response.responseData;
      }
    }));
  }
  getMaritalStatus() {
    this.commonservice.getMaritalStatus().subscribe((res => {
      if (res) {
        this.response = res;
        this.maritalStatusList = this.response.responseData;
      }
    }));
  }
  getEmpLevel() {
    this.commonservice.getLevel().subscribe((res => {
      if (res) {
        this.response = res;
        this.empLevelList = this.response.responseData;
      }
    }));
  }
  getVisaType() {
    this.commonservice.getVisaType().subscribe((res => {
      if (res) {
        this.response = res;
        this.visaTypeList = this.response.responseData;
      }
    }));
  }
  getCountry() {
    this.commonservice.getCountry().subscribe((res => {
      if (res) {
        this.response = res;
        this.CountryList = this.response.responseData;
      }
    }));
  }
  getState() {
    this.commonservice.getstate().subscribe((res => {
      if (res) {
        this.response = res;
        this.StateList = this.response.responseData;
      }
    }));
  }
  getReligion() {
    this.commonservice.getReligion().subscribe((res => {
      if (res) {
        this.response = res;
        this.religionList = this.response.responseData;
      }
    }));
  }
  getHobbies() {
    this.commonservice.getHobbies().subscribe((res => {
      if (res) {
        this.response = res;
        this.hobbiesList = this.response.responseData;
      }
    }));
  }
  getAgedob() {
    this.todayDate = this.empBasicInfoForm.value.dateOfBirth;
        var today = new Date();
        var birthDate = new Date(this.todayDate);
        var age = today.getFullYear() - birthDate.getFullYear();
        var m = today.getMonth() - birthDate.getMonth();
        var da = today.getDate() - birthDate.getDate();
        if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
            age--;
        }
        if(m<0){
            m +=12;
        }
        if(da<0){
            da +=30;
        }
    
      if(age < 18 || age > 100)
    {
      this.notificationService.Warning({ message: 'Date Of Birth Should be greater than 18 years', title: '' });
      this.empBasicInfoForm.controls.dateOfBirth.setValue("");
      return;
    } else {
     
    }
   
    }
    formattedaddress=" "; 
    public handleAddressChange(addressObj: any) {
      this.formattedaddress=addressObj.address1
      this.empBasicInfoForm.controls['address1'].setValue(this.formattedaddress);
      this.empBasicInfoForm.controls['country'].setValue(addressObj.country);
      this.empBasicInfoForm.controls['city'].setValue(addressObj.city);
      this.empBasicInfoForm.controls['state'].setValue(addressObj.state);
      this.empBasicInfoForm.controls['code'].setValue(addressObj.zip);
    }
    getOtherreligion(event) { 
      this.list=this.religionList;
      this.selectedType = event;
      const index = this.list.findIndex(x => x.id == this.selectedType);
      this.selectedother= this.list[index].codeDescription;
      if(this.selectedother=="Other"){   
       this.isShownOtherReligion=true;
      }
      else { 
        this.isShownOtherReligion=false;
        this.empBasicInfoForm.controls['otherreligion'].patchValue("");
      }
    }
    getOtherLanguage(event) { 
      debugger
      this.list=this.languageList;
      this.selectedType = event;
      const index = this.list.findIndex(x => x.id == this.selectedType);
      this.selectedother= this.list[index].codeDescription;
      if(this.selectedother=="Other"){   
       this.isShownOtherLanguage=true;
      }
      else { 
        this.isShownOtherLanguage=false;
        this.empBasicInfoForm.controls['otherLanguage'].patchValue("");
      }
    }
}
