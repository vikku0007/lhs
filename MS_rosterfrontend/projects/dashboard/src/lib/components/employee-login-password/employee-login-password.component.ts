import { Component, OnInit, Input, ElementRef, ViewChild } from '@angular/core';
import { EmployeeDetails } from '../../viewmodel/employee-details';
import { ActivatedRoute } from '@angular/router';
import { EmpServiceService } from '../../services/emp-service.service';
import { ResponseModel } from 'projects/viewmodels/response-model';
import {LoaderService } from 'projects/lhs-service/src/projects';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { NotificationService } from 'projects/core/src/projects';
import { EmployeeJobProfile } from '../../viewmodel/employee-jobprofile';

@Component({
  selector: 'lib-employee-login-password',
  templateUrl: './employee-login-password.component.html',
  styleUrls: ['./employee-login-password.component.scss']
})
export class EmployeeLoginPasswordComponent implements OnInit {
  @Input() empPrimaryInfo: EmployeeDetails;
  @ViewChild('btnonboadingCancel') cancel: ElementRef;
  responseModel: ResponseModel = {};
  txtEmail: string;
  employeeid = 0;
  isShownpassword: boolean = false ;
  isHidepassword: boolean = false ;
  rForm: FormGroup;
  responseinfo: any;
  @Input() empJobProfile: EmployeeJobProfile;
  @ViewChild('formDirective') private formDirective: NgForm;
  constructor(private route: ActivatedRoute, private empService: EmpServiceService,
    private fb: FormBuilder,private notificationService: NotificationService,private loaderService: LoaderService) { }

  ngOnInit() {
     this.route.queryParams.subscribe(params => {
     this.employeeid = parseInt(params.Id);
    });
    this.getEmployeeDetails();
    this.createForm();
  }
  createForm() {
    this.rForm = this.fb.group({
      Password: [null, [Validators.minLength(7),Validators.required]],
    })
  }
  getEmployeeDetails() {
    this.loaderService.start();
    const data = {
      Id: Number(this.employeeid)
       }
     this.empService.getEmployeePassword(data).subscribe(res => {
     this.responseModel = res;
      if (this.responseModel.status > 0) {
        this.loaderService.stop();
       this.txtEmail=this.responseModel.responseData[0].emailId;
      if(this.responseModel.responseData[0].passwordExist>0&&this.responseModel.responseData[0].passwordExist!=null){
        this.isShownpassword=true;
        this.isHidepassword=false;
      }
      else{
        this.isShownpassword=false;
        this.isHidepassword=true;
        
      }
       }
       else {
       
       }
     });
   }
   UpdatePasswordDetail(){
    if (this.rForm.valid) {
        const data = {
          EmployeeId: this.employeeid,
          Password:this.rForm.value.Password
         }
        
        this.empService.UpdateEmployeePassword(data).subscribe(res => {
          this.responseModel = res;
          switch (this.responseModel.status) {
            case 1:
              this.responseinfo = this.responseModel.responseData;
              this.cancel.nativeElement.click();
              this.rForm.reset();
              this.formDirective.resetForm();
              this.getEmployeeDetails();
              this.notificationService.Success({ message: "Password Updated Successfully", title: null });
              break;
  
            default:
              this.notificationService.Error({ message: this.responseModel.message, title: null });
              break;
          }
        });
      }
   
   }
   Reset(){
    this.rForm.reset();
    this.formDirective.resetForm();
   }
}
