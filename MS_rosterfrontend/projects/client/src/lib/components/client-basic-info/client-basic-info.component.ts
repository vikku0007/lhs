import { Component, OnInit, Input, ViewChild, ElementRef, OnChanges, SimpleChanges } from '@angular/core';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { ActivatedRoute } from '@angular/router';
import { NotificationService } from 'projects/core/src/lib/services/notification-service/notification.service';
import { ClientService } from '../../services/client.service';
import {ClientBasicData } from '../../view-models/client-basicinfo';
import { CommonService } from 'projects/lhs-service/src/lib/service/common/common.service';
import { ClientSupportCordinator } from '../../view-models/client-SupportCoordinator';
interface Language {
  value: string;
  name: string;
}
@Component({
  selector: 'lib-client-basic-info',
  templateUrl: './client-basic-info.component.html',
  styleUrls: ['./client-basic-info.component.css']
})
export class ClientBasicInfoComponent implements OnInit, OnChanges {
  @Input() BasicInfo: ClientSupportCordinator ;
  rForm: FormGroup;
  response: ResponseModel = {};
  LocationList:any;
  SalutationList:any;
  GenderList:any;
  @ViewChild('btnprimarycareCancel') cancel: ElementRef;
  @ViewChild('formDirective') private formDirective: NgForm;
  getErrorMessage:'Please Enter Value';
  responseModel: ResponseModel = {};
  languages: Language[] = [
    {value: '', name: 'Select Language'},
    {value: '1', name: 'English'},
    {value: '2', name: 'Spanish'},
    {value: '3', name: 'French'}
  ];
  selectedLang = '1';
  selectedRelationship = 'relation-1';
  ClientId: any;
  
  constructor(private route: ActivatedRoute, private fb: FormBuilder, private clientservice:ClientService, private notificationService: NotificationService
    ,private commonservice:CommonService) { }

  ngOnChanges(changes: SimpleChanges): void {
     this.route.queryParams.subscribe(params => {
        this.ClientId = parseInt(params['Id']);
      });
    }
   ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.ClientId = parseInt(params['Id']);
    });
    this.createForm();
    this.getClientDetails();
    this.GetGender();
    this.GetSalutation();
    
  }

  createForm() {
    this.rForm = this.fb.group({
      Salutation: [null, Validators.required],
      name: [null, Validators.required],
      gender: [null, Validators.required],
      EmailId: [null, Validators.compose([Validators.required, Validators.pattern(/^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/)])],
      phoneNo: [null, Validators.compose([Validators.nullValidator, Validators.minLength(8), Validators.maxLength(10)])],
      MobileNo: [null, Validators.compose([Validators.required, Validators.minLength(8), Validators.maxLength(10)])],
    });
  }
  getClientDetails() {
    const data = {
       Id: this.ClientId
     }
     this.clientservice.GetClientDetailPageInfo(data).subscribe(res => {
      this.responseModel = res;
       if (this.responseModel.status > 0) {
         this.BasicInfo = this.responseModel.responseData.clientSupportCoordinatorModel;
       
       }
       else {
         //this.notificationService.Error({ message: this.response.message, title: null });
       }
     });
   }
  editsupportInfoDetails() {
    this.rForm.get('Salutation').patchValue(this.BasicInfo.salutation);
    this.rForm.get('name').patchValue(this.BasicInfo.name);
    this.rForm.get('EmailId').patchValue(this.BasicInfo.emailId);
    this.rForm.get('phoneNo').patchValue(this.BasicInfo.phoneNo);
    this.rForm.get('MobileNo').patchValue(this.BasicInfo.mobileNo);
    this.rForm.get('gender').patchValue(this.BasicInfo.gender);
    
  }
 UpdateClientSupportcoordinatorInfo() {
    if (this.rForm.valid) {
      const data = {
        ClientId: this.ClientId,
        Salutation: this.rForm.value.Salutation,
        Name: this.rForm.value.name,
        EmailId: this.rForm.value.EmailId,
        MobileNo: String(this.rForm.value.MobileNo),
        Gender:this.rForm.value.gender,
        PhoneNo: String(this.rForm.value.phoneNo),
      }
      this.clientservice.AddClientSuppportCoordinator(data).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
            this.BasicInfo = this.response.responseData;
            this.cancel.nativeElement.click();
            this.getClientDetails();
           this.notificationService.Success({ message: this.response.message, title: null });
            break;

          default:
            this.notificationService.Error({ message: this.response.message, title: null });
            break;
        }
      });
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
}