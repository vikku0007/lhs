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
  selector: 'lib-incident-details',
  templateUrl: './incident-details.component.html',
  styleUrls: ['./incident-details.component.scss'],
  providers: [
    {
        provide: DateAdapter, useClass: AppDateAdapter
    },
    {
        provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS
    }
    ]
})

export class IncidentDetailsComponent implements OnInit {
  response: ResponseModel = {};
  rFormIncidentInfo: FormGroup;
  clientId=0;
  LocationList:any;
  ReportedToList:any;
  ReportedList:any;
  LocationTypeList: any;
  list: any;
  selectedType: any;
  textIncident: string;
  todayDatemax = new Date();
  @ViewChild('btnEditaccidentCancel') editCancel: ElementRef;
  @ViewChild('formDirective') private formDirective: NgForm;
  @ViewChild('fileInput', {static: false}) fileInput: ElementRef;
  selectedName: any;
  EventTypeList: any;
  isShownOtherLocation: boolean = false ;
  isShownLocatiodropdown: boolean = false ;
  isShownLocation: boolean = false ;
  baseUrl : string = environment.baseUrl;
  TimeDropdownList : TimeList [] = ELEMENT_DATA;
  shiftId: number;
  employeeId: any;
  constructor(private fb: FormBuilder,private membershipService: MembershipService,private notification:NotificationService, private route: ActivatedRoute,private logoutService:LogoutService,private commonservice:CommonService) {
    }
   ngOnInit(): void {
    this.route.paramMap.subscribe((params: any) => {
      this.clientId = Number(params.params.id);
      this.shiftId = Number(params.params.shiftId);
    });
     this.getLocationType();
     this.createFormIncident();
     this.getAccidentIncidentInfo();
     this.employeeId = this.membershipService.getUserDetails('employeeId');
  }
  get otherlocation() {
    return this.rFormIncidentInfo.get('otherlocation');
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
  
  
  getLocationType(){
    this.commonservice.getLocationType().subscribe((res=>{
      if(res){
        this.response = res;
        this.LocationTypeList=this.response.responseData||[];
       
      }else{

      }
    }));
  }
   formattedaddress=" "; 
   public handleAddressChange(address: any) {
   this.formattedaddress=address.address1
   this.rFormIncidentInfo.controls['otherlocation'].setValue(this.formattedaddress);
   
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
  
   getAccidentIncidentInfo() {
      const data = {
       Id : this.clientId,
       ShiftId : this.shiftId,

       };
     this.logoutService.getAccidentIncidentInfo(data).subscribe(res => {
      this.response = res;
     if (this.response.status > 0) {
     if(this.response.responseData.clientIncidentDetails.clientId>0){
      this.rFormIncidentInfo.controls['locationtype'].patchValue(this.response.responseData.clientIncidentDetails.locationType);
      this.rFormIncidentInfo.controls['Incidentdate'].patchValue(this.response.responseData.clientIncidentDetails.dateTime);
      this.rFormIncidentInfo.controls['Reason'].patchValue(this.response.responseData.clientIncidentDetails.unknowndateReason);
      this.rFormIncidentInfo.controls['NdisProviderTime'].patchValue(this.response.responseData.clientIncidentDetails.startTimeString=="12:00 AM"?"24:00":this.response.responseData.clientIncidentDetails.startTimeString=="12:30 AM"?"00:30":this.response.responseData.clientIncidentDetails.startTimeString=="01:00 AM"?"01:00"
      :this.response.responseData.clientIncidentDetails.startTimeString=="01:30 AM"?"01:30":
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
  
    AddIncidentDetails() {  
      if (this.rFormIncidentInfo.valid) {
        const data = {
          'ClientId': this.clientId,
          'ShiftId': this.shiftId,
          'EmployeeId': this.employeeId,
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
          this.logoutService.AddIncidentDetails(data).subscribe(res => {
          this.response = res;
          switch (this.response.status) {
            case 1:
             this.notification.Success({ message: this.response.message, title: null });
             this.getAccidentIncidentInfo()
              break;
             default:
              this.notification.Warning({ message: this.response.message, title: null });
              break;
          }
        });
      }
    }
   
}
export interface TimeList {
  id?: string;
  text?: string;
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