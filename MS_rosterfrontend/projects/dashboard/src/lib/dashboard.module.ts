import { NgModule } from '@angular/core';
import { DashboardComponent } from './dashboard.component';
import { EmployeeComponent } from './components/employee/employee.component';
import { EmployeeDetailComponent } from './components/employee-detail/employee-detail.component';
import { DashboardRoutingModule } from './dashboard-routing.module';
import { MatTableModule } from '@angular/material/table';
import { LhsComponentModule } from 'projects/lhs-component/src/projects';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatPaginatorModule } from '@angular/material/paginator';
import { EmployeeAwardsComponent } from './components/employee-awards/employee-awards.component';
import { EmployeeKinInfoComponent } from './components/employee-kininfo/employee-kininfo.component';
import { EmployeeJobProfileComponent } from './components/employee-jobprofile/employee-jobprofile.component';
import { EmployeeDrivinglicenseComponent } from './components/employee-drivinglicense/employee-drivinglicense.component';
import { EmployeePayRateseComponent } from './components/employee-payrates/employee-payrates.component';
import { EmployeePrimaryInfoComponent } from './components/employee-primaryinfo/employee-primaryinfo.component';
import { EmployeeEducationComponent } from './components/employee-education/employee-education.component';
import { EmployeeMiscInfoComponent } from './components/employee-miscinfo/employee-miscinfo.component';
import { EmployeeExperienceComponent } from './components/employee-experience/employee-experience.component';
import { CommunicationListComponent } from './components/communication-list/communication-list.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSortModule } from '@angular/material/sort';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatRadioModule } from '@angular/material/radio';
import { MatOptionModule, MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { ComplianceListComponent } from './components/compliance-list/compliance-list.component';
import { AppraisalListComponent } from './components/appraisal-list/appraisal-list.component';
import {AccidentsIncidentsDetailsComponent} from './components/employee-innerpages/accidents-incidents-details/accidents-incidents-details.component';
import {AccidentsIncidentsListComponent} from './components/accidents-incidents-list/accidents-incidents-list.component';
import {LeaveDetailsComponent} from './components/employee-innerpages/leave-details/leave-details.component';
import {LeaveListComponent} from './components/leave-list/leave-list.component';
import {StaffWarningDetailsComponent} from './components/employee-innerpages/staff-warning-details/staff-warning-details.component';
import {StaffWarningListComponent} from './components/staff-warning-list/staff-warning-list.component';
import {PageSubmenuComponent} from './components/page-submenu/page-submenu.component';
import { UserCardComponent } from './components/employee-innerpages/user-card/user-card.component';
import { CommunicationDetailsComponent } from './components/employee-innerpages/communication-details/communication-details.component';
import { AppraisalDetailsComponent } from './components/employee-innerpages/appraisal-details/appraisal-details.component';
import { ComplianceDetailsComponent } from './components/employee-innerpages/compliance-details/compliance-details.component';
import {CommonModule } from '@angular/common';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { LhsServiceModule } from 'projects/lhs-service/src/projects';
import { LhsDirectivesModule } from 'projects/lhs-directives/src/lib/lhs-directives.module';
import { EmployeeTrainingComponent } from './components/employee-training/employee-training.component';
import { EmployeeTodayShiftComponent } from './components/employee-today-shift/employee-today-shift.component';
import { EmployeeLoginPasswordComponent } from './components/employee-login-password/employee-login-password.component';
import { MatGoogleMapsAutocompleteModule } from '@angular-material-extensions/google-maps-autocomplete';
import { AgmCoreModule } from '@agm/core';
import { EmployeeCompliancesComponent } from './components/employee-compliances/employee-compliances.component';
import { EmployeeDocumentchecklistComponent } from './components/employee-documentchecklist/employee-documentchecklist.component';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';

@NgModule({
  declarations: [DashboardComponent, EmployeeComponent, EmployeeDetailComponent, EmployeeAwardsComponent, EmployeeKinInfoComponent,
    EmployeeJobProfileComponent, EmployeeDrivinglicenseComponent, EmployeePayRateseComponent,
    EmployeePrimaryInfoComponent, EmployeeEducationComponent,  EmployeeMiscInfoComponent,
  EmployeeExperienceComponent,
  CommunicationListComponent,
  ComplianceListComponent,
  AppraisalListComponent,
  PageSubmenuComponent, AccidentsIncidentsDetailsComponent,  AccidentsIncidentsListComponent, LeaveDetailsComponent,
  LeaveListComponent, StaffWarningDetailsComponent, StaffWarningListComponent, UserCardComponent, 
  CommunicationDetailsComponent, AppraisalDetailsComponent, ComplianceDetailsComponent, EmployeeTrainingComponent, EmployeeTodayShiftComponent, EmployeeLoginPasswordComponent, EmployeeCompliancesComponent, EmployeeDocumentchecklistComponent,
 
],
  imports: [DashboardRoutingModule, MatTableModule, LhsComponentModule, FormsModule, ReactiveFormsModule, MatPaginatorModule,
    MatFormFieldModule, MatSortModule, MatInputModule, MatSelectModule, MatRadioModule, MatNativeDateModule, MatOptionModule,
    MatDatepickerModule, CommonModule, MatCheckboxModule, LhsServiceModule,LhsDirectivesModule,MatGoogleMapsAutocompleteModule
    , NgxMatSelectSearchModule
  ],
  exports: [DashboardComponent]
})
export class DashboardModule { }
