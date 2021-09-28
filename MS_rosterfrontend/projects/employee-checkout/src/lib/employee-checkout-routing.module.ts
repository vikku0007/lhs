import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { LogoutComponent } from './components/logout/logout.component';
import { AccidentIncidentComponent } from './components/accident-incident/accident-incident.component';
import { MedicalHistoryComponent } from './components/medical-history/medical-history.component';
import { ProgressNotesComponent } from './components/progress-notes/progress-notes.component';
import { ClientDocumentsComponent } from './components/client-documents/client-documents.component';
import { CheckoutComponent } from './components/checkout/checkout.component';
import { ToDoListComponent } from './components/to-do-list/to-do-list.component';
import { EditTodoComponent } from './components/edit-todo/edit-todo.component';

const routes: Routes = [
    { path: '', component: LogoutComponent },
    { path: ':id', component: LogoutComponent },
    { path: 'accident-incident', component: AccidentIncidentComponent },
    { path: 'accident-incident/:id/:shiftId', component: AccidentIncidentComponent },
    { path: 'medical-history', component: MedicalHistoryComponent },
    { path: 'medical-history/:id/:shiftId', component: MedicalHistoryComponent },
    { path: 'progress-notes', component: ProgressNotesComponent },
    { path: 'progress-notes/:id/:shiftId', component: ProgressNotesComponent },
    { path: 'client-document', component: ClientDocumentsComponent },
    { path: 'client-document/:id/:shiftId', component: ClientDocumentsComponent },
    { path: 'checkout', component: CheckoutComponent },
    { path: 'to-do-list', component: ToDoListComponent },
    { path: 'to-do-list/:shiftId', component: ToDoListComponent },
    { path: 'edit-todo', component: EditTodoComponent },
    { path: 'edit-todo/:id/:shiftId', component: EditTodoComponent },
   
];

@NgModule({
    declarations: [],
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class EmployeeCheckoutRoutingModule { }