import { Component, OnInit, AfterViewInit, ViewChild, Input } from '@angular/core';
import { ApiService, MembershipService, NotificationService } from 'projects/core/src/projects';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { ActivatedRoute } from '@angular/router';
import { CommonService } from 'projects/lhs-service/src/lib/service/common/common.service';
import { AppDateAdapter } from 'projects/lhs-directives/src/projects';
import { APP_DATE_FORMATS } from 'projects/lhs-directives/src/lib/directives/date-format.directive';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';
import { ClientDetails } from '../../view-models/add-client-details';
import { ClientService } from 'projects/client/src/lib/services/client.service';
import { LogoutService } from '../../services/logout.service';
import { MedicalHistory } from '../../view-models/client-medical-history';

interface Gender {
  value: string;
  name: string;
}

// For Condition
interface Condition {
  value: string;
  name: string;
}

// For Symptom
interface Symptom {
  value: string;
  name: string;
}

// For Consume Alcohol

interface ConsumeAlcohol {
  value: string;
  name: string;
}

@Component({
  selector: 'lib-medical-history',
  templateUrl: './medical-history.component.html',
  styleUrls: ['./medical-history.component.scss']
})

export class MedicalHistoryComponent implements OnInit, AfterViewInit {
  consumeAlcohols: ConsumeAlcohol[] = [
    { value: '', name: 'Select An Option' },
    { value: 'occasionally', name: 'Occasionally' },
    { value: 'yes', name: 'Yes' },
    { value: 'no', name: 'No' }
  ];
 getErrorMessage: 'Please Enter Value';
 rForm: FormGroup;
  clientId: number;
  responseModel: ResponseModel = {};
  clientInfo: ClientDetails = {};
  clientMedicalInfo: MedicalHistory = {};
  EditList: MedicalHistory[] = [];
  ClientId: number;
  genders: Gender[] = [];
  @Input() BasicInfo: ClientDetails;
  @ViewChild('formDirective') private formDirective: NgForm;
  EditId: number;
  ConditionList: any;
  SymptomsList: any;
  genderlist: any;
  isShownmedication: boolean = false ;
  list: any;
  selectedType: any;
  selectedName: any;
  isShownCondition: boolean = false ;selectedSymptom: any;
  isShownSymptoms: boolean = false ;
  listsymptom: any;
  btntext: string;
  shiftId: number;
  selectedvalue: any;
  employeeId: any;

  constructor(private logoutService:LogoutService,private notificationService: NotificationService, private fb: FormBuilder, private route: ActivatedRoute,
    private commonService: CommonService,private membershipService: MembershipService) {
      this.route.paramMap.subscribe((params: any) => {
        this.clientId = Number(params.params.id);
        this.shiftId = Number(params.params.shiftId);
      });
  }

  ngOnInit(): void {
    this.employeeId = this.membershipService.getUserDetails('employeeId');
    this.createForm();
    this.getGender();
    this.getConditionType();
    this.getSymptomsType();
    this.Checkradiobutton();
  }
 
  ngAfterViewInit(): void {
    this.getPrimaryInfo();
    this.getClientMedicalInfo();
  }
  Checkradiobutton(){
    this.rForm.get('name').disable();
    this.rForm.get('mobileNo').disable();
    this.rForm.get('gender').disable();
    this.rForm.get('isTakingMedication').setValue("0");
    this.rForm.get('isMedicationAllergy').setValue("0");
    this.rForm.get('isTakingTobacco').setValue("0");
    this.rForm.get('isTakingIllegalDrug').setValue("0");
  }
  createForm() {
    this.rForm = this.fb.group({
      name: [null, Validators.required],
      mobileNo: ['',[Validators.maxLength(13),Validators.required]],
      gender: [null, Validators.required],
      checkCondition: [null, Validators.required],
      checkSymptoms: [null, Validators.required],
      isTakingMedication: [null, Validators.required],
      isMedicationAllergy: [null, Validators.required],
      isTakingTobacco: [null, Validators.required],
      isTakingIllegalDrug: [null, Validators.required],
      takingAlcohol: [null, Validators.required],
      Medication: [null, Validators.nullValidator],
      OtherCondition: [null, Validators.nullValidator],
      OtherSymptoms: [null, Validators.nullValidator]
    });
  }

  getGender() {
    this.commonService.getGenderList().subscribe((res: any) => {
      this.responseModel = res;
      switch (this.responseModel.status) {
        case 1:
          this.genderlist=this.responseModel.responseData || [];
         break;
         default:
          break;
      }
    });
  }

  getPrimaryInfo() {
    const data = {
      id: this.clientId
    }
    this.logoutService.getClientPrimaryInfo(data).subscribe((res: any) => {
      this.responseModel = res;
      switch (this.responseModel.status) {
        case 1:
          this.clientInfo = this.responseModel.responseData;
          this.rForm.controls['name'].patchValue(this.clientInfo.fullName);
          this.rForm.controls['mobileNo'].patchValue(this.clientInfo.mobileNo);
          this.rForm.controls['gender'].patchValue(this.clientInfo.gender);
          break;
         case 0:
          this.notificationService.Error({ message: 'some Error occured', title: null });
          break;
          default:
          break;
      }
    })
  }

  getClientMedicalInfo() {
     const data = {
      clientId: this.clientId,
      ShiftId:this.shiftId
    }
    this.logoutService.getClientMedicalHistory(data).subscribe((res: any) => {
      this.responseModel = res;
      if(this.responseModel.responseData!=null){
        this.btntext="Update";
      }
      else{
        this.btntext="Submit";
      }
      switch (this.responseModel.status) {
        case 1:
          this.clientMedicalInfo = this.responseModel.responseData;
          this.rForm.controls['checkCondition'].patchValue(this.clientMedicalInfo?.checkCondition);
          this.rForm.controls['checkSymptoms'].patchValue(this.clientMedicalInfo?.checkSymptoms);
          this.rForm.controls['isTakingMedication'].patchValue(this.clientMedicalInfo?.isTakingMedication == true ? '1' : '0');
          this.rForm.controls['isMedicationAllergy'].patchValue(this.clientMedicalInfo?.isMedicationAllergy == true ? '1' : '0');
          this.rForm.controls['isTakingIllegalDrug'].patchValue(this.clientMedicalInfo?.isTakingIllegalDrug == true ? '1' : '0');
          this.rForm.controls['isTakingTobacco'].patchValue(this.clientMedicalInfo?.isTakingTobacco == true ? '1' : '0');
          this.rForm.controls['takingAlcohol'].patchValue(this.clientMedicalInfo?.takingAlcohol);
          this.rForm.controls['gender'].patchValue(this.clientMedicalInfo.gender);
          if(this.clientMedicalInfo.isTakingMedication==true){
           if(this.clientMedicalInfo.medicationDetail!=null && this.clientMedicalInfo.medicationDetail!=""){
              this.rForm.controls['Medication'].patchValue(this.clientMedicalInfo.medicationDetail);
              this.isShownmedication=true;
            }
            else{
              this.rForm.controls['Medication'].patchValue("");
              this.isShownmedication=false;
            }
          }
            if(this.clientMedicalInfo.otherCondition!=null&&this.clientMedicalInfo.otherCondition!=""){
              this.rForm.controls['OtherCondition'].patchValue(this.clientMedicalInfo.otherCondition);
              this.isShownCondition=true;
            }
            else{
              this.rForm.controls['OtherCondition'].patchValue("");
              this.isShownCondition=false;
            }
            if(this.clientMedicalInfo.otherSymptoms!=null&&this.clientMedicalInfo.otherSymptoms!=""){
              this.rForm.controls['OtherSymptoms'].patchValue(this.clientMedicalInfo.otherSymptoms);
              this.isShownSymptoms=true;
            }
            else{
              this.rForm.controls['OtherSymptoms'].patchValue("");
              this.isShownSymptoms=false;
            }
         
          
         break;
        case 0:
         break;
        default:
        break;
      }
    })

  }
  SelectOtherCondition(event:any) {
    this.list=this.ConditionList;
    this.selectedType = event;
    const index = this.list.findIndex(x => x.id == this.selectedType);
    this.selectedName= this.list[index].codeDescription;
    if(this.selectedName=="Other"){
      this.isShownCondition=true;
    }
    else{
      this.isShownCondition=false;
      this.rForm.controls['OtherCondition'].patchValue("");
    }
  }
  SelectOtherSymptoms(event:any) {
    this.listsymptom=this.SymptomsList;
    this.selectedvalue = event;
    const index = this.listsymptom.findIndex(x => x.id == this.selectedvalue);
    this.selectedSymptom= this.listsymptom[index].codeDescription;
    if(this.selectedSymptom=="Other"){
      this.isShownSymptoms=true;
     
    }
    else{
      this.isShownSymptoms=false;
      this.rForm.controls['OtherSymptoms'].patchValue("");
    }
  }
  addClientMedicalDetails() {
      if (this.rForm.valid) {
      this.clientMedicalInfo = this.rForm.value;
      this.clientMedicalInfo.clientId = this.clientId;
      this.clientMedicalInfo.employeeId = this.employeeId;
      this.clientMedicalInfo.name=this.rForm.controls['name'].value,
      this.clientMedicalInfo.gender=this.rForm.controls['gender'].value,
      this.clientMedicalInfo.mobileNo=String(this.rForm.controls['mobileNo'].value),
      this.clientMedicalInfo.isMedicationAllergy = Boolean(this.rForm.controls['isMedicationAllergy'].value == '0' ? false : true);
      this.clientMedicalInfo.isTakingIllegalDrug = this.rForm.controls['isTakingIllegalDrug'].value == '0' ? false : true;
      this.clientMedicalInfo.isTakingMedication = Boolean(this.rForm.controls['isTakingMedication'].value == '0' ? false : true);
      this.clientMedicalInfo.isTakingTobacco = Boolean(this.rForm.controls['isTakingTobacco'].value == '0' ? false : true);
      this.clientMedicalInfo.medicationDetail = (this.rForm.controls['Medication'].value );
      this.clientMedicalInfo.otherCondition = (this.rForm.controls['OtherCondition'].value );
      this.clientMedicalInfo.otherSymptoms = (this.rForm.controls['OtherSymptoms'].value );
      this.clientMedicalInfo.shiftId=this.shiftId;
      this.logoutService.addClientMedicalHistory(this.clientMedicalInfo).subscribe((res: any) => {
      this.responseModel = res;
      switch (this.responseModel.status) {
      case 1:
      this.notificationService.Success({ message: this.responseModel.message, title: '' });
      this.rForm.reset();
      this.formDirective.resetForm();
      this.getClientMedicalInfo();
      this.rForm.controls['name'].patchValue(this.clientMedicalInfo.name);
      this.rForm.controls['mobileNo'].patchValue(this.clientMedicalInfo.mobileNo);
      this.rForm.controls['gender'].patchValue(this.clientMedicalInfo.gender);
      break;
      case 0:
      this.notificationService.Warning({ message: this.responseModel.message, title: null });
      break;
      default:
      this.rForm.controls['name'].patchValue(this.clientMedicalInfo.name);
      this.rForm.controls['mobileNo'].patchValue(this.clientMedicalInfo.mobileNo);
      this.rForm.controls['gender'].patchValue(this.clientMedicalInfo.gender);
      break;
        }
      })
    }
  }
  getConditionType(){
    this.commonService.getConditionType().subscribe((res=>{
      if(res){
        this.responseModel = res;
        this.ConditionList=this.responseModel.responseData || [];
       
      }else{

      }
    }));
  }
  getSymptomsType(){
    this.commonService.getSymptomsType().subscribe((res=>{
      if(res){
        this.responseModel = res;
        this.SymptomsList=this.responseModel.responseData || [];
       
      }else{

      }
    }));
  }
  showmedication(){
    if(this.rForm.value.isTakingMedication=='1'){
      this.isShownmedication=true;
    }
    else{
      this.isShownmedication=false;
      
    }
  }
}