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
  selector: 'lib-client-guardiani-information',
  templateUrl: './client-guardiani-information.component.html',
  styleUrls: ['./client-guardiani-information.component.scss'],
  providers: [
    {
        provide: DateAdapter, useClass: AppDateAdapter
    },
    {
        provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS
    }
    ]
})

export class ClientGuardianiInformationComponent implements OnInit, OnChanges {
  @Input() primarycarerInfo: GuardianInfo;
  rForm: FormGroup;
  rFormInfo: FormGroup;
  response: ResponseModel = {};
  @ViewChild('btnprimarycareCancel') cancel: ElementRef;
  @ViewChild('formDirective') private formDirective: NgForm;
  getErrorMessage:'Please Enter Value';
  relationShipList: RelationShip[];
 
  ClientId: number;
  responseModel: ResponseModel = {};
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
    this.getClientDetails();
   // this.getrelationShip();
    this.rForm.controls.RelationShip.setValue("Advocates");
    this.rForm.get('RelationShip').disable();
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

  getClientDetails() {
    
    const data = {
      Id: this.ClientId
    }
    this.clientservice.GetClientDetailPageInfo(data).subscribe(res => {
     this.responseModel = res;
      if (this.responseModel.status > 0) {
         this.primarycarerInfo = this.responseModel.responseData.clientGuardianModels;
       
      }
      else {
        //this.notificationService.Error({ message: this.response.message, title: null });
      }
    });
  }
  createForm() {
    this.rForm = this.fb.group({
      Name: [null, Validators.required],
      RelationShip: ['', Validators.required],
      EmailId: [null, Validators.compose([Validators.required, Validators.pattern(/^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/)])],
      MobileNo: [null,[Validators.minLength(17),Validators.maxLength(17),Validators.required]],
      PhoneNo:[null,[Validators.minLength(17),Validators.maxLength(17),Validators.required]],
      MiddleName: [null, Validators.nullValidator],
      LastName: [null, Validators.required],
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
    this.rForm.get('RelationShip').patchValue(this.primarycarerInfo.relationShip);
    this.rForm.get('EmailId').patchValue(this.primarycarerInfo.email);
    this.rForm.get('MobileNo').patchValue(this.primarycarerInfo.contactNo);
    this.rForm.get('PhoneNo').patchValue(this.primarycarerInfo.phoneNo);
    this.rForm.get('MiddleName').patchValue(this.primarycarerInfo.middleName);
    this.rForm.get('LastName').patchValue(this.primarycarerInfo.lastName);
  }
 UpdatePrimaryCareInfo() {
    if (this.rForm.valid) {
      const data = {
        ClientId: this.ClientId,
        FirstName: this.rForm.value.Name,
        RelationShip: "Advocates",
        Email: this.rForm.value.EmailId,
        ContactNo: String(this.rForm.value.MobileNo),
        PhoneNo:String(this.rForm.value.PhoneNo),
        MiddleName: this.rForm.value.MiddleName,
        LastName: this.rForm.value.LastName,
      }
      this.clientservice.AddGuardianInfo(data).subscribe(res => {
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

}