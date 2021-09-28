import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AccountService } from '../../services/account.service';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { NotificationService } from 'projects/core/src/projects';

@Component({
  selector: 'lib-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {
  rForm: FormGroup;
  responseModel: ResponseModel = {};
  @ViewChild('formDirective') private formDirective: NgForm;
   constructor(private fb: FormBuilder, private router: Router,
    private authService: AccountService,  private route: ActivatedRoute,
    private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.createForm();
  }

  createForm() {
    this.rForm = this.fb.group({
      Password: [null, Validators.compose([Validators.required, Validators.minLength(6)])],
      ConfirmPassword: [null, Validators.compose([Validators.required, Validators.minLength(6)])]
     
    });
  }
  get Password() {
    return this.rForm.get('Password');
  }
  get ConfirmPassword() {
    return this.rForm.get('ConfirmPassword');
  }

  ResetPassword() {
    if (this.rForm.valid) {
      if(this.rForm.value.ConfirmPassword!=this.rForm.value.Password){
        this.notificationService.Warning({ message: 'Password and Confirm Password are not match', title: null });
      }
      const data = {
        NewPassword: this.rForm.value.ConfirmPassword,
        email: this.route.snapshot.queryParamMap.get("eid"),
      Token: this.route.snapshot.queryParamMap.get("tkn")
      };
      this.authService.resetPassword(data).subscribe(res => {
        this.responseModel = res;
        switch (this.responseModel.status) {
          case 1:
            this.notificationService.Success({ message: 'Password Reset Successfully', title: null });
            this.router.navigate(['auth/login']);
            break;
            default:
              this.notificationService.Error({ message: this.responseModel.message, title: null });
              break;
        }
      });

    }
  }

}
