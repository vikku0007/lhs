import { Component, OnInit, Input, ViewChild, ElementRef } from '@angular/core';
import { EmployeeMiscInfo } from '../../viewmodel/employee-miscinfo';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { EmpServiceService } from '../../services/emp-service.service';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { ActivatedRoute } from '@angular/router';
import { NotificationService } from 'projects/core/src/lib/services/notification-service/notification.service';
import { CommonService } from 'projects/lhs-service/src/projects';


@Component({
  selector: 'lib-employee-miscinfo',
  templateUrl: './employee-miscinfo.component.html',
  styleUrls: ['./employee-miscinfo.component.scss']
})
export class EmployeeMiscInfoComponent implements OnInit {
  @Input() empMiscInfo: EmployeeMiscInfo;
  rForm: FormGroup;
  response: ResponseModel = {};
  @ViewChild('btnMiskInfoCancel') cancel: ElementRef;
  getErrorMessage: any;
  ethnicityList: any;
  religionList: any;
  constructor(private route: ActivatedRoute, private fb: FormBuilder,private commonservice:CommonService, private empService: EmpServiceService, private notificationService: NotificationService) { }

  ngOnInit(): void {
    if (this.empMiscInfo.employeeId == 0) {
      this.route.queryParams.subscribe(params => {
        this.empMiscInfo.employeeId = parseInt(params['Id']);
      });
    }
    this.createForm();
    this.getEthnicityType();
    this.getReligion();
  }

  createForm() {
    this.rForm = this.fb.group({
      smoker: [this.empMiscInfo.smoker === true ? '1' : '0' , Validators.required],
      ethnicity: [this.empMiscInfo.ethnicity, Validators.required],
      height: [this.empMiscInfo.height, Validators.compose([Validators.required,
        Validators.max(10),
        ])],
      religion: [this.empMiscInfo.religion, Validators.required],
      weight: [this.empMiscInfo.weight, Validators.compose([Validators.required,
        Validators.max(200),
        ])],
    });
  }

 

  get smoker() {
    return this.rForm.get('smoker');
  }

  get ethnicity() {
    return this.rForm.get('ethnicity');
  }

  get height() {
    return this.rForm.get('height');
  }
  get religion() {
    return this.rForm.get('religion');
  }
  get weight() {
    return this.rForm.get('weight');
  }

  UpdateMiscInfo() {
    if (this.rForm.valid) {
     const data = {
        employeeId: this.empMiscInfo.employeeId,
        Weight: parseFloat(this.weight.value),
        Height: parseFloat(this.height.value),
        Ethnicity: this.ethnicity.value,
        Religion: this.religion.value,
        Smoker: this.smoker.value === '1' ? true : false,
      
      }
      this.empService.updateEmployeeMiscInfo(data).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
            this.empMiscInfo = this.response.responseData;
            this.cancel.nativeElement.click();
            this.notificationService.Success({ message: 'Employee Misc Info updated successfully', title: null });
           
            break;

          default:
            break;
        }
      });
    }
  }
  getEthnicityType() {
    this.commonservice.getEthnicityType().subscribe((res => {
      if (res) {
        this.response = res;
        this.ethnicityList = this.response.responseData;
      }
    }));
  }
  getReligion() {
    this.commonservice.getReligion().subscribe((res => {
      if (res) {
        this.response = res;
        this.religionList = this.response.responseData;
      }
    }));
  }
}
