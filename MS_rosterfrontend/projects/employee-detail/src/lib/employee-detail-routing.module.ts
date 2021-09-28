import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { EmployeedetailsComponent } from './component/employeedetails/employeedetails.component';
import { EmpCommunicationComponent } from './component/emp-communication/emp-communication.component';
import { EmpStaffWarningComponent } from './component/emp-staff-warning/emp-staff-warning.component';
import { EmpLeaveComponent } from './component/emp-leave/emp-leave.component';
import { EmpAccidentIncidentComponent } from './component/emp-accident-incident/emp-accident-incident.component';
import { EmpDocumentchecklistComponent } from './component/emp-documentchecklist/emp-documentchecklist.component';

const routes: Routes = [
 { path: 'employeedetails', component: EmployeedetailsComponent },
 { path: 'employeedetails/:id', component: EmployeedetailsComponent },
 { path: 'communicationdetail', component: EmpCommunicationComponent },
 { path: 'communicationdetail/:id', component: EmpCommunicationComponent },
 { path: 'staffwarning', component: EmpStaffWarningComponent },
 { path: 'staffwarning/:id', component: EmpStaffWarningComponent },
 { path: 'leave', component: EmpLeaveComponent },
 { path: 'leave/:id', component: EmpLeaveComponent },
 { path: 'accidentdetail', component: EmpAccidentIncidentComponent },
 { path: 'accidentdetail/:id', component: EmpAccidentIncidentComponent },
 { path: 'documentchecklist', component: EmpDocumentchecklistComponent },
 { path: 'documentchecklist/:id', component: EmpDocumentchecklistComponent },
];

@NgModule({
    declarations: [],
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class EmployeeDetailRoutingModule { }