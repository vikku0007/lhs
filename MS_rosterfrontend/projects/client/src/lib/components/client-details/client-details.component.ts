import { Component, OnInit, ViewChild, ElementRef, Input } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ClientViewModel } from '../../view-models/Client-view-model';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { ActivatedRoute } from '@angular/router';
import { NotificationService } from 'projects/core/src/lib/services/notification-service/notification.service';
import { ClientService } from '../../services/client.service';
import { ClientBasicData } from '../../view-models/client-basicinfo';
import { CommonService } from 'projects/lhs-service/src/lib/service/common/common.service';
import { Constants } from '../../config/constants';
import { environment } from 'src/environments/environment';
import * as moment from 'moment';
import { AppDateAdapter } from 'projects/lhs-directives/src/projects';
import { APP_DATE_FORMATS } from 'projects/lhs-directives/src/lib/directives/date-format.directive';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';
interface Language {
  value: string;
  name: string;
}

interface Relation {
  value: string;
  name: string;
}

interface FundType {
  value: string;
  name: string;
}

//For Funding Info
export interface PeriodicElementFundingInfo {
  fundType: string;
  amount: string;
  hours: number;
  expiry: string;
}

//For Funding Info
const ELEMENT_DATA_FUNDING_INFO: PeriodicElementFundingInfo[] = [
  { fundType: 'Fund Type', amount: '$2,500', hours: 11.5, expiry: '10-May-2020' },
  { fundType: 'Fund Type', amount: '$1,150', hours: 9.5, expiry: '12-Jun-2020' },
];

@Component({
  selector: 'lib-client-details',
  templateUrl: './client-details.component.html',
  styleUrls: ['./client-details.component.scss'],
  providers: [
    {
        provide: DateAdapter, useClass: AppDateAdapter
    },
    {
        provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS
    }
    ]
})

export class ClientDetailsComponent implements OnInit {
  getErrorMessage: 'Please Enter Value';

  //For Language
  
  selectedLang = 'lang-0';

  //For Relationship
  relations: Relation[] = [
    { value: '', name: 'Select Language' },
    { value: 'relation-0', name: 'Sister' },
    { value: 'relation-1', name: 'Brother' },
    { value: 'relation-2', name: 'Mother' },
    { value: 'relation-2', name: 'Father' }
  ];
  selectedRelationship = 'relation-1';

  //For Fund Type
  ftypes: FundType[] = [
    { value: '', name: 'Select FundType' },
    { value: 'ftype-1', name: 'Fund Type 1' },
    { value: 'ftype-2', name: 'Fund Type 2' },
    { value: 'ftype-3', name: 'Fund Type 3' }
  ];
  selectedFundType = 'ftype-1';

  //For Funding Info
  displayedColumnsFundingInfo: string[] = ['fundType', 'amount', 'hours', 'expiry', 'action'];
  dataSourceFundingInfo = new MatTableDataSource(ELEMENT_DATA_FUNDING_INFO);

  @Input() BasicInfo: ClientBasicData;
  
  rForm: FormGroup;
  response: ResponseModel = {};
  @ViewChild('fileInput', {static: false}) fileInput: ElementRef;
  @ViewChild('btnprimarycareCancel') cancel: ElementRef;
  @ViewChild('formDirective') private formDirective: NgForm;
  @ViewChild('btnImageCancel') btnImageCancel: ElementRef;
  LocationList:any;
  SalutationList:any;
  GenderList:any;
  ClientId: number;
  ServiceTypeList: any;
  ImageName: string;
  ImageSize: string;
  clientPic: any;
  baseUrl : string = environment.baseUrl;
  todayDatemax = new Date();
  todayDate:any;
  LocationTypeList: any;
  list: any;
  selectedType: any;
  locationId: any;
  status: boolean;
  
  delete(elm) {
    this.dataSourceFundingInfo.data = this.dataSourceFundingInfo.data.filter(i => i !== elm)
  }
  responseModel: ResponseModel = {};
  pageData: ClientViewModel = {
    ClientPrimaryInfo: {},
    ClientPrimaryCareInfo: { clientId: 0 },
    clientOnboadingInfo: {},
    ClientAdditionalNotes: {},
    ClientFundingInfo: {},

  };
  employeeid = 0;
  constructor(private route: ActivatedRoute, private fb: FormBuilder, 
    private clientservice: ClientService, private notificationService: NotificationService,private commonservice:CommonService) { }
    isShownOtherLocation: boolean = false ;
    isShownLocatiodropdown: boolean = false ;
  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.ClientId = parseInt(params['Id']);
    });
    this.createForm();
    this.getClientDetails();
    this.GetGender();
    this.GetSalutation();
    this.GetLocation();
    this.getServiceType();
    this.getLocationType();
    this.rForm.value.addHasExpiry="1";
    
  }

  createForm() {
    this.rForm = this.fb.group({
      Salutation: [null, Validators.required],
      FirstName: [null, Validators.required],
      MiddleName: [],
      LastName: [null, Validators.required],
      DOB: [null, Validators.required],
      EmailId: [null, Validators.compose([Validators.nullValidator, Validators.pattern(/^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/)])],
     // TelephoneNo: [null, Validators.compose([null,Validators.required,Validators.pattern(/^(\+?\(61\)|\(\+?61\)|\+?61|\(0[1-9]\)|0[1-9])?( ?-?[0-9]){7,9}$/)])],
      TelephoneNo:['',[Validators.maxLength(16),Validators.nullValidator]],
     // Location: [null, Validators.nullValidator],
      Address: [null, Validators.required],
      gender:[null, Validators.required],
      ServiceType:['', Validators.nullValidator],
      NDIS:['', Validators.nullValidator],
      //locationtype: ['', Validators.required],
      //otherlocation: ['', Validators.nullValidator],
    });
  }
  get Address() {
    return this.rForm.get('Address');
  }
  getLocationType(){
    this.commonservice.getLocationType().subscribe((res=>{
      if(res){
        this.responseModel = res;
        this.LocationTypeList=this.responseModel.responseData||[];
       
      }else{

      }
    }));
  }
  getClientDetails() {
    const data = {
      Id: this.ClientId
    }
    this.clientservice.GetClientDetailPageInfo(data).subscribe(res => {
      this.responseModel = res;
      debugger;
      if (this.responseModel.status > 0) {
        this.BasicInfo = this.responseModel.responseData.clientPrimaryInfo;
        this.BasicInfo.fullName = this.BasicInfo.firstName + ' ' + (this.BasicInfo?.middleName ? this.BasicInfo.middleName : ' ') + ' ' +
          this.BasicInfo.lastName;
          this.clientPic=this.baseUrl + this.BasicInfo.imageUrl;
         
      }
      else {
        //this.notificationService.Error({ message: this.response.message, title: null });
      }
    });
  }
  editBasicInfoDetails() {
     this.rForm.patchValue({
      Salutation: this.BasicInfo.salutation,
      FirstName: this.BasicInfo.firstName,
      MiddleName: this.BasicInfo.middleName,
      LastName: this.BasicInfo.lastName,
      DOB:this.BasicInfo.dateOfBirth,
      EmailId:this.BasicInfo.emailId,
      TelephoneNo:this.BasicInfo.mobileNo,
       Address:this.BasicInfo.address,
      gender:this.BasicInfo.gender,
      ServiceType:this.BasicInfo.serviceType,
      NDIS:this.BasicInfo.ndis,
     // locationtype:this.BasicInfo.locationType
    });
    // if(this.BasicInfo.otherLocation!=""){
    //   this.rForm.controls['otherlocation'].setValue(this.BasicInfo.otherLocation);
    //   this.isShownOtherLocation=true;
    //   this.isShownLocatiodropdown=false;
    // }
    // else if(this.BasicInfo.locationId!=null){
    //   this.rForm.controls['Location'].setValue(this.BasicInfo.locationId);
    //   this.isShownOtherLocation=false;
    //   this.isShownLocatiodropdown=true;
     
    // }
  }
  getAge() {
    
   this.todayDate = this.rForm.value.DOB;
  // var newDate = moment(this.todayDate).add(1, 'year').format('YYYY-MM-DD').toString();
  // var dateString = document.getElementById("date");
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
     this.notificationService.Warning({ message: 'DOB should be greater than 18 years', title: '' });
     this.rForm.controls.DOB.setValue("");
     return;
   } else {
   
   
   }
   
  
   }

  UpdateClientBasicInfo() {
    if (this.rForm.valid) {
      // if(this.rForm.value.Location==null||this.rForm.value.Location=="")  {
      //   this.locationId = null;
      // }
      // else{
      //   this.locationId = parseInt(this.rForm.value.Location);
      // }
      
      const data = {
        Id: this.ClientId,
        Salutation: this.rForm.value.Salutation,
        FirstName: this.rForm.value.FirstName,
        MiddleName: this.rForm.value.MiddleName,
        LastName: this.rForm.value.LastName,
        DateOfBirth: moment(this.rForm.value.DOB).format('YYYY-MM-DD').toString(),
        EmailId: this.rForm.value.EmailId,
        MobileNo: String(this.rForm.value.TelephoneNo),
      //  LocationId: (this.locationId),
        Address: this.rForm.value.Address,
        Gender:this.rForm.value.gender,
        ServiceType:this.rForm.value.ServiceType,
        NDIS:this.rForm.value.NDIS,
         // OtherLocation:this.rForm.value.otherlocation
      }
      this.clientservice.UpdateClientBasicInfo(data).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
            this.BasicInfo = this.response.responseData;
            this.cancel.nativeElement.click();
            this.notificationService.Success({ message: this.response.message, title: null });
            this.getClientDetails();
            break;

          default:
            this.notificationService.Error({ message: this.response.message, title: null });
            break;
        }
      });
    }
  }
  selectChangeHandler(event:any) {
    this.list=this.LocationTypeList;
    this.selectedType = event;
   if(this.selectedType==5){
      this.GetLocation()
      this.isShownLocatiodropdown=true;
      this.isShownOtherLocation=false;
      this.rForm.controls['otherlocation'].patchValue("");
    }
    else{
      this.isShownLocatiodropdown=false;
      this.isShownOtherLocation=true;
    }
  }
  GetLocation(){
    this.commonservice.getLocation().subscribe((res=>{
      if(res){
        this.responseModel = res;
        this.LocationList=this.responseModel.responseData||[];
       
      }else{

      }
    }));
  }
  GetSalutation(){
    this.commonservice.getSalutation().subscribe((res=>{
      if(res){
        this.responseModel = res;
        this.SalutationList=this.responseModel.responseData||[];
       
      }else{

      }
    }));
  }
  GetGender(){
    this.commonservice.getGenderList().subscribe((res=>{
      if(res){
        this.responseModel = res;
        this.GenderList=this.responseModel.responseData||[];
       
      }else{

      }
    }));
  }
  getServiceType(){
    this.commonservice.getServiceType().subscribe((res=>{
      if(res){
        this.responseModel = res;
        this.ServiceTypeList=this.responseModel.responseData||[];
       
      }else{

      }
    }));
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
        formData.append('ClientId', this.ClientId.toString()); 
        this.clientservice.UpdateClientProfileImage(formData).subscribe(res => {
          
          this.response = res;
          switch (this.response.status) {
            case 1:
              this.BasicInfo.imageUrl = this.response.responseData.path;
              this.btnImageCancel.nativeElement.click();
              this.notificationService.Success({ message: 'Client Image Updated successfully', title: null });
              this.clientPic=this.baseUrl + this.BasicInfo.imageUrl;
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
  formattedaddress=" "; 
 public handleAddressChange(address: any) {
   this.formattedaddress=address.address1
   this.rForm.controls['Address'].setValue(this.formattedaddress);
   
 }
 GeneratePassword(){
   debugger;
  const data = {
    ClientId: this.ClientId,
    EmailId : null
  }
  this.clientservice.GenerateUserANdPas(data).subscribe(res => {
    this.response = res;
    switch (this.response.status) {
      case 1:
        
      this.BasicInfo.userName = this.response.responseData.userName;
        this.notificationService.Success({ message: this.response.message, title: null });
     //   this.getClientDetails();
        break;

      default:
        this.notificationService.Error({ message: this.response.message, title: null });
        break;
    }
  });
 }

 
}