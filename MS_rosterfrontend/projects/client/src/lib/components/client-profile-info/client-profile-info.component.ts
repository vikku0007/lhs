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
import { isBuffer } from 'util';

@Component({
  selector: 'lib-client-profile-info',
  templateUrl: './client-profile-info.component.html',
  styleUrls: ['./client-profile-info.component.scss']
})
export class ClientProfileInfoComponent implements OnInit {
  rFormsocial: FormGroup;
  rFormarrangement: FormGroup;
  rFormotherinfo: FormGroup;
  rFormcultureneed: FormGroup;
  rFormpreferences: FormGroup;
  rFormeatingnutritioninfo: FormGroup;
  rFormbehaviourconcern: FormGroup;
  response: ResponseModel = {};
  clientbehaviourconcern: ResponseModel = {};
  clientprofileinfo: ResponseModel = {};
  ClientId: number;
  SocialconnectionInfo: any;
  isShownindependent: boolean = false ;
  isShownmeal: boolean = false ;
  isShownutensils: boolean = false ;
  isShownfoodffluids: boolean = false ;
  isShowndislikes: boolean = false ;
  isShownhistorydetails: boolean = false ;
  isShownhistorydescription: boolean = false ;
  isShownabscondingdescription: boolean = false ;
  isShownconcern: boolean = false ;
  isShownBSP: boolean = false ;
  textsocial: string;
  textculture: string;
  textPersonalPreference: string;
  textLivingArrangement: string;
  textNutritionModel: string;
  textBehaviourofconcern: string;
  textotherinfo: string;
  socialconnectioninfo: any;
  cultureneedinfo: any;
  clientimportanceinfo: any;
  charecteristicsinfo: any;
  anxitiesinfo: any;
  livingarrangementinfo: any;
  otherinformationinfo: any;
  interestinfo: any;
  Iseatsindependent: string;
  eatingdetails: any;
  Ispreparingmeals: string;
  mealdetails: any;
  Isutensils: string;
  utensilsdetails: any;
  Isfluids: string;
  foodfluiddetails: any;
  Ismodifiedfood: string;
  IsPEG: string;
  Isswallowing: string;
  Isdietplan: string;
  likedislikedetails: any;
  Ischokinghistory: string;
  chokingdetails: any;
  foodpreferencedetails: any;
  abscondingdescription: any;
  Ishistoryfalls: string;
  historydescription: any;
  Isabsconding: string;
  IsBSP: string;
  BSPdetails: any;
  Isbehaviourconcern: string;
  behaviourdetails: any;
  Isselfinjury: string;
  Iskicking: string;
  Ispinching: string;
  Isscreaming: string;
  Ishitting: string;
  Isbiting: string;
  Ispullinghair: string;
  Isthrowing: string;
  Isheadbutting: string;
  Isobsessive: string;
  Isother: string;
  Isbanging: string;
  constructor(private route: ActivatedRoute, private fb: FormBuilder, 
    private clientservice: ClientService, private notificationService: NotificationService,private commonservice:CommonService) { }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.ClientId = parseInt(params['Id']);
    });
    this.createFormsocial();
    this.createFormcultureneed();
    this.createFormotherinfo();
    this.createFormPreference();
    this.createFormarrangement();
    this.createFormEatingNutrition();
    this.createFormBehaviour();
    this.getClientDetails();
    this.getClientbehaviourDetails();
  }
  createFormsocial() {
    this.rFormsocial = this.fb.group({
      socialconnection: [null, Validators.required],
    });
  }
  createFormcultureneed() {
    this.rFormcultureneed = this.fb.group({
      cultureneed: [null, Validators.required],
    });
  }
  createFormPreference() {
    this.rFormpreferences = this.fb.group({
      interest: [null, Validators.required],
      clientimportance: [null, Validators.required],
      charecteristics: [null, Validators.required],
      anxities: [null, Validators.required],
    });
  }
  createFormarrangement() {
    this.rFormarrangement = this.fb.group({
      livingarrangement: [null, Validators.required],
    });
  }
  createFormotherinfo() {
    this.rFormotherinfo = this.fb.group({
      otherinformation: [null, Validators.required],
    });
  }
  createFormEatingNutrition() {
    this.rFormeatingnutritioninfo = this.fb.group({
      Iseatsindependent: ['',null],
      eatingdetails: [null, Validators.nullValidator],
      Ispreparingmeals: ['',null],
      mealdetails: [null, Validators.nullValidator],
      Isutensils: ['',null],
      utensilsdetails: [null, Validators.nullValidator],
      Isfluids: ['',null],
      foodfluiddetails: [null, Validators.nullValidator],
      Ismodifiedfood: ['',null],
      IsPEG: ['',null],
      Isswallowing: ['',null],
      Isdietplan: ['',null],
      likedislikedetails: [null, Validators.nullValidator],
      Ischokinghistory: ['',null],
      chokingdetails: [null, Validators.nullValidator],
      foodpreferencedetails: [null, Validators.nullValidator],
    });
  }
  createFormBehaviour() {
    this.rFormbehaviourconcern = this.fb.group({
      Ishistoryfalls: ['',null],
      historydescription: [null, Validators.nullValidator],
      Isabsconding: ['',null],
      abscondingdescription: [null, Validators.nullValidator],
      Isbehaviourconcern: ['',null],
      Isselfinjury: ['',null],
      Iskicking: ['',null],
      Ispinching: ['',null],
      Isscreaming: ['',null], 
      Ishitting: ['',null],
      Isbiting: ['',null],
      Ispullinghair: ['',null],
      Isthrowing: ['',null],
      Isheadbutting: ['',null],
      Isobsessive: ['',null],
      Isbanging: ['',null],
      Isother: ['',null],
      behaviourdetails: [null, Validators.nullValidator],
      IsBSP: ['',null],
      BSPdetails: [null, Validators.nullValidator],
      
    });
  }
  AddSocialconnectioninfo() {
    if (this.rFormsocial.valid) {
      const data = {
        ClientId: this.ClientId,
        SocialConnection: this.rFormsocial.value.socialconnection,
         
      }
      this.clientservice.AddClientSocialConnectionsInfo(data).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
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
  AddcultureNeedinfo() {
    if (this.rFormcultureneed.valid) {
      const data = {
        ClientId: this.ClientId,
        CultureNeed: this.rFormcultureneed.value.cultureneed,
         
      }
      this.clientservice.AddClientCultureNeedInfo(data).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
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

  AddClientPreferenceinfo() {
    if (this.rFormpreferences.valid) {
      const data = {
        ClientId: this.ClientId,
        Interest: this.rFormpreferences.value.interest,
        ClientImportance: this.rFormpreferences.value.clientimportance,
        Charecteristics: this.rFormpreferences.value.charecteristics,
        FearsandAnxities: this.rFormpreferences.value.anxities,
         
      }
      this.clientservice.AddClientPreferencesInfo(data).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
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

  AddLivingArrangementinfo() {
    if (this.rFormarrangement.valid) {
      const data = {
        ClientId: this.ClientId,
        LivingArrangement: this.rFormarrangement.value.livingarrangement
         
      }
      this.clientservice.AddClientLivingArrangementInfo(data).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
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

  AddClientOtherInfo() {
    if (this.rFormotherinfo.valid) {
      const data = {
        ClientId: this.ClientId,
        OtherInformation: this.rFormotherinfo.value.otherinformation,
         
      }
      this.clientservice.AddClientOtherInformtionInfo(data).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
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

  getClientDetails() {
    const data = {
       Id: this.ClientId
     }
     this.clientservice.GetClientProfileDetails(data).subscribe(res => {
      this.response = res;
      this.clientprofileinfo=res;
      debugger;
       if (this.response.status > 0) {
         if(this.response.responseData.clientSocialConnectionsModel.clientId>0){
           this.textsocial="Update";
         }
         else{
          this.textsocial="Submit"
         }
         if(this.response.responseData.clientCultureNeedModel.clientId>0){
          this.textculture="Update";
        }
        else{
         this.textculture="Submit"
        }
        if(this.response.responseData.clientPersonalPreferencesModel.clientId>0){
          this.textPersonalPreference="Update";
        }
        else{
         this.textPersonalPreference="Submit"
        }
        if(this.response.responseData.clientLivingArrangementModel.clientId>0){
          this.textLivingArrangement="Update";
        }
        else{
         this.textLivingArrangement="Submit"
        }
        if(this.response.responseData.clientEatingNutritionModel.clientId>0){
          this.textNutritionModel="Update";
        }
        else{
         this.textNutritionModel="Submit"
        }
      
        if(this.response.responseData.clientOtherInformtionModel.clientId>0){
          this.textotherinfo="Update";
        }
        else{
         this.textotherinfo="Submit"
        }
         this.rFormsocial.controls['socialconnection'].patchValue(this.response.responseData.clientSocialConnectionsModel.socialConnection);
         this.rFormcultureneed.controls['cultureneed'].patchValue(this.response.responseData.clientCultureNeedModel.cultureNeed);
         this.rFormpreferences.controls['interest'].patchValue(this.response.responseData.clientPersonalPreferencesModel.interest);
         this.rFormpreferences.controls['clientimportance'].patchValue(this.response.responseData.clientPersonalPreferencesModel.clientImportance);
         this.rFormpreferences.controls['charecteristics'].patchValue(this.response.responseData.clientPersonalPreferencesModel.charecteristics);
         this.rFormpreferences.controls['anxities'].patchValue(this.response.responseData.clientPersonalPreferencesModel.fearsandAnxities);
         this.rFormarrangement.controls['livingarrangement'].patchValue(this.response.responseData.clientLivingArrangementModel.livingArrangement);
         this.rFormotherinfo.controls['otherinformation'].patchValue(this.response.responseData.clientOtherInformtionModel.otherInformation);
         this.rFormeatingnutritioninfo.controls['Iseatsindependent'].patchValue(this.response.responseData.clientEatingNutritionModel.isEatsIndependently==true?'1':'');
         this.rFormeatingnutritioninfo.controls['eatingdetails'].patchValue(this.response.responseData.clientEatingNutritionModel.eatingNutritionDetails);
         this.rFormeatingnutritioninfo.controls['Ispreparingmeals'].patchValue(this.response.responseData.clientEatingNutritionModel.isPreparingMeals==true?'1':'');
         this.rFormeatingnutritioninfo.controls['mealdetails'].patchValue(this.response.responseData.clientEatingNutritionModel.mealsDetails);
         this.rFormeatingnutritioninfo.controls['Isutensils'].patchValue(this.response.responseData.clientEatingNutritionModel.isUsesUtensils==true?'1':'');
         this.rFormeatingnutritioninfo.controls['utensilsdetails'].patchValue(this.response.responseData.clientEatingNutritionModel.utensilsDetails);
         this.rFormeatingnutritioninfo.controls['Isfluids'].patchValue(this.response.responseData.clientEatingNutritionModel.isFluids==true?'1':'');
         this.rFormeatingnutritioninfo.controls['foodfluiddetails'].patchValue(this.response.responseData.clientEatingNutritionModel.fluidsDetails);
         this.rFormeatingnutritioninfo.controls['Ismodifiedfood'].patchValue(this.response.responseData.clientEatingNutritionModel.isModifiedFood==true?'1':'');
         this.rFormeatingnutritioninfo.controls['IsPEG'].patchValue(this.response.responseData.clientEatingNutritionModel.isPEG==true?'1':'');
         this.rFormeatingnutritioninfo.controls['Isswallowing'].patchValue(this.response.responseData.clientEatingNutritionModel.isSwallowingImpairment==true?'1':'');
         this.rFormeatingnutritioninfo.controls['Isdietplan'].patchValue(this.response.responseData.clientEatingNutritionModel.isDietPlan==true?'1':'');
         this.rFormeatingnutritioninfo.controls['likedislikedetails'].patchValue(this.response.responseData.clientEatingNutritionModel.allergiesDetails);
         this.rFormeatingnutritioninfo.controls['Ischokinghistory'].patchValue(this.response.responseData.clientEatingNutritionModel.hasChoking==true?'1':'');
         this.rFormeatingnutritioninfo.controls['chokingdetails'].patchValue(this.response.responseData.clientEatingNutritionModel.chokingDetails);
         this.rFormeatingnutritioninfo.controls['foodpreferencedetails'].patchValue(this.response.responseData.clientEatingNutritionModel.foodPreferences);
        }
       else {
         //this.notificationService.Error({ message: this.response.message, title: null });
       }
     });
   }
  
   AddClienteatingNutritionInfo() {
    if (this.rFormeatingnutritioninfo.valid) {
      if(this.rFormeatingnutritioninfo.value.Iseatsindependent=='1'){
        if(this.rFormeatingnutritioninfo.value.eatingdetails==null){
          this.notificationService.Warning({ message: "Please Enter Eating and Nutrition Details", title: null });
         return;
        }
      }
     else if(this.rFormeatingnutritioninfo.value.Ispreparingmeals==null){
        if(this.rFormeatingnutritioninfo.value.mealdetails==""){
          this.notificationService.Warning({ message: "Please Enter Meals Details", title: null });
         return;
        }
      }
      else if(this.rFormeatingnutritioninfo.value.Isutensils==null){
        if(this.rFormeatingnutritioninfo.value.utensilsdetails==""){
          this.notificationService.Warning({ message: "Please Enter Utensils Details", title: null });
         return;
        }
      }
      else if(this.rFormeatingnutritioninfo.value.Isfluids==null){
        if(this.rFormeatingnutritioninfo.value.foodfluiddetails==""){
          this.notificationService.Warning({ message: "Please Enter Food Details", title: null });
         return;
        }
      }
      else if(this.rFormeatingnutritioninfo.value.Isdietplan==null){
        if(this.rFormeatingnutritioninfo.value.likedislikedetails==""){
          this.notificationService.Warning({ message: "Please Enter Likes/Dislikes", title: null });
         return;
        }
      }
      else if(this.rFormeatingnutritioninfo.value.Ischokinghistory==null){
        if(this.rFormeatingnutritioninfo.value.chokingdetails==""){
          this.notificationService.Warning({ message: "Please Enter  Details", title: null });
         return;
        }
      }
      const data = {
        ClientId: this.ClientId,
        IsEatsIndependently: this.rFormeatingnutritioninfo.value.Iseatsindependent== "1" ? true : false,
        EatingNutritionDetails: this.rFormeatingnutritioninfo.value.eatingdetails,
        IsPreparingMeals: this.rFormeatingnutritioninfo.value.Ispreparingmeals== "1" ? true : false,
        MealsDetails: this.rFormeatingnutritioninfo.value.mealdetails,
        IsUsesUtensils: this.rFormeatingnutritioninfo.value.Isutensils== "1" ? true : false,
        UtensilsDetails: this.rFormeatingnutritioninfo.value.utensilsdetails,
        IsFluids: this.rFormeatingnutritioninfo.value.Isfluids== "1" ? true : false,
        FluidsDetails: this.rFormeatingnutritioninfo.value.foodfluiddetails,
        IsModifiedFood: this.rFormeatingnutritioninfo.value.Ismodifiedfood== "1" ? true : false,
        IsPEG: this.rFormeatingnutritioninfo.value.IsPEG== "1" ? true : false,
        IsSwallowingImpairment: this.rFormeatingnutritioninfo.value.Isswallowing== "1" ? true : false,
        IsDietPlan: this.rFormeatingnutritioninfo.value.Isdietplan== "1" ? true : false,
        AllergiesDetails: this.rFormeatingnutritioninfo.value.likedislikedetails,
        HasChoking: this.rFormeatingnutritioninfo.value.Ischokinghistory== "1" ? true : false,
        ChokingDetails: this.rFormeatingnutritioninfo.value.chokingdetails,
        FoodPreferences: this.rFormeatingnutritioninfo.value.foodpreferencedetails,
         
      }
      debugger;
      this.clientservice.AddClientEatingNutritionInfo(data).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
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
  AddClientBehaviourInfo() {
    if (this.rFormbehaviourconcern.valid) {
      if(this.rFormbehaviourconcern.value.Ishistoryfalls=='1'){
        if(this.rFormbehaviourconcern.value.historydescription==null){
          this.notificationService.Warning({ message: "Please Enter History Of Falls Description", title: null });
         return;
        }
      }
     else if(this.rFormeatingnutritioninfo.value.Isabsconding=='1'){
        if(this.rFormeatingnutritioninfo.value.abscondingdescription==null){
          this.notificationService.Warning({ message: "Please Enter History Of Absconding", title: null });
         return;
        }
      }
      else if(this.rFormeatingnutritioninfo.value.IsBSP=='1'){
        if(this.rFormeatingnutritioninfo.value.BSPdetails==null){
          this.notificationService.Warning({ message: "Please Enter Behaviour Support Plan", title: null });
         return;
        }
      }
      else if(this.rFormeatingnutritioninfo.value.Isbehaviourconcern=='1'){
        if(this.rFormeatingnutritioninfo.value.behaviourdetails==""){
          this.notificationService.Warning({ message: "Please Enter List Of Behaviour", title: null });
         return;
        }
      }
    
      const data = {
        ClientId: this.ClientId,
        IsHistoryFalls: this.rFormbehaviourconcern.value.Ishistoryfalls== "1" ? true : false,
        HistoryFallsDetails: this.rFormbehaviourconcern.value.historydescription,
        IsAbsconding: this.rFormbehaviourconcern.value.Isabsconding== "1" ? true : false,
        AbscondingHistory: this.rFormbehaviourconcern.value.abscondingdescription,
        IsBehaviourConcern: this.rFormbehaviourconcern.value.Isbehaviourconcern== "1" ? true : false,
        IsSelfInjury: this.rFormbehaviourconcern.value.Isselfinjury== "1" ? true : false,
        IsKicking: this.rFormbehaviourconcern.value.Iskicking== "1" ? true : false,
        IsPinching: this.rFormbehaviourconcern.value.Ispinching== "1" ? true : false,
        IsScreaming: this.rFormbehaviourconcern.value.Isscreaming== "1" ? true : false,
        IsHitting: this.rFormbehaviourconcern.value.Ishitting== "1" ? true : false,
        IsBiting: this.rFormbehaviourconcern.value.Isbiting== "1" ? true : false,
        IsPullingHair: this.rFormbehaviourconcern.value.Ispullinghair== "1" ? true : false,
        IsThrowing: this.rFormbehaviourconcern.value.Isthrowing== "1" ? true : false,
        IsHeadButting: this.rFormbehaviourconcern.value.Isheadbutting== "1" ? true : false,
        IsObsessiveBehaviour: this.rFormbehaviourconcern.value.Isobsessive== "1" ? true : false,
        IsBanging: this.rFormbehaviourconcern.value.Isbanging== "1" ? true : false,
        IsOther: this.rFormbehaviourconcern.value.Isother== "1" ? true : false,
        BehaviourConcernDetails: this.rFormbehaviourconcern.value.behaviourdetails,
        IsBSP: this.rFormbehaviourconcern.value.IsBSP== "1" ? true : false,
        BSPDetails: this.rFormbehaviourconcern.value.BSPdetails,
        
         
      }
     
      this.clientservice.AddClientBehaviourConcernInfo(data).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
            this.getClientbehaviourDetails();
            this.notificationService.Success({ message: this.response.message, title: null });
            break;

          default:
            this.notificationService.Error({ message: this.response.message, title: null });
            break;
        }
      });
    }
  }

  getClientbehaviourDetails() {
    const data = {
       Id: this.ClientId
     }
     this.clientservice.GetClientProfileDetails(data).subscribe(res => {
      this.response = res;
     this.clientbehaviourconcern=this.response;
       if (this.response.status > 0) {
        if(this.response.responseData.clientSafetyBehavioursInfoModel.clientId>0){
          this.textBehaviourofconcern="Update";
        }
        else{
         this.textBehaviourofconcern="Submit"
        }
        this.rFormbehaviourconcern.controls['Ishistoryfalls'].patchValue(this.response.responseData.clientSafetyBehavioursInfoModel.isHistoryFalls==true?'1':'2');
       if(this.response.responseData.clientSafetyBehavioursInfoModel.isHistoryFalls==true){
         this.isShownhistorydescription=true;
         this.rFormbehaviourconcern.controls['historydescription'].patchValue(this.response.responseData.clientSafetyBehavioursInfoModel.historyFallsDetails);
       }
        this.rFormbehaviourconcern.controls['Isabsconding'].patchValue(this.response.responseData.clientSafetyBehavioursInfoModel.isAbsconding==true?'1':'2');
       if(this.response.responseData.clientSafetyBehavioursInfoModel.isAbsconding==true){
         this.isShownabscondingdescription=true;
        this.rFormbehaviourconcern.controls['abscondingdescription'].patchValue(this.response.responseData.clientSafetyBehavioursInfoModel.abscondingHistory);
       }
       this.rFormbehaviourconcern.controls['IsBSP'].patchValue(this.response.responseData.clientSafetyBehavioursInfoModel.isBSP==true?'1':'2');
       if(this.response.responseData.clientSafetyBehavioursInfoModel.isBSP==true){
         this.isShownBSP=true;
         this.rFormbehaviourconcern.controls['BSPdetails'].patchValue(this.response.responseData.clientSafetyBehavioursInfoModel.bspDetails);
       }
       
       this.rFormbehaviourconcern.controls['Isbehaviourconcern'].patchValue(this.response.responseData.clientBehaviourofConcernModel.isBehaviourConcern==true?'1':'2');
        if(this.response.responseData.clientBehaviourofConcernModel.isBehaviourConcern==true){
          this.isShownconcern=true;
          this.rFormbehaviourconcern.controls['behaviourdetails'].patchValue(this.response.responseData.clientBehaviourofConcernModel.behaviourConcernDetails);
        }
        this.rFormbehaviourconcern.controls['Isselfinjury'].patchValue(this.response.responseData.clientBehaviourofConcernModel.isSelfInjury==true?'1':'');
        this.rFormbehaviourconcern.controls['Iskicking'].patchValue(this.response.responseData.clientBehaviourofConcernModel.isKicking==true?'1':'');
        this.rFormbehaviourconcern.controls['Ispinching'].patchValue(this.response.responseData.clientBehaviourofConcernModel.isPinching==true?'1':'');
        this.rFormbehaviourconcern.controls['Isscreaming'].patchValue(this.response.responseData.clientBehaviourofConcernModel.isScreaming==true?'1':'');
        this.rFormbehaviourconcern.controls['Ishitting'].patchValue(this.response.responseData.clientBehaviourofConcernModel.isHitting==true?'1':'');
        this.rFormbehaviourconcern.controls['Isbiting'].patchValue(this.response.responseData.clientBehaviourofConcernModel.isBiting==true?'1':'');
        this.rFormbehaviourconcern.controls['Ispullinghair'].patchValue(this.response.responseData.clientBehaviourofConcernModel.isPullingHair==true?'1':'');
        this.rFormbehaviourconcern.controls['Isthrowing'].patchValue(this.response.responseData.clientBehaviourofConcernModel.isThrowing==true?'1':'');
        this.rFormbehaviourconcern.controls['Isheadbutting'].patchValue(this.response.responseData.clientBehaviourofConcernModel.isHeadButting==true?'1':'');
        this.rFormbehaviourconcern.controls['Isobsessive'].patchValue(this.response.responseData.clientBehaviourofConcernModel.isObsessiveBehaviour==true?'1':'');
        this.rFormbehaviourconcern.controls['Isother'].patchValue(this.response.responseData.clientBehaviourofConcernModel.isOther==true?'1':'');
        this.rFormbehaviourconcern.controls['Isbanging'].patchValue(this.response.responseData.clientBehaviourofConcernModel.isBanging==true?'1':'');
        }
       else {
         //this.notificationService.Error({ message: this.response.message, title: null });
       }
     });
   }


  show(){
    if(this.rFormeatingnutritioninfo.value.Iseatsindependent=='1'){
      this.isShownindependent=true;
    }
    else{
      this.isShownindependent=false;
      this.rFormeatingnutritioninfo.value.eatingdetails.setValue("");
    }
  }
  showmeal(){
    if(this.rFormeatingnutritioninfo.value.Ispreparingmeals=='1'){
      this.isShownmeal=true;
    }
    else{
      this.isShownmeal=false;
      this.rFormeatingnutritioninfo.value.mealdetails.setValue("");
    }
  }
  showutensil(){
    if(this.rFormeatingnutritioninfo.value.Isutensils=='1'){
      this.isShownutensils=true;
    }
    else{
      this.isShownutensils=false;
      this.rFormeatingnutritioninfo.value.utensilsdetails.setValue("");
    }
  }
  showfluids(){
    if(this.rFormeatingnutritioninfo.value.Isfluids=='1'){
      this.isShownfoodffluids=true;
    }
    else{
      this.isShownfoodffluids=false;
      this.rFormeatingnutritioninfo.value.foodfluiddetails.setValue("");
    }
  }
  showdislike(){
    if(this.rFormeatingnutritioninfo.value.Isdietplan=='1'){
      this.isShowndislikes=true;
    }
    else{
      this.isShowndislikes=false;
      this.rFormeatingnutritioninfo.value.likedislikedetails.setValue("");
    }
  }
  showchoking(){
    if(this.rFormeatingnutritioninfo.value.Ischokinghistory=='1'){
      this.isShownhistorydetails=true;
    }
    else{
      this.isShownhistorydetails=false;
      this.rFormeatingnutritioninfo.value.chokingdetails.setValue("");
    }
  }

  showhistoryfalls(){
   
    if(this.rFormbehaviourconcern.value.Ishistoryfalls=='1'){
      this.isShownhistorydescription=true;
    }
    else{
      this.isShownhistorydescription=false;
      this.rFormbehaviourconcern.controls['historydescription'].setValue("");
    }
  }
  showabsconding(){
    if(this.rFormbehaviourconcern.value.Isabsconding=='1'){
      this.isShownabscondingdescription=true;
    }
    else{
      this.isShownabscondingdescription=false;
      this.rFormbehaviourconcern.controls['abscondingdescription'].setValue("");
    }
  }
  showconcern(){
    if(this.rFormbehaviourconcern.value.Isbehaviourconcern=='1'){
      this.isShownconcern=true;
    }
    else{
      this.isShownconcern=false;
      this.rFormbehaviourconcern.controls['behaviourdetails'].setValue("");
    }
  }
  showhistoryBSP(){
    if(this.rFormbehaviourconcern.value.IsBSP=='1'){
      this.isShownBSP=true;
    }
    else{
      this.isShownBSP=false;
      this.rFormbehaviourconcern.controls['BSPdetails'].setValue("");
    }
  }
  ShowclientProfile() {
    
       if (this.clientprofileinfo.status > 0) {
         this.socialconnectioninfo=(this.clientprofileinfo.responseData.clientSocialConnectionsModel.socialConnection);
         this.cultureneedinfo=(this.clientprofileinfo.responseData.clientCultureNeedModel.cultureNeed);
         this.interestinfo=(this.clientprofileinfo.responseData.clientPersonalPreferencesModel.interest);
         this.clientimportanceinfo=(this.clientprofileinfo.responseData.clientPersonalPreferencesModel.clientImportance);
         this.charecteristicsinfo=(this.clientprofileinfo.responseData.clientPersonalPreferencesModel.charecteristics);
         this.anxitiesinfo=(this.clientprofileinfo.responseData.clientPersonalPreferencesModel.fearsandAnxities);
         this.livingarrangementinfo=(this.clientprofileinfo.responseData.clientLivingArrangementModel.livingArrangement);
         this.otherinformationinfo=(this.clientprofileinfo.responseData.clientOtherInformtionModel.otherInformation);
         this.Iseatsindependent=(this.response.responseData.clientEatingNutritionModel.isEatsIndependently==true?'Yes':'No');
         this.eatingdetails=(this.response.responseData.clientEatingNutritionModel.eatingNutritionDetails);
         this.Ispreparingmeals=(this.response.responseData.clientEatingNutritionModel.isPreparingMeals==true?'Yes':'No');
         this.mealdetails=(this.response.responseData.clientEatingNutritionModel.mealsDetails);
         this.Isutensils=(this.response.responseData.clientEatingNutritionModel.isUsesUtensils==true?'Yes':'No');
         this.utensilsdetails=(this.response.responseData.clientEatingNutritionModel.utensilsDetails);
         this.Isfluids=(this.response.responseData.clientEatingNutritionModel.isFluids==true?'Yes':'No');
         this.foodfluiddetails=(this.response.responseData.clientEatingNutritionModel.fluidsDetails);
         this.Ismodifiedfood=(this.response.responseData.clientEatingNutritionModel.isModifiedFood==true?'Yes':'No');
         this.IsPEG=(this.response.responseData.clientEatingNutritionModel.isPEG==true?'Yes':'No');
         this.Isswallowing=(this.response.responseData.clientEatingNutritionModel.isSwallowingImpairment==true?'Yes':'No');
         this.Isdietplan=(this.response.responseData.clientEatingNutritionModel.isDietPlan==true?'Yes':'No');
         this.likedislikedetails=(this.response.responseData.clientEatingNutritionModel.allergiesDetails);
         this.Ischokinghistory=(this.response.responseData.clientEatingNutritionModel.hasChoking==true?'Yes':'No');
         this.chokingdetails=(this.response.responseData.clientEatingNutritionModel.chokingDetails);
         this.foodpreferencedetails=(this.response.responseData.clientEatingNutritionModel.foodPreferences);
         this.Ishistoryfalls=(this.response.responseData.clientSafetyBehavioursInfoModel.isHistoryFalls==true?'Yes':'No');
        this.historydescription=(this.response.responseData.clientSafetyBehavioursInfoModel.historyFallsDetails);
       this.Isabsconding=(this.response.responseData.clientSafetyBehavioursInfoModel.isAbsconding==true?'Yes':'No');
this.abscondingdescription=(this.response.responseData.clientSafetyBehavioursInfoModel.abscondingHistory);
this.IsBSP=(this.response.responseData.clientSafetyBehavioursInfoModel.isBSP==true?'Yes':'No');
this.BSPdetails=(this.response.responseData.clientSafetyBehavioursInfoModel.bspDetails);
this.Isbehaviourconcern=(this.response.responseData.clientBehaviourofConcernModel.isBehaviourConcern==true?'Yes':'No');
this.behaviourdetails=(this.response.responseData.clientBehaviourofConcernModel.behaviourConcernDetails);
this.Isselfinjury=(this.response.responseData.clientBehaviourofConcernModel.isSelfInjury==true?'Yes':'No');
this.Iskicking=(this.response.responseData.clientBehaviourofConcernModel.isKicking==true?'Yes':'No');
this.Ispinching=(this.response.responseData.clientBehaviourofConcernModel.isPinching==true?'Yes':'No');
this.Isscreaming=(this.response.responseData.clientBehaviourofConcernModel.isScreaming==true?'Yes':'No');
this.Ishitting=(this.response.responseData.clientBehaviourofConcernModel.isHitting==true?'Yes':'No');
this.Isbiting=(this.response.responseData.clientBehaviourofConcernModel.isBiting==true?'Yes':'No');
this.Ispullinghair=(this.response.responseData.clientBehaviourofConcernModel.isPullingHair==true?'Yes':'No');
this.Isthrowing=(this.response.responseData.clientBehaviourofConcernModel.isThrowing==true?'Yes':'No');
this.Isheadbutting=(this.response.responseData.clientBehaviourofConcernModel.isHeadButting==true?'Yes':'No');
this.Isobsessive=(this.response.responseData.clientBehaviourofConcernModel.isObsessiveBehaviour==true?'Yes':'No');
this.Isother=(this.response.responseData.clientBehaviourofConcernModel.isOther==true?'Yes':'No');
this.Isbanging=(this.response.responseData.clientBehaviourofConcernModel.isBanging==true?'Yes':'No');
        }
       else {
         //this.notificationService.Error({ message: this.response.message, title: null });
       }
    
   }
  
}
