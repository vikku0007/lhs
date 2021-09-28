import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie';
import { AccountService } from '../../services/account.service';
import { MembershipService } from 'projects/core/src/lib/services/membership-service/membership.service';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { NotificationService } from 'projects/core/src/projects';


@Component({
  selector: 'lib-forgot-passwords',
  templateUrl: './forgot-passwords.component.html',
  styleUrls: ['./forgot-passwords.component.scss']
})
export class ForgotPasswordsComponent implements OnInit {
  rForm: FormGroup;
  responseModel: ResponseModel = {};
  @ViewChild('formDirective') private formDirective: NgForm;
   constructor(private fb: FormBuilder, private router: Router,
    private authService: AccountService, private membershipService: MembershipService,
    private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.createForm();
  }

  createForm() {
    this.rForm = this.fb.group({
      EmailId: [null, Validators.compose([Validators.required, Validators.pattern(/^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/)])]
    });
  }

  get EmailId() {
    return this.rForm.get('EmailId');
  }

  ForgotPassword() {
    if (this.rForm.valid) {
      const data = {
        ForgetPasswordEmailId: this.rForm.value.EmailId
      };
      this.authService.forgotPassword(data).subscribe(res => {
        this.responseModel = res;
        switch (this.responseModel.status) {
          case 1:
            this.notificationService.Success({ message: 'Reset Password Link Send on your Email', title: null });
            this.rForm.reset();
            this.formDirective.resetForm();
            break;
            default:
              this.notificationService.Error({ message: this.responseModel.message, title: null });
              break;
        }
      });

    }
  }

}
