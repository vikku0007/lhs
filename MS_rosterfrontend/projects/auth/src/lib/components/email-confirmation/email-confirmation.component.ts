import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AccountService } from '../../services/account.service';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { NotificationService } from 'projects/core/src/projects';

@Component({
  selector: 'lib-email-confirmation',
  templateUrl: './email-confirmation.component.html',
  styleUrls: ['./email-confirmation.component.scss']
})
export class EmailConfirmationComponent implements OnInit {
  rForm: FormGroup;
  confirmemail !: string;
  confirmEmailToken: any;
  confirmApplicationId:any;
  isSuccessmsg :boolean = true ;
  isErrormsg: boolean = false ;
  responseModel: ResponseModel = {};
  isSuccesstxt: any;
  isErrortxt: string;
  isconfirmedmsg: boolean=false;
  isconfirmtxt: string;
  constructor(private fb: FormBuilder, private router: Router,private authService: AccountService,  private route: ActivatedRoute,private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.confirmemail = params['eid'];
      this.confirmEmailToken = params['tkn'];
     if (this.confirmemail && this.confirmEmailToken) {
        this.fn_confirmEmail();
      }
    });
  }
  fn_confirmEmail() {
     const data = {
      Email: this.confirmemail,
      Token:decodeURIComponent(this.confirmEmailToken)  
  };
  this.authService.emailConfirmation(data).subscribe(res => {
    this.responseModel = res;
    
    
    if (this.responseModel.status==2)
      {
         this.isSuccesstxt="Your email has been confirmed successfully."
        setTimeout(() => {
        this.router.navigate(['auth/login']);
      }, 10000);
        } 
        else if (this.responseModel.status==2) {
         this.isconfirmtxt="Email already confirmed.";
         setTimeout(() => {
          this.router.navigate(['auth/login']);
        }, 10000);
        } 
        else{
          this.isErrortxt="An error occured, Please try again";
        
        }
   
   
  
  });

  }
}
