import { NgModule } from '@angular/core';
import { EmployeeDetailComponent } from './employee-detail.component';
import { MatTableModule } from '@angular/material/table';
import { LhsComponentModule } from 'projects/lhs-component/src/projects';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSortModule } from '@angular/material/sort';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatRadioModule } from '@angular/material/radio';
import { MatOptionModule, MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import {CommonModule } from '@angular/common';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { LhsServiceModule } from 'projects/lhs-service/src/projects';
import { LhsDirectivesModule } from 'projects/lhs-directives/src/lib/lhs-directives.module';
import { EmployeeDetailRoutingModule } from './employee-detail-routing.module';
import { EmployeedetailsComponent } from './component/employeedetails/employeedetails.component';
import { EmpTodayShiftComponent } from './component/emp-today-shift/emp-today-shift.component';
import { EmpPrimaryDetailComponent } from './component/emp-primary-detail/emp-primary-detail.component';
import { EmpNextKinComponent } from './component/emp-next-kin/emp-next-kin.component';
import { EmpAwarddetailComponent } from './component/emp-awarddetail/emp-awarddetail.component';
import { EmpDriverlicenseComponent } from './component/emp-driverlicense/emp-driverlicense.component';
import { EmpJobProfileComponent } from './component/emp-job-profile/emp-job-profile.component';
import { EmpLoginPasswordComponent } from './component/emp-login-password/emp-login-password.component';
import { EmpQualificationComponent } from './component/emp-qualification/emp-qualification.component';
import { EmpExperienceComponent } from './component/emp-experience/emp-experience.component';
import { EmpProffessionaldevelopmentComponent } from './component/emp-proffessionaldevelopment/emp-proffessionaldevelopment.component';
import { EmpPayrateComponent } from './component/emp-payrate/emp-payrate.component';
import { SubmenuPageComponent } from './component/submenu-page/submenu-page.component';
import { EmpCommunicationComponent } from './component/emp-communication/emp-communication.component';
import { EmpAccidentIncidentComponent } from './component/emp-accident-incident/emp-accident-incident.component';
import { EmpLeaveComponent } from './component/emp-leave/emp-leave.component';
import { EmpStaffWarningComponent } from './component/emp-staff-warning/emp-staff-warning.component';
import { EmpCompliancesComponent } from './component/emp-compliances/emp-compliances.component';
import { EmpDocumentchecklistComponent } from './component/emp-documentchecklist/emp-documentchecklist.component';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';


@NgModule({
  declarations: [EmployeeDetailComponent, EmployeedetailsComponent, EmpTodayShiftComponent, EmpPrimaryDetailComponent, EmpNextKinComponent, EmpAwarddetailComponent, EmpDriverlicenseComponent, EmpJobProfileComponent, EmpLoginPasswordComponent, EmpQualificationComponent, EmpExperienceComponent, EmpProffessionaldevelopmentComponent, EmpPayrateComponent, SubmenuPageComponent, EmpCommunicationComponent, EmpAccidentIncidentComponent, EmpLeaveComponent, EmpStaffWarningComponent, EmpCompliancesComponent, EmpDocumentchecklistComponent],
  imports: [EmployeeDetailRoutingModule,MatTableModule, LhsComponentModule, FormsModule, ReactiveFormsModule, MatPaginatorModule,
    MatFormFieldModule, MatSortModule, MatInputModule, MatSelectModule, MatRadioModule, MatNativeDateModule, MatOptionModule,
    MatDatepickerModule, CommonModule, MatCheckboxModule, LhsServiceModule,LhsDirectivesModule
    , NgxMatSelectSearchModule
   
  ],
  exports: [EmployeeDetailComponent]
})
export class EmployeeDetailModule { }
