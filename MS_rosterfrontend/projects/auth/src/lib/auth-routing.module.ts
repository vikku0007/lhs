import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { NgModule } from '@angular/core';
import { EmailConfirmationComponent } from './components/email-confirmation/email-confirmation.component';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';
import { ForgotPasswordsComponent } from './components/forgot-passwords/forgot-passwords.component';



const routes: Routes = [
    { path: 'login', component: LoginComponent },
    { path: 'forgotpassword', component: ForgotPasswordsComponent },
    { path: 'confirmemail', component: EmailConfirmationComponent },
    { path: 'resetpassword', component: ResetPasswordComponent },
];

@NgModule({
    declarations: [],
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AuthRoutingModule { }