import { Component, OnInit, AfterViewInit, ViewChild, Input } from '@angular/core';
import { ApiService, MembershipService, NotificationService } from 'projects/core/src/projects';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { ClientService } from '../../services/client.service';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { AddClientDetails } from '../../view-models/add-client-details';
import { ActivatedRoute } from '@angular/router';
import { ClientMedicalHistory } from '../../view-models/client-medical-history';
import { ClientBasicData } from '../../view-models/client-basicinfo';

import { Gender } from 'projects/lhs-service/src/lib/viewmodels/gender';
import { CommonService } from 'projects/lhs-service/src/lib/service/common/common.service';
import { AppDateAdapter } from 'projects/lhs-directives/src/projects';
import { APP_DATE_FORMATS } from 'projects/lhs-directives/src/lib/directives/date-format.directive';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';



// For Event Type
// interface Gender {
//   value: string;
//   name: string;
// }

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
  selector: 'lib-medical-history-details',
  templateUrl: './medical-history-details.component.html',
  styleUrls: ['./medical-history-details.component.scss'],
  providers: [
    {
        provide: DateAdapter, useClass: AppDateAdapter
    },
    {
        provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS
    }
    ]
})
export class MedicalHistoryDetailsComponent implements OnInit, AfterViewInit {


  getErrorMessage: 'Please Enter Value';

  //For Person
  // genders: Gender[] = [
  //   { value: '', name: 'Select Gender' },
  //   { value: 'male', name: 'Male' },
  //   { value: 'female', name: 'Female' },
  //   { value: 'other', name: 'Other' }
  // ];
  // selectedGender = 'male';

  //For Condition
  conditions: Condition[] = [
    { value: '', name: 'Select Condition' },
    { value: 'asthma', name: 'Asthma' },
    { value: 'cancer', name: 'Cancer' },
    { value: 'cardiac', name: 'Cardiac Disease' }
  ];
  // selectedCondition = 'condition-1';

  //For Symptom
  symptoms: Symptom[] = [
    { value: '', name: 'Select Symptom' },
    { value: 'cardiovascular', name: 'Cardiovascular' },
    { value: 'hematological', name: 'Hematological' },
    { value: 'lymphatic', name: 'Lymphatic' }
  ];
  // selectedSymptom = 'symptom-1';

  //For ConsumeAlcohol
  consumeAlcohols: ConsumeAlcohol[] = [
    { value: '', name: 'Select An Option' },
    { value: 'occasionally', name: 'Occasionally' },
    { value: 'yes', name: 'Yes' },
    { value: 'no', name: 'No' }
  ];
  // selectedConsumeAlcohol = 'consumeAlcohol-1';

  rForm: FormGroup;
  clientId: number;
  responseModel: ResponseModel = {};
  clientInfo: AddClientDetails = {};
  clientMedicalInfo: ClientMedicalHistory = {};
  EditList: ClientMedicalHistory[] = [];
  ClientId: number;
  genders: Gender[] = [];
  @Input() BasicInfo: ClientBasicData;
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
  selectedvalue: any;
  ShiftId: number;
  EmpId: number;
;
  constructor(private clientService: ClientService, private membershipService: MembershipService,
    private notificationService: NotificationService, private fb: FormBuilder, private route: ActivatedRoute,
    private commonService: CommonService) {
    this.route.queryParams.subscribe(params => {
      this.clientId = Number(params.Id);
    
    });
   
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.ClientId = parseInt(params['Id']);  
      this.EditId = Number(params.EId);
      this.ShiftId = parseInt(params['ShiftId']); 
      this.EmpId = parseInt(params['EmpId']); 
    });
    this.createForm();
    this.getGender();
    this.getConditionType();
    this.getSymptomsType();
    this.Checkradiobutton();
    if(this.EditId>0){
      this.getMedicalById();
     }
    
  }
  getMedicalById(){
    const data = {
      Id: Number(this.EditId),
     
    };
    this.clientService.getClientPrimaryInfo(data).subscribe(res => {
      this.responseModel = res;
         this.EditList = this.responseModel.responseData;
         this.rForm.controls['checkCondition'].patchValue(this.EditList[0]['checkCondition']);
         this.rForm.controls['checkSymptoms'].patchValue(this.EditList[0]['checkSymptoms']);
         this.rForm.controls['isTakingMedication'].patchValue(this.EditList[0]['isTakingMedication'] == true ? '1' : '0');
         this.rForm.controls['isMedicationAllergy'].patchValue(this.EditList[0]['isMedicationAllergy'] == true ? '1' : '0');
         this.rForm.controls['isTakingIllegalDrug'].patchValue(this.EditList[0]['isTakingIllegalDrug'] == true ? '1' : '0');
         this.rForm.controls['isTakingTobacco'].patchValue(this.EditList[0]['isTakingTobacco'] == true ? '1' : '0');
         this.rForm.controls['takingAlcohol'].patchValue(this.EditList[0]['takingAlcohol']);
         this.rForm.controls['gender'].patchValue(this.EditList[0]['gender']);
         if(this.clientMedicalInfo?.isTakingMedication==true){
          if(this.EditList[0]['medicationDetail']!=null||this.EditList[0]['medicationDetail']!=""){
            this.rForm.controls['Medication'].patchValue(this.EditList[0]['medicationDetail']);
            this.isShownmedication=true;
          }
          else{
            this.rForm.controls['Medication'].patchValue("");
            this.isShownmedication=true;
          }
        }
      })
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
      mobileNo: ['',[Validators.maxLength(16),Validators.required]],
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
         // this.genders = this.responseModel.responseData;
          this.genderlist=this.responseModel.responseData || [];
        //  this.genders.forEach(x => x.value = x.codeDescription.toLowerCase());
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
    this.clientService.getClientPrimaryInfo(data).subscribe((res: any) => {
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
         // this.notificationService.Error({ message: 'some Error occured', title: null });
          break;
      }
    })
  }

  getClientMedicalInfo() {
    const data = {
      clientId: this.clientId,
      ShiftId:this.ShiftId
    }
    this.clientService.getClientMedicalHistory(data).subscribe((res: any) => {
      this.responseModel = res;
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
         // this.notificationService.Error({ message: 'some Error occured', title: null });
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
      this.clientMedicalInfo.shiftId = this.ShiftId;
      this.clientMedicalInfo.employeeId = this.EmpId;
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
      this.clientService.addClientMedicalHistory(this.clientMedicalInfo).subscribe((res: any) => {
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
