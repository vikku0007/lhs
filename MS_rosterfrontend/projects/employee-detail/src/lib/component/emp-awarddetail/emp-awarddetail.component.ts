import { Component, OnInit, Input, ViewChild, ElementRef } from '@angular/core';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { NotificationService } from 'projects/core/src/lib/services/notification-service/notification.service';
import { ActivatedRoute } from '@angular/router';
import { CommonService } from 'projects/lhs-service/src/lib/service/common/common.service';
import { EmployeeAwardInfo } from '../../view-models/employee-award-info';
import { EmpDetailService } from '../../services/emp-detail.service';


@Component({
  selector: 'lib-emp-awarddetail',
  templateUrl: './emp-awarddetail.component.html',
  styleUrls: ['./emp-awarddetail.component.scss']
})
export class EmpAwarddetailComponent implements OnInit {
  @Input() empAward: EmployeeAwardInfo;
  rForm: FormGroup;
  awardGroupList:any;
  response: ResponseModel = {};
  @ViewChild('btnAwardCancel') cancel: ElementRef;
  @ViewChild('formDirective ') formDirective: NgForm;
  getErrorMessage: any;

  constructor(private route: ActivatedRoute, private commonService: CommonService,private fb: FormBuilder, private empService: EmpDetailService, private notificationService: NotificationService) { }

  ngOnInit(): void {
    if (this.empAward.employeeId == 0) {
      this.route.paramMap.subscribe((params: any) => {
        this.empAward.employeeId = Number(params.params.id);
      });
      
    }
    this.createForm();
    this.getAwardGroup();
  }

  createForm() {
    this.rForm = this.fb.group({
      allowances: [this.empAward.allowances != null ? this.empAward.allowances : null, Validators.compose([Validators.required,
      Validators.max(5000),
      ])],
      dailyHours: [this.empAward.dailyhours != null ? this.empAward.dailyhours : null, Validators.compose([Validators.required,
      Validators.max(12),
      ])],
      weeklyHours: [this.empAward.weeklyhours != null ? this.empAward.weeklyhours : null, Validators.compose([Validators.required,
      Validators.max(60),
      ])],
      awardGroup :[this.empAward.awardGroup==0?null:this.empAward.awardGroup, Validators.required],
      
    });
  }

  get allowances() {
    return this.rForm.get('allowances');
  }

  get dailyHours() {
    return this.rForm.get('dailyHours');
  }
  get awardGroup() {
    return this.rForm.get('awardGroup');
  }
  get weeklyHours() {
    return this.rForm.get('weeklyHours');
  }

  

  UpdateAwardDetails() {
    if (this.rForm.valid) {
      const data = {
        EmployeeId: this.empAward.employeeId,
        Allowances: this.rForm.get('allowances').value,
        Dailyhours: this.rForm.get('dailyHours').value,
        Weeklyhours: this.rForm.get('weeklyHours').value,
        awardGroup : Number(this.rForm.get('awardGroup').value),
      }
      this.empService.updateEmployeeDetails(data).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
            this.empAward = this.response.responseData;   
            this.empAward.awardGroupName =  this.awardGroupList.filter(x=>x.id === Number(this.rForm.get('awardGroup').value))[0].codeDescription;         
            this.cancel.nativeElement.click();            
            this.notificationService.Success({ message: 'Awards Updated successfully', title: null });
            break;

          default:
            break;
        }
      });
    }
  }

  cancelModal() {
    // this.rForm.reset();
    // this.formDirective.resetForm();
  }
  getAwardGroup(){
    this.commonService.getAwardGroup().subscribe((res=>{
      if(res){
        this.response = res;
        this.awardGroupList=this.response.responseData || [];
       
      }else{

      }
    }));
  }
}
