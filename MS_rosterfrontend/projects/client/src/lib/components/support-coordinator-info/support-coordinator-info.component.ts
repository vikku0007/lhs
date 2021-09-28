import { Component, OnInit, Input, ViewChild, ElementRef, OnChanges, SimpleChanges } from '@angular/core';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { ActivatedRoute } from '@angular/router';
import { NotificationService } from 'projects/core/src/lib/services/notification-service/notification.service';
import { ClientService } from '../../services/client.service';
import { ClientPrimarycarerInfo } from '../../view-models/client-primary-carerinfo';
import { CommonService } from 'projects/lhs-service/src/lib/service/common/common.service';
import { GuardianInfo } from '../../view-models/client-GuardianInfo';
import { AppDateAdapter } from 'projects/lhs-directives/src/projects';
import { APP_DATE_FORMATS } from 'projects/lhs-directives/src/lib/directives/date-format.directive';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';
interface RelationShip {
  id: number;
  codeDescription: string;
}
@Component({
  selector: 'lib-support-coordinator-info',
  templateUrl: './support-coordinator-info.component.html',
  styleUrls: ['./support-coordinator-info.component.scss'],
  providers: [
    {
        provide: DateAdapter, useClass: AppDateAdapter
    },
    {
        provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS
    }
    ]
})

export class SupportCoordinatorInfoComponent implements OnInit, OnChanges {
  @Input() primarycarerInfo: GuardianInfo;
  rForm: FormGroup;
  rFormInfo: FormGroup;
  response: ResponseModel = {};
  @ViewChild('btnprimarycareCancel') cancel: ElementRef;
  @ViewChild('formDirective') private formDirective: NgForm;
  getErrorMessage:'Please Enter Value';
  relationShipList: RelationShip[];
  isShownrelation: boolean = false ;
  ClientId: number;
  responseModel: ResponseModel = {};
  list: RelationShip[];
  selectedType: any;
  selectedName: string;
  GenderList: any;
  SalutationList: any;
  constructor(private route: ActivatedRoute,private commonService: CommonService, private fb: FormBuilder, private clientservice:ClientService, private notificationService: NotificationService) { }

  ngOnChanges(changes: SimpleChanges): void {
     this.route.queryParams.subscribe(params => {
        this.ClientId = parseInt(params['Id']);
      });
    }
  

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.ClientId = parseInt(params['Id']);
    });
    this.createFormInfo();
    this.createForm();
    this.getrelationShip();
    this.GetGender();
    this.GetSalutation();
    this.getClientDetails();
    
  }
  GetGender(){
    this.commonService.getGenderList().subscribe((res=>{
      if(res){
        this.responseModel = res;
        this.GenderList=this.responseModel.responseData||[];
       
      }else{

      }
    }));
  }
  getrelationShip(){
    
    this.commonService.getRelationShip().subscribe(res => {
      this.response = res;
     switch (this.response.status) {
        case 1:
         this.relationShipList = this.response.responseData;
          break;

        default:
          break;
      }
    });
  }
  GetSalutation(){
    this.commonService.getSalutation().subscribe((res=>{
      if(res){
        this.responseModel = res;
        this.SalutationList=this.responseModel.responseData||[];
       
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
         this.primarycarerInfo = this.responseModel.responseData.clientSupportCoordinatorModel;
       
      }
      else {
        //this.notificationService.Error({ message: this.response.message, title: null });
      }
    });
  }
  createForm() {
    this.rForm = this.fb.group({
      Name: [null, Validators.required],
     // RelationShip: ['', Validators.nullValidator],
      EmailId: [null, Validators.compose([Validators.required, Validators.pattern(/^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/)])],
      MobileNo: ['',[Validators.maxLength(16),Validators.required]],
      PhoneNo:['',[Validators.maxLength(16),Validators.nullValidator]],
      MiddleName: [null, Validators.nullValidator],
      LastName: [null, Validators.required],
      gender: [null, Validators.required],
     // otherrelation: [null, Validators.nullValidator],
      Salutation: [null, Validators.required],
    });
  }
  createFormInfo() {
    this.rFormInfo = this.fb.group({
      NameInfo: [],
      RelationInfo: [],
      EmailInfo: [],
      PhoneInfo: [],
      MobileInfo: [],
      
    });
  }
  editprimarycareDetails() {
   this.rForm.get('Name').patchValue(this.primarycarerInfo.firstName);
    //this.rForm.get('RelationShip').patchValue(this.primarycarerInfo.relationship);
    this.rForm.get('EmailId').patchValue(this.primarycarerInfo.emailId);
    this.rForm.get('MobileNo').patchValue(this.primarycarerInfo.mobileNo);
    this.rForm.get('PhoneNo').patchValue(this.primarycarerInfo.phoneNo);
    this.rForm.get('MiddleName').patchValue(this.primarycarerInfo.middleName);
    this.rForm.get('LastName').patchValue(this.primarycarerInfo.lastName);
    this.rForm.get('gender').patchValue(this.primarycarerInfo.gender);
    this.rForm.get('otherrelation').patchValue(this.primarycarerInfo.otherRelation);
    this.rForm.get('Salutation').patchValue(this.primarycarerInfo.salutation);
  }
 UpdatePrimaryCareInfo() {
    if (this.rForm.valid) {
      const data = {
        ClientId: this.ClientId,
        FirstName: this.rForm.value.Name,
        RelationShip: 0,
        EmailId: this.rForm.value.EmailId,
        MobileNo: String(this.rForm.value.MobileNo),
        PhoneNo:String(this.rForm.value.PhoneNo),
        MiddleName: this.rForm.value.MiddleName,
        LastName: this.rForm.value.LastName,
        OtherRelation:null,
        Gender:this.rForm.value.gender,
        Salutation: this.rForm.value.Salutation,
      }
      this.clientservice.AddClientSuppportCoordinator(data).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
            this.primarycarerInfo = this.response.responseData;
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
  selectChangeHandler(event:any) {
    this.list=this.relationShipList;
     this.selectedType = event;
    const index = this.list.findIndex(x => x.id == this.selectedType);
     this.selectedName= this.list[index].codeDescription;
     if (this.selectedName == "Other") {
       this.isShownrelation=true;
  }
  else{
    this.isShownrelation=false;
    this.rForm.get('otherrelation').patchValue("");
   
  }

   }
}
