import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie';
import { AccountService } from '../../services/account.service';
import { MembershipService } from 'projects/core/src/lib/services/membership-service/membership.service';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { NotificationService } from 'projects/core/src/projects';


@Component({
  selector: 'lib-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  rForm: FormGroup;
  responseModel: ResponseModel = {};
  getErrorMessage: 'Please Enter Value';

  constructor(private fb: FormBuilder, private router: Router, private cookie: CookieService,
    private authService: AccountService, private membershipService: MembershipService,
    private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.createForm();
  }

  createForm() {
    this.rForm = this.fb.group({
      // email: [null, Validators.compose([Validators.required, Validators.pattern(/^[A-Za-z0-9]+$/), Validators.maxLength(15)])],
      email: [null, Validators.compose([Validators.required, Validators.maxLength(50)])],
      password: [null, Validators.compose([Validators.required, Validators.minLength(6)])]
    });
  }

  get email() {
    return this.rForm.get('email');
  }

  get password() {
    return this.rForm.get('password');
  }

  Login() {
    if (this.rForm.valid) {
      const data = {
        userName: this.rForm.value.email,
        password: this.rForm.value.password
      };
      this.authService.login(data).subscribe(res => {
        debugger;
        this.responseModel = res;
        switch (this.responseModel.statusCode) {
          case 200:
            this.notificationService.Success({ message: 'Login successful', title: null });
            this.membershipService.setCookies(this.responseModel.responseData);
            localStorage.setItem("access_token", this.responseModel.responseData.token);
            if (this.responseModel.responseData.userRole == 1 ||
              this.responseModel.responseData.userRole == 10) {
              this.router.navigate(['admin/dashboard']);
            }
            else if (this.responseModel.responseData.userRole == 3 ||
              this.responseModel.responseData.userRole == 5 ||
              this.responseModel.responseData.userRole == 6 ||
              this.responseModel.responseData.userRole == 7 ||
              this.responseModel.responseData.userRole == 8 ||
              this.responseModel.responseData.userRole == 9 ||
              // this.responseModel.responseData.userRole == 10 ||
              this.responseModel.responseData.userRole == 11 ||
              this.responseModel.responseData.userRole == 12 ||
              this.responseModel.responseData.userRole == 13 ||
              this.responseModel.responseData.userRole == 14 ||
              this.responseModel.responseData.userRole == 15 ||
              this.responseModel.responseData.userRole == 16 ||
              this.responseModel.responseData.userRole == 17 ) {
              this.router.navigate(['employee/dashboard', this.responseModel.responseData.employeeId]);
            }
            break;
          case 404:
            this.notificationService.Warning({ message: 'Invalid user details', title: null });
            break;
          default:
            this.notificationService.Warning({ message: 'Invalid user details', title: null });
            break;
        }
      });

    }
  }

}
