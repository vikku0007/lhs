import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ClientComponent } from './client.component';
import { ClientListComponent } from './components/client-list/client-list.component';
import { ClientDetailsComponent } from './components/client-details/client-details.component';
import { AccidentIncidentListComponent } from './components/accident-incident-list/accident-incident-list.component';
import { DocumentListComponent } from './components/document-list/document-list.component';
import { DocumentDetailsComponent } from './components/document-details/document-details.component';
import { MedicalHistoryListComponent } from './components/medical-history-list/medical-history-list.component';
import { MedicalHistoryDetailsComponent } from './components/medical-history-details/medical-history-details.component';
import { ProgressNotesListComponent } from './components/progress-notes-list/progress-notes-list.component';
import { ProgressNotesDetailsComponent } from './components/progress-notes-details/progress-notes-details.component';
import { ClientPrimaryCareerInfoComponent } from './components/client-primarycareerinfo/client-primarycareerinfo.component';
import { ClientOnboadingnotesComponent } from './components/client-onboadingnotes/client-onboadingnotes.component';
import { ClientAdditionalNotesComponent } from './components/client-additional-notes/client-additional-notes.component';
import { ClientFundinginfoComponent } from './components/client-fundinginfo/client-fundinginfo.component';
import { ClientBasicInfoComponent } from './components/client-basic-info/client-basic-info.component';
import { AccidentIncidentDetailsComponent } from './components/accident-incident-details/accident-incident-details.component';
import { ClientDocumentChecklistComponent } from './components/client-document-checklist/client-document-checklist.component';
import { ClientShiftListComponent } from './components/client-shift-list/client-shift-list.component';
import { MedicalShiftListComponent } from './components/medical-shift-list/medical-shift-list.component';
import { ProgressShiftListComponent } from './components/progress-shift-list/progress-shift-list.component';
// import { ClientComponent } from './components/client/client.component';

const routes: Routes = [
    { path: '', component: ClientListComponent },
    { path: 'client-details', component: ClientDetailsComponent },
    { path: 'accident-details', component: AccidentIncidentDetailsComponent },
    { path: 'accident-list', component: AccidentIncidentListComponent },
    { path: 'document-list', component: DocumentListComponent },
    { path: 'document-details', component: DocumentDetailsComponent },
    { path: 'medical-history-list', component: MedicalHistoryListComponent },
    { path: 'medical-history-details', component: MedicalHistoryDetailsComponent },
    { path: 'progress-notes-list', component: ProgressNotesListComponent },
    { path: 'progress-notes-details', component: ProgressNotesDetailsComponent },
    { path: 'client-primarycareerinfo', component: ClientPrimaryCareerInfoComponent },
    { path: 'client-onboardingnotes', component: ClientOnboadingnotesComponent },
    { path: 'client-additionalnotes', component: ClientAdditionalNotesComponent },
    { path: 'client-fundinginfo', component: ClientFundinginfoComponent },
    { path: 'client-basicinfo', component: ClientBasicInfoComponent },
    { path: 'client-document-checklist', component: ClientDocumentChecklistComponent },
    { path: 'client-shift-list', component: ClientShiftListComponent },
    { path: 'medical-shift-list', component: MedicalShiftListComponent },
    { path: 'progress-shift-list', component: ProgressShiftListComponent }

];

@NgModule({
    declarations: [],
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ClientRoutingModule { }