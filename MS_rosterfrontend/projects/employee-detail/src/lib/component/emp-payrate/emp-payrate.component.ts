import { Component, OnInit, Input, ViewChild, ElementRef, OnChanges, SimpleChanges } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { ActivatedRoute } from '@angular/router';
import { NotificationService } from 'projects/core/src/lib/services/notification-service/notification.service';
import { CommonService } from 'projects/lhs-service/src/projects';
import { EmployeePayRates } from '../../view-models/employee-payrates';
import { EmpDetailService } from '../../services/emp-detail.service';

@Component({
  selector: 'lib-emp-payrate',
  templateUrl: './emp-payrate.component.html',
  styleUrls: ['./emp-payrate.component.scss']
})
export class EmpPayrateComponent implements OnInit, OnChanges {
  @Input() empPayRates: EmployeePayRates;
  rForm: FormGroup;
  response: ResponseModel = {};
  levelList: any;
  globalPayRate: any = {};
  @ViewChild('btnPayRatesCancel') cancel: ElementRef;
  getErrorMessage: any;
  constructor(private fb: FormBuilder, private route: ActivatedRoute, private commonService: CommonService,private empService: EmpDetailService, private notificationService: NotificationService) { }

  ngOnChanges(changes: SimpleChanges): void {
    if (this.empPayRates !== undefined) {
      this.route.paramMap.subscribe((params: any) => {
        this.empPayRates.employeeId = Number(params.params.id);
      });
     
    }
  }

  ngOnInit(): void {
    this.getlevelList();
    this.createForm();
    this.getEmployeelevel();
  }

  createForm() {
    this.rForm = this.fb.group({
      level: [this.empPayRates.level, Validators.compose([Validators.required, 
      Validators.minLength(1),
      ])],
      monToFri6To12AM: [this.empPayRates.monToFri6To12AM, Validators.compose([Validators.required,
      Validators.maxLength(8),
      ])],
      sat6To12AM: [this.empPayRates.sat6To12AM, Validators.compose([Validators.required,
      Validators.maxLength(8),
      ])],
      sun6To12AM: [this.empPayRates.sun6To12AM, Validators.compose([Validators.required,
      Validators.maxLength(8),
      ])],
      holiday6To12AM: [this.empPayRates.holiday6To12AM, Validators.compose([Validators.required,
      Validators.maxLength(8),
      ])],
      monToFri12To6AM: [this.empPayRates.monToFri12To6AM, Validators.compose([Validators.required,
      Validators.maxLength(8),
      ])],
      sat12To6AM: [this.empPayRates.sat12To6AM, Validators.compose([Validators.required,
      Validators.maxLength(8),
      ])],
      sun12To6AM: [this.empPayRates.sun12To6AM, Validators.compose([Validators.required,
      Validators.maxLength(8),
      ])],
      holiday12To6AM: [this.empPayRates.holiday12To6AM, Validators.compose([Validators.required,
      Validators.maxLength(8),
      ])],
      activenights: [this.empPayRates.activeNightsAndSleep, Validators.compose([Validators.required,
        Validators.maxLength(8),
        ])],
        housecleaning: [this.empPayRates.houseCleaning, Validators.compose([Validators.required,
          Validators.maxLength(8),
          ])],
          transportpetrol: [this.empPayRates.transportPetrol, Validators.compose([Validators.required,
            Validators.maxLength(8),
            ])],
    });
  }
  getEmployeelevel(){
   
    this.commonService.getEmployeelevel(this.empPayRates.employeeId).subscribe((res=>{
      if(res){
        this.response = res;
        this.rForm.controls['level'].patchValue(this.response.responseData);
        this.getGlobalPayRate(this.response.responseData);
      }else{

      }
    }));
  }
  get level() {
    return this.rForm.get('level');
  }
  get monToFri6To12AM() {
    return this.rForm.get('monToFri6To12AM');
  }
  get sat6To12AM() {
    return this.rForm.get('sat6To12AM');
  }
  get sun6To12AM() {
    return this.rForm.get('sun6To12AM');
  }

  get holiday6To12AM() {
    return this.rForm.get('holiday6To12AM');
  }

  get monToFri12To6AM() {
    return this.rForm.get('monToFri12To6AM');
  }

  get sat12To6AM() {
    return this.rForm.get('sat12To6AM');
  }
  get sun12To6AM() {
    return this.rForm.get('sun12To6AM');
  }
  get holiday12To6AM() {
    return this.rForm.get('holiday12To6AM');
  }
  get activenights() {
    return this.rForm.get('activenights');
  }
  get housecleaning() {
    return this.rForm.get('housecleaning');
  }
  get transportpetrol() {
    return this.rForm.get('transportpetrol');
  }

  UpdateEmpPayrate() {
    if (this.rForm.valid) {
      const data = {
        EmployeeId: this.empPayRates.employeeId,
        // tslint:disable-next-line: radix
        Level: parseInt(this.level.value),
        MonToFri6To12AM: parseFloat(this.monToFri6To12AM.value),
        Sat6To12AM: parseFloat(this.sat6To12AM.value),
        Sun6To12AM: parseFloat(this.sun6To12AM.value),
        Holiday6To12AM: parseFloat(this.holiday6To12AM.value),
        MonToFri12To6AM: parseFloat(this.monToFri12To6AM.value),
        Sat12To6AM: parseFloat(this.sat12To6AM.value),
        Sun12To6AM: parseFloat(this.sun12To6AM.value),
        Holiday12To6AM: parseFloat(this.holiday12To6AM.value),
        ActivenightsandSleep:parseFloat(this.activenights.value),
        HouseCleaning:parseFloat(this.housecleaning.value),
        TransportPetrol:parseFloat(this.transportpetrol.value),
      };
      this.empService.updatePayRate(data).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
            this.empPayRates = this.response.responseData;
            this.cancel.nativeElement.click();
            this.notificationService.Success({ message: 'PayRates updated successfully', title: null });
            break;

          default:
            break;
        }
      });
    }
  }
  selected() {
  
   if(Number(this.level.value))
   {
    this.getGlobalPayRate(Number(this.level.value));
   }
   
   
}
  getGlobalPayRate(level: number){
    this.commonService.getGlobalPayrate(level).subscribe((res=>{
      if(res){
        this.response = res;
        this.globalPayRate=this.response.responseData || {};
        this.rForm.get('monToFri6To12AM').setValue(this.globalPayRate.monToFri6To12AM);
        this.rForm.get('sat6To12AM').setValue(this.globalPayRate.sat6To12AM);
          this.rForm.get('sun6To12AM').setValue(this.globalPayRate.sun6To12AM);
            this.rForm.get('holiday6To12AM').setValue(this.globalPayRate.holiday6To12AM);
              this.rForm.get('monToFri12To6AM').setValue(this.globalPayRate.monToFri12To6AM);
                this.rForm.get('sat12To6AM').setValue(this.globalPayRate.sat12To6AM);
                  this.rForm.get('sun12To6AM').setValue(this.globalPayRate.sun12To6AM);
                    this.rForm.get('holiday12To6AM').setValue(this.globalPayRate.holiday12To6AM);
                    this.rForm.get('activenights').setValue(this.globalPayRate.activeNightsAndSleep);
                    this.rForm.get('housecleaning').setValue(this.globalPayRate.houseCleaning);
                    this.rForm.get('transportpetrol').setValue(this.globalPayRate.transportPetrol);
      }else{

      }
    }));
  }
  getlevelList(){
    this.commonService.getLevel().subscribe((res=>{
      if(res){
        this.response = res;
        this.levelList=this.response.responseData || [];
       
      }else{

      }
    }));
  }
}
