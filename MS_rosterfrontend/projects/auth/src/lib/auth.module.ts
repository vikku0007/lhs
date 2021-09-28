import { NgModule } from '@angular/core';
import { AuthComponent } from './auth.component';
import { LoginComponent } from './components/login/login.component';
import { AuthRoutingModule } from './auth-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CoreModule, InterceptorService } from 'projects/core/src/projects';
import { AccountService } from './services/account.service';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule } from '@angular/material/sort';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatRadioModule } from '@angular/material/radio';
import { MatNativeDateModule, MatOptionModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { LhsServiceModule } from 'projects/lhs-service/src/projects';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { EmailConfirmationComponent } from './components/email-confirmation/email-confirmation.component';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';
import { ForgotPasswordsComponent } from './components/forgot-passwords/forgot-passwords.component';


@NgModule({
  declarations: [AuthComponent, LoginComponent,EmailConfirmationComponent, ResetPasswordComponent, ForgotPasswordsComponent],
  imports: [FormsModule, ReactiveFormsModule, AuthRoutingModule, CoreModule, MatTableModule,
    MatSortModule,
    MatInputModule,
    MatSelectModule,
    MatRadioModule,
    MatNativeDateModule,
    MatOptionModule,
    MatDatepickerModule,
    MatFormFieldModule,
    MatProgressSpinnerModule,
    LhsServiceModule,
    CoreModule

  ],
  exports: [AuthComponent],
  providers: [AccountService, { provide: HTTP_INTERCEPTORS, useClass: InterceptorService }]
})
export class AuthModule { }

