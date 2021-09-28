import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { EmployeeComponent } from './components/employee/employee.component';
import { EmployeeDetailComponent } from './components/employee-detail/employee-detail.component';
import { CommunicationListComponent } from './components/communication-list/communication-list.component';
import { ComplianceListComponent } from './components/compliance-list/compliance-list.component';
import { AppraisalListComponent } from './components/appraisal-list/appraisal-list.component';
import {AccidentsIncidentsDetailsComponent} from './components/employee-innerpages/accidents-incidents-details/accidents-incidents-details.component';
import {AccidentsIncidentsListComponent} from './components/accidents-incidents-list/accidents-incidents-list.component';
import {LeaveDetailsComponent} from './components/employee-innerpages/leave-details/leave-details.component';
import {LeaveListComponent} from './components/leave-list/leave-list.component';
import {StaffWarningDetailsComponent} from './components/employee-innerpages/staff-warning-details/staff-warning-details.component';
import {StaffWarningListComponent} from './components/staff-warning-list/staff-warning-list.component';
import { CommunicationDetailsComponent } from './components/employee-innerpages/communication-details/communication-details.component';
import { AppraisalDetailsComponent } from './components/employee-innerpages/appraisal-details/appraisal-details.component';
import { ComplianceDetailsComponent } from './components/employee-innerpages/compliance-details/compliance-details.component';
import { EmployeeDocumentchecklistComponent } from './components/employee-documentchecklist/employee-documentchecklist.component';


const routes: Routes = [
    { path: '', component: EmployeeComponent },
    { path: 'emp-detail/:Id', component: EmployeeDetailComponent },
    { path: 'emp-detail', component: EmployeeDetailComponent },
    { path: 'communication-list', component: CommunicationListComponent },
    { path: 'compliance-list', component: ComplianceListComponent },
    { path: 'appraisal-list', component: AppraisalListComponent },
    { path: 'accidents-incidents-details', component: AccidentsIncidentsDetailsComponent },
    { path: 'accidents-incidents-details/:Id', component: AccidentsIncidentsDetailsComponent },
    { path: 'accident-list', component: AccidentsIncidentsListComponent },
    { path: 'leave-details', component: LeaveDetailsComponent },
    { path: 'leave-details/:Id', component: LeaveDetailsComponent },
    { path: 'leave-list', component: LeaveListComponent },
    { path: 'staffwarning-detail', component: StaffWarningDetailsComponent },
    { path: 'staffwarning-detail/:Id', component: StaffWarningDetailsComponent },
    { path: 'staffwarning-list', component: StaffWarningListComponent },
    { path: 'communication-detail', component: CommunicationDetailsComponent },
    { path: 'compliance-detail', component: ComplianceDetailsComponent },
    { path: 'appraisal-detail', component: AppraisalDetailsComponent },
    { path: 'employeechecklist', component: EmployeeDocumentchecklistComponent },
];

@NgModule({
    declarations: [],
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class DashboardRoutingModule { }