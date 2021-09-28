import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { IncidentReportedComponent } from './components/incident-reported/incident-reported.component';
import { ComplaintsComponent } from './components/complaints/complaints.component';
import { WarningComponent } from './components/warning/warning.component';
import { DocumentsComponent } from './components/documents/documents.component';
import { CommunicationComponent } from './components/communication/communication.component';
import { IncidentReportedNewComponent } from './components/incident-reported-new/incident-reported-new.component';
import { AccidentIncidentClientDetailsComponent } from './components/accident-incident-client-details/accident-incident-client-details.component';

const routes: Routes = [
    { path: 'incident-reported', component: IncidentReportedNewComponent },
    { path: 'incident-reported/:id', component: IncidentReportedNewComponent },
    { path: 'complaints', component: ComplaintsComponent },
    { path: 'complaints/:id', component: ComplaintsComponent },
    { path: 'warning', component: WarningComponent },
    { path: 'warning/:id', component: WarningComponent },
    { path: 'document', component: DocumentsComponent },
    { path: 'document/:id', component: DocumentsComponent },
    { path: 'communication', component: CommunicationComponent },
    { path: 'communication/:id', component: CommunicationComponent },
    { path: 'client-detail', component: AccidentIncidentClientDetailsComponent },
    { path: 'client-detail/:id', component: AccidentIncidentClientDetailsComponent }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class EmployeeOthersRoutingModule { }